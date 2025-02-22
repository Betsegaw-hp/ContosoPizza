using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PizzaController : ControllerBase
{
	private readonly ILogger<PizzaController> _logger;
	private readonly PizzaService _pizzaServices;
	public PizzaController(ILogger<PizzaController> logger, PizzaService pizzaServices)
	{
		_logger = logger;
		_pizzaServices = pizzaServices;
	}

	// GET all action
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		return Ok(await _pizzaServices.GetAll());
	}
	// GET by Id action
	[HttpGet("{id}")]
	public async Task<IActionResult> GetOne(int id)
	{
		var pizza = await _pizzaServices.Get(id);
		if (pizza == null) return NotFound();
		return Ok(pizza);
	}

	// POST action
	[Authorize(Roles = "Admin")]
	[HttpPost]
	public async Task<IActionResult> Create(Pizza pizza)
	{
		var p = await _pizzaServices.Get(pizza.Id);
		if (p != null)
			return BadRequest($"pizza with id: {pizza.Id} exists already!");
		await _pizzaServices.Add(pizza);
		return CreatedAtAction(nameof(GetOne), new { id = pizza.Id }, await _pizzaServices.Get(pizza.Id));
	}

	// PUT action
	[Authorize(Roles = "Admin")]
	[HttpPut("{id}")]
	public async Task<IActionResult> Update(int id, [FromBody] Pizza pizza)
	{
		if (id != pizza.Id)
			return BadRequest($"The request body's Id: {pizza.Id} value doesn't match the route's id: {id} value.");

		var existingPizza = await _pizzaServices.Get(id);
		if (existingPizza == null)
			return NotFound($"The pizza with id: {id} doesn't exist!");

		existingPizza.Name = pizza.Name;
		existingPizza.IsGlutenFree = pizza.IsGlutenFree;
		await _pizzaServices.Update(existingPizza);
		return NoContent();
	}
	// DELETE action
	[Authorize(Roles = "Admin")]
	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		try
		{
			await _pizzaServices.Delete(id);
			return NoContent();
		}
		catch (Exception)
		{
			return BadRequest("Error: Pizza could not be deleted.");
		}
	}
}
