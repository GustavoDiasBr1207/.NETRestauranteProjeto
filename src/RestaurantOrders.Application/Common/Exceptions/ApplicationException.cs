namespace RestaurantOrders.Application.Common.Exceptions;

/// <summary>Exceção genérica da camada de Application para erros de orquestração.</summary>
public class ApplicationException : Exception
{
    public ApplicationException(string message) : base(message) { }

    public ApplicationException(string message, Exception innerException)
        : base(message, innerException) { }
}
