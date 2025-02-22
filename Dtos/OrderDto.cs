using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.DTOs
{
	public class OrderDto
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string Username { get; set; } = string.Empty;
		public DateTime Timestamp { get; set; }
		public OrderStatus Status { get; set; }
		public decimal TotalPrice { get; set; }
		public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

		public async void PolpulateUsername(PizzaDbContext context, int id)
		{
			Username = await context.Users
				.Where(u => u.Id == id)
				.Select(u => u.Username)
				.FirstOrDefaultAsync() ??
				"";
		}
	}

	public class CreateOrderDto
	{
		public List<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();
	}
}
