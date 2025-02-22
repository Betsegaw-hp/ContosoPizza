namespace ContosoPizza.DTOs
{
	public class OrderItemDto
	{
		public int Id { get; set; }
		public int PizzaId { get; set; }
		public string PizzaName { get; set; } = string.Empty;
		public bool IsGlutenFree { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}

	public class CreateOrderItemDto
	{
		public int PizzaId { get; set; }
		public int Quantity { get; set; }
	}
}