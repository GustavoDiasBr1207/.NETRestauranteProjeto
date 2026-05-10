namespace RestaurantOrders.Application.Common.Exceptions;

/// <summary>
/// Application layer exception
/// </summary>
public class ApplicationException : Exception
{
    public ApplicationException(string message) : base(message) { }
    
    public ApplicationException(string message, Exception innerException) 
        : base(message, innerException) { }
}
