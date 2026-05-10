namespace RestaurantOrders.Application.Common.Mappings;

using AutoMapper;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Application.Common.DTOs;

/// <summary>
/// AutoMapper profile for mapping entities to DTOs
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Order mappings
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.PlacedAt, opt => opt.MapFrom(src => src.SubmittedAt ?? src.CreatedAt));
        
        CreateMap<OrderItem, OrderItemDto>();
        
        // Menu mappings
        CreateMap<MenuCategory, MenuCategoryDto>();
        CreateMap<MenuItem, MenuItemDto>();
        
        // Table mappings
        CreateMap<Table, TableDto>();
    }
}
