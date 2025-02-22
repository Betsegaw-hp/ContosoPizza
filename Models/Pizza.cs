using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Models;

public class Pizza
{
	[Key]
	public int Id { get; set; }

	[Required]
	public string? Name { get; set; } = null!;

	[Required]
	public bool IsGlutenFree { get; set; }

	[Required]
	[Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
	public double Price { get; set; }

}