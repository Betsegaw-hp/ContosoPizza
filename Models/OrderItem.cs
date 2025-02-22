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

		[ForeignKey("PizzaSize")]
		public int PizzaSizeId { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
		public int Quantity { get; set; } = 1;

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		[Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
		public decimal UnitPrice { get; set; }

		// nav properties
		public virtual Order Order { get; set; } = null!;
		public virtual PizzaSize PizzaSize { get; set; } = null!;
	}
}
