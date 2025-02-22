using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoPizza.Models
{
	public class OrderItem
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("Order")]
		public int OrderId { get; set; }

		[ForeignKey("Pizza")]
		public int PizzaId { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
		public int Quantity { get; set; } = 1;

		// Navigation properties
		public virtual Order Order { get; set; } = null!;
		public virtual Pizza Pizza { get; set; } = null!;
	}
}