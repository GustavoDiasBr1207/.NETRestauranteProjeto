namespace RestaurantOrders.API.Models;

/// <summary>Corpo padrão de erro retornado pela API.</summary>
public sealed class ErrorResponse
{
    /// <summary>Código HTTP do erro (ex: 400, 404, 422, 500).</summary>
    public int Status { get; init; }

    /// <summary>Descrição legível do erro.</summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>Momento UTC em que o erro ocorreu.</summary>
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}
