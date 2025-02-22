using ContosoPizza.Constants;
using ContosoPizza.Models;

namespace ContosoPizza.DTOs
{
	public class PizzaDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public bool IsGlutenFree { get; set; }
		public PizzaCategory Category { get; set; }

		public List<PizzaSizeDto> AvailableSizes { get; set; } = new List<PizzaSizeDto>();
	}

	public class PizzaSizeDto
	{
		public int Id { get; set; }
		public PizzaSizeSymbol Size { get; set; }
		public decimal Price { get; set; }
	}

	public class UpdatePizzaDto
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public bool? IsGlutenFree { get; set; }
		public PizzaCategory? Category { get; set; }
		// If provided, this list replaces or updates the existing sizes.
		public List<UpdatePizzaSizeDto>? AvailableSizes { get; set; }
	}

	public class UpdatePizzaSizeDto
	{
		public int? Id { get; set; }
		public PizzaSizeSymbol? Size { get; set; }
		public decimal? Price { get; set; }
	}

	public class CreatePizzaDto
	{
		public string Name { get; set; } = string.Empty;
		public bool IsGlutenFree { get; set; }
		public PizzaCategory Category { get; set; }
		public List<CreatePizzaSizeDto> AvailableSizes { get; set; } = new();
	}

	public class CreatePizzaSizeDto
	{
		public PizzaSizeSymbol Size { get; set; }
		public decimal Price { get; set; }
	}
}
