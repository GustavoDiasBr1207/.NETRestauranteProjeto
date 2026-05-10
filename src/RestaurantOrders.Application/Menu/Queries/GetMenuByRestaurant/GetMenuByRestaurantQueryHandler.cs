namespace RestaurantOrders.Application.Menu.Queries.GetMenuByRestaurant;

using MediatR;
using AutoMapper;
using RestaurantOrders.Application.Common.DTOs;
using RestaurantOrders.Domain.Interfaces.Repositories;

/// <summary>
/// Handler for GetMenuByRestaurantQuery
/// </summary>
public class GetMenuByRestaurantQueryHandler : IRequestHandler<GetMenuByRestaurantQuery, List<MenuCategoryDto>>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IMapper _mapper;
    
    public GetMenuByRestaurantQueryHandler(IMenuRepository menuRepository, IMapper mapper)
    {
        _menuRepository = menuRepository;
        _mapper = mapper;
    }
    
    public async Task<List<MenuCategoryDto>> Handle(GetMenuByRestaurantQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement logic to get menu by restaurant
        var categories = await _menuRepository.GetCategoriesWithItemsAsync(request.RestaurantId, cancellationToken);
        return _mapper.Map<List<MenuCategoryDto>>(categories);
    }
}
