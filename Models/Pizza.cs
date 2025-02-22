using System.ComponentModel.DataAnnotations;
using ContosoPizza.Constants;

namespace ContosoPizza.Models
{
	public class Pizza
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string? Name { get; set; } = null!;

		[Required]
		public bool IsGlutenFree { get; set; }

		[Required]
		public PizzaCategory Category { get; set; }

		// nav property for available sizes
		public virtual ICollection<PizzaSize> AvailableSizes { get; set; } = new List<PizzaSize>();
	}

}