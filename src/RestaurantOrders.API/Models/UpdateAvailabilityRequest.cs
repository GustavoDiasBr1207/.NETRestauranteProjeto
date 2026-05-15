namespace RestaurantOrders.API.Models;

/// <summary>Body para atualizar a disponibilidade de um item do cardápio.</summary>
public sealed class UpdateAvailabilityRequest
{
    /// <summary><c>true</c> para habilitar o item; <c>false</c> para desabilitar.</summary>
    public bool IsAvailable { get; init; }
}
