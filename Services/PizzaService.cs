using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services
{
	public class PizzaService
	{
		private readonly PizzaDbContext _context;

		public PizzaService(PizzaDbContext context)
		{
			_context = context;
		}

		public async Task<List<Pizza>> GetAll() => await _context.Pizzas.ToListAsync();

		public async Task<Pizza?> Get(int id) => await _context.Pizzas.FindAsync(id);

		public async Task Add(Pizza pizza)
		{
			_context.Pizzas.Add(pizza);
			await _context.SaveChangesAsync();
		}

		public async Task Update(Pizza pizza)
		{
			var existingPizza = await _context.Pizzas.AsNoTracking().FirstOrDefaultAsync(p => p.Id == pizza.Id)
								?? throw new KeyNotFoundException($"Pizza with ID {pizza.Id} does not exist.");

			// _context.Pizzas.Update(pizza);
			_context.Entry(existingPizza).CurrentValues.SetValues(pizza); // This updates only the modified properties
			_context.Pizzas.Update(pizza);
			await _context.SaveChangesAsync();
		}

		public async Task Delete(int id)
		{
			var pizza = await _context.Pizzas.FindAsync(id);
			if (pizza != null)
			{
				_context.Pizzas.Remove(pizza);
				await _context.SaveChangesAsync();
			}
		}
	}
}
