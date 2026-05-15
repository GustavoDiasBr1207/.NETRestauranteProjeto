namespace RestaurantOrders.Application.Common.Mappings;

using AutoMapper;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Application.Common.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrderDto>()
            .ForMember(d => d.Status,      o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.TotalAmount, o => o.MapFrom(s => s.TotalAmount.Amount))
            .ForMember(d => d.Currency,    o => o.MapFrom(s => s.TotalAmount.Currency));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.MenuItemName, o => o.MapFrom(s => s.Name))
            .ForMember(d => d.UnitPrice,    o => o.MapFrom(s => s.UnitPrice.Amount))
            .ForMember(d => d.Subtotal,     o => o.MapFrom(s => s.Subtotal.Amount));

        CreateMap<MenuCategory, MenuCategoryDto>();

        CreateMap<MenuItem, MenuItemDto>()
            .ForMember(d => d.Price, o => o.MapFrom(s => s.Price.Amount));

        CreateMap<Table, TableDto>()
            .ForMember(d => d.QrCodeToken, o => o.MapFrom(s => s.QrCode.Token));
    }
}
