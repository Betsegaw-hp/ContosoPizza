using AutoMapper;
using ContosoPizza.Models;
using ContosoPizza.DTOs;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Order, OrderDto>()
				.ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Status)); ;
		CreateMap<OrderItem, OrderItemDto>();
		CreateMap<Pizza, PizzaDto>();
		CreateMap<User, UserDto>();
	}
}
