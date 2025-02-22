using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Models
{
	public enum PizzaSizeSymbol { S, M, L, XL }

	[Index(nameof(PizzaId), nameof(Size), IsUnique = true)]
	public class PizzaSize
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("Pizza")]
		public int PizzaId { get; set; }

		[Required]
		public PizzaSizeSymbol Size { get; set; }

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		[Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
		public decimal Price { get; set; }

		// nav
		public Pizza Pizza { get; set; } = null!;
	}

}