using ContosoPizza.Models;

namespace ContosoPizza.DTOs
{
	public class OrderDto
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string Username { get; set; } = string.Empty;
		public DateTime Timestamp { get; set; }
		public OrderStatus OrderStatus { get; set; }
		public List<OrderItemDto> OrderItems { get; set; } = [];
	}

	public class CreateOrderDto
	{
		public List<CreateOrderItemDto> OrderItems { get; set; } = new();
	}
}