namespace RestaurantOrders.Application.Menu.Commands.CreateMenuItem;

using MediatR;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.ValueObjects;
using RestaurantOrders.Domain.Interfaces.Repositories;

/// <summary>
/// Command to create a menu item
/// </summary>
public class CreateMenuItemCommand : IRequest<Guid>
{
    public Guid RestaurantId { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
}

/// <summary>
/// Handler for CreateMenuItemCommand
/// </summary>
public class CreateMenuItemCommandHandler : IRequestHandler<CreateMenuItemCommand, Guid>
{
    private readonly IMenuRepository _menuRepository;
    
    public CreateMenuItemCommandHandler(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }
    
    public async Task<Guid> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement logic to create menu item
        var price = new Money(request.Price);
        var menuItem = MenuItem.Create(request.CategoryId, request.Name, request.Description, price, request.ImageUrl);
        
        await _menuRepository.AddMenuItemAsync(menuItem, cancellationToken);
        
        return menuItem.Id;
    }
}
