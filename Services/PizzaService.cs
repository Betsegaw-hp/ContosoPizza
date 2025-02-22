using AutoMapper;
using ContosoPizza.DTOs;
using ContosoPizza.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services
{
	public class PizzaService
	{
		private readonly PizzaDbContext _context;
		private readonly IMapper _mapper;


		public PizzaService(PizzaDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<PizzaDto>> GetAll()
		{
			var pizzas = await _context.Pizzas
							.Include(p => p.AvailableSizes)
							.ToListAsync();

			return _mapper.Map<List<PizzaDto>>(pizzas);
		}
		public async Task<PizzaDto?> Get(int id)
		{
			var pizza = await _context.Pizzas
								.Include(p => p.AvailableSizes)
								.FirstOrDefaultAsync(p => p.Id == id);

			return _mapper.Map<PizzaDto>(pizza);
		}

		public async Task<PizzaDto> Add(CreatePizzaDto createPizzaDto)
		{
			var pizza = new Pizza
			{
				Name = createPizzaDto.Name,
				IsGlutenFree = createPizzaDto.IsGlutenFree,
				Category = createPizzaDto.Category,
				AvailableSizes = createPizzaDto.AvailableSizes.Select(cs => new PizzaSize
				{
					Size = cs.Size,
					Price = cs.Price
				}).ToList()
			};

			_context.Pizzas.Add(pizza);
			await _context.SaveChangesAsync();
			return _mapper.Map<PizzaDto>(pizza);
		}

		public async Task<PizzaDto> Update(UpdatePizzaDto updatePizzaDto)
		{
			var pizza = await _context.Pizzas
												.AsNoTracking()
												.Include(p => p.AvailableSizes)
												.FirstOrDefaultAsync(p => p.Id == updatePizzaDto.Id)
								?? throw new KeyNotFoundException($"Pizza with ID {updatePizzaDto.Id} does not exist.");

			// Update scalar properties if provided
			if (!string.IsNullOrEmpty(updatePizzaDto.Name))
				pizza.Name = updatePizzaDto.Name;
			if (updatePizzaDto.IsGlutenFree.HasValue)
				pizza.IsGlutenFree = updatePizzaDto.IsGlutenFree.Value;
			if (updatePizzaDto.Category.HasValue)
				pizza.Category = updatePizzaDto.Category.Value;

			// TODO: I should include update for sizes

			// _context.Pizzas.Update(pizza);
			_context.Entry(pizza).CurrentValues.SetValues(updatePizzaDto);
			_context.Pizzas.Update(pizza);
			await _context.SaveChangesAsync();
			return _mapper.Map<PizzaDto>(pizza);
		}

		public async Task Delete(int id)
		{
			var pizza = await _context.Pizzas.Include(p => p.AvailableSizes)
												.FirstOrDefaultAsync(p => p.Id == id);
			if (pizza != null)
			{
				_context.PizzaSizes.RemoveRange(pizza.AvailableSizes);
				_context.Pizzas.Remove(pizza);
				await _context.SaveChangesAsync();
			}
		}
	}
}
