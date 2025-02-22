using ContosoPizza.Constants;
using ContosoPizza.DTOs;
using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/[controller]")]
	public class OrderController : ControllerBase
	{
		private readonly ILogger<OrderController> _logger;
		private readonly OrderService _orderServices;
		private readonly IAuthorizationService _authorizationService;


		public OrderController(
			ILogger<OrderController> logger,
			OrderService orderServices,
			IAuthorizationService authorizationService)
		{
			_logger = logger;
			_orderServices = orderServices;
			_authorizationService = authorizationService;
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> GetAll() =>
			Ok(await _orderServices.GetAll());


		[HttpGet("{id}")]
		public async Task<IActionResult> GetOrder(int id)
		{
			var order = await _orderServices.Get(id);

			var authResult = await _authorizationService.AuthorizeAsync(User, order?.Id, PolicyKeyWords.OrderOwnerOrAdminPolicy.ToString());
			if (!authResult.Succeeded)
				return StatusCode(403, "You are not authorized to view this order.");

			return Ok(order);
		}

		[HttpGet("user/{userId}")]
		public async Task<IActionResult> GetAllOrdersByUserId(int userId)
		{
			List<OrderDto> orders = await _orderServices.GetOrdersByUser(userId);

			foreach (var order in orders)
			{
				var authResult = await _authorizationService.AuthorizeAsync(User, order.Id, PolicyKeyWords.OrderOwnerOrAdminPolicy.ToString());
				if (!authResult.Succeeded)
				{
					return StatusCode(403, "You are not authorized to view these orders or atleast an order.");
				}
			}

			return Ok(orders);
		}

		[HttpPost]
		public async Task<IActionResult> MakeOrder(CreateOrderDto createOrderDto)
		{

			var order = await _orderServices.Create(createOrderDto);
			return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, await _orderServices.Get(order.Id));
		}

		[HttpPatch("{id}/cancel")]
		public async Task<IActionResult> CancelOrder(int id)
		{
			var order = await _orderServices.Get(id);
			if (order == null) return NotFound();

			var authResult = await _authorizationService.AuthorizeAsync(User, order.Id, PolicyKeyWords.OrderOwnerOrAdminPolicy.ToString());
			if (!authResult.Succeeded)
				return StatusCode(403, "You are not authorized to cancel this order.");

			await _orderServices.Cancel(id);
			return StatusCode(202, new { message = $"Order with id: {id} is cancelled!" });
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteOrder(int id)
		{
			await _orderServices.Delete(id);
			return StatusCode(202, new { message = $"Order with id: {id} deleted" });
		}


	}
}