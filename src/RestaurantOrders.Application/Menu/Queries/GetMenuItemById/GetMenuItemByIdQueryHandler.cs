namespace RestaurantOrders.Application.Menu.Queries.GetMenuItemById;

using MediatR;
using AutoMapper;
using RestaurantOrders.Application.Common.DTOs;
using RestaurantOrders.Domain.Interfaces.Repositories;

/// <summary>
/// Handler for GetMenuItemByIdQuery
/// </summary>
public class GetMenuItemByIdQueryHandler : IRequestHandler<GetMenuItemByIdQuery, MenuItemDto?>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IMapper _mapper;
    
    public GetMenuItemByIdQueryHandler(IMenuRepository menuRepository, IMapper mapper)
    {
        _menuRepository = menuRepository;
        _mapper = mapper;
    }
    
    public async Task<MenuItemDto?> Handle(GetMenuItemByIdQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement logic to get menu item by id
        var menuItem = await _menuRepository.GetItemByIdAsync(request.MenuItemId, cancellationToken);
        
        if (menuItem == null)
            return null;
        
        return _mapper.Map<MenuItemDto>(menuItem);
    }
}
