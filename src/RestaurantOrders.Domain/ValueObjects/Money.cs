namespace RestaurantOrders.Domain.ValueObjects;

/// <summary>
/// Value object representing money with amount and currency
/// Immutable, supports operators like +, and ensures currency consistency
/// </summary>
public class Money : IEquatable<Money>
{
    public decimal Amount { get; init; }
    public string Currency { get; init; } = "BRL";
    
    public Money(decimal amount, string currency = "BRL")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative");
        
        Amount = amount;
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
    }
    
    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException($"Cannot add different currencies: {left.Currency} and {right.Currency}");
        
        return new Money(left.Amount + right.Amount, left.Currency);
    }
    
    public static Money operator -(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException($"Cannot subtract different currencies: {left.Currency} and {right.Currency}");
        
        return new Money(left.Amount - right.Amount, left.Currency);
    }
    
    public bool Equals(Money? other)
    {
        if (other is null) return false;
        return Amount == other.Amount && Currency == other.Currency;
    }
    
    public override bool Equals(object? obj) => Equals(obj as Money);
    
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    
    public override string ToString() => $"{Currency} {Amount:F2}";
}
