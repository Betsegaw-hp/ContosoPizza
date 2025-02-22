using AutoMapper;
using ContosoPizza.Models;
using ContosoPizza.DTOs;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Order, OrderDto>();
		CreateMap<OrderItem, OrderItemDto>();
		CreateMap<Pizza, PizzaDto>();
		CreateMap<PizzaSize, PizzaSizeDto>();
		CreateMap<User, UserDto>();

		CreateMap<CreatePizzaDto, Pizza>();
		CreateMap<CreatePizzaSizeDto, PizzaSize>();

		CreateMap<UpdatePizzaDto, Pizza>();
		CreateMap<UpdatePizzaSizeDto, PizzaSize>();
	}
}
