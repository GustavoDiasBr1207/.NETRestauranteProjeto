namespace RestaurantOrders.Domain.Interfaces;

/// <summary>
/// Contrato para commit atômico de todas as mudanças rastreadas pela unidade de trabalho.
/// Implementado pelo <c>ApplicationDbContext</c> na camada de Infrastructure.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>Persiste todas as mudanças pendentes no banco de dados.</summary>
    Task<int> CommitAsync(CancellationToken ct = default);
}
