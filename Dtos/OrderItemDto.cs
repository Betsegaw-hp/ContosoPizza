using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.DTOs
{
	public class OrderItemDto
	{
		public int Id { get; set; }
		public int PizzaSizeId { get; set; }
		public string PizzaName { get; set; } = string.Empty;
		public bool IsGlutenFree { get; set; }
		public decimal UnitPrice { get; set; }  // Price at order time
		public int Quantity { get; set; }

		public async void PopulatePizaName(PizzaDbContext context, int id)
		{
			PizzaName = await context.Pizzas
							.Where(p => p.Id == id)
							.Select(p => p.Name)
							.FirstOrDefaultAsync() ?? "";
		}
	}

	public class CreateOrderItemDto
	{
		public int PizzaSizeId { get; set; }
		public int Quantity { get; set; }
	}
}
