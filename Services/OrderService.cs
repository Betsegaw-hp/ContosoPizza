using System.Security.Claims;
using AutoMapper;
using ContosoPizza.DTOs;
using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services
{
	public class OrderService
	{
		private readonly PizzaDbContext _context;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public OrderService(PizzaDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
		}


		public async Task<List<OrderDto>> GetAll()
		{
			var orders = await _context.Orders
								.AsNoTracking()
								.Include(o => o.OrderItems)
								.ThenInclude(oi => oi.Pizza)
								.OrderByDescending(o => o.Timestamp)
								.ToListAsync();

			return _mapper.Map<List<OrderDto>>(orders);
		}
		public async Task<OrderDto?> Get(int id)
		{
			var order = await _context.Orders
								.AsNoTracking()
								.Include(o => o.OrderItems)
								.ThenInclude(oi => oi.Pizza)
								.FirstOrDefaultAsync(o => o.Id == id);

			return _mapper.Map<OrderDto>(order);
		}
		public async Task<List<OrderDto>> GetOrdersByUser(int userId)
		{
			List<Order> orders = await _context.Orders
					.AsNoTracking()
					.Where(order => order.UserId == userId)
					.Include(o => o.OrderItems)
					.ThenInclude(oi => oi.Pizza)
					.OrderByDescending(o => o.Timestamp)
					.ToListAsync();

			return _mapper.Map<List<OrderDto>>(orders);
		}
		public async Task<OrderDto> Create(CreateOrderDto createOrderDto)
		{
			if (createOrderDto.OrderItems == null || createOrderDto.OrderItems.Count == 0)
				throw new ArgumentException("Order must contain at least one item.");

			var requestedPizzaIds = createOrderDto.OrderItems.Select(oi => oi.PizzaId).ToList();
			var existingPizzaIds = await _context.Pizzas
				.Where(p => requestedPizzaIds.Contains(p.Id))
				.Select(p => p.Id)
				.ToListAsync();

			if (requestedPizzaIds.Except(existingPizzaIds).Any())
				throw new InvalidOperationException("One or more PizzaId(s) are invalid.");

			var userId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
			var order = new Order
			{
				UserId = userId,
				OrderItems = createOrderDto.OrderItems.Select(oi => new OrderItem
				{
					PizzaId = oi.PizzaId,
					Quantity = oi.Quantity
				}).ToList()
			};


			_context.Orders.Add(order);
			await _context.SaveChangesAsync();

			return _mapper.Map<OrderDto>(order);
		}


		public async Task Cancel(int orderId)
		{
			var order = await _context.Orders.FindAsync(orderId) ?? throw new KeyNotFoundException($"Order with ID {orderId} not found.");
			if (order.Status != OrderStatus.Pending)
				throw new InvalidOperationException("Order cannot be cancelled once processed.");

			order.Status = OrderStatus.Cancelled;
			_context.Orders.Update(order);
			await _context.SaveChangesAsync();
		}


		public async Task Delete(int id)
		{
			var order = await _context.Orders
				.Include(o => o.OrderItems)
				.FirstOrDefaultAsync(o => o.Id == id);

			if (order is not null)
			{
				_context.OrderItems.RemoveRange(order.OrderItems);
				_context.Orders.Remove(order);
				await _context.SaveChangesAsync();
			}

		}
	}
}