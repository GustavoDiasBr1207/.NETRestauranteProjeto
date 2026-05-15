namespace RestaurantOrders.Application.Common.DTOs;

/// <summary>Categoria do cardápio com seus itens disponíveis.</summary>
public class MenuCategoryDto
{
    /// <summary>Identificador único da categoria.</summary>
    public Guid Id { get; set; }

    /// <summary>Nome da categoria (ex: "Entradas", "Pratos Principais", "Bebidas").</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Itens disponíveis nesta categoria, ordenados por <c>DisplayOrder</c>.</summary>
    public List<MenuItemDto> Items { get; set; } = new();
}
