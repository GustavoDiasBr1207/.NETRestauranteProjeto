namespace RestaurantOrders.Domain.ValueObjects;

/// <summary>
/// Value Object imutável que representa um valor monetário com moeda.
/// Padrão da plataforma: BRL. Operadores +, -, * garantem invariantes de moeda.
/// </summary>
public class Money : IEquatable<Money>
{
    public decimal Amount   { get; private set; }
    public string  Currency { get; private set; } = "BRL";

    private Money() { Amount = 0; Currency = "BRL"; } // EF Core

    public Money(decimal amount, string currency = "BRL")
    {
        if (amount < 0)
            throw new ArgumentException("O valor monetário não pode ser negativo.");

        Amount   = amount;
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
    }

    public static Money Zero(string currency = "BRL") => new(0, currency);

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException(
                $"Não é possível somar moedas diferentes: {left.Currency} e {right.Currency}.");
        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public static Money operator -(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException(
                $"Não é possível subtrair moedas diferentes: {left.Currency} e {right.Currency}.");
        return new Money(left.Amount - right.Amount, left.Currency);
    }

    public static Money operator *(Money money, int multiplier)
    {
        if (multiplier < 0)
            throw new ArgumentException("O multiplicador não pode ser negativo.");
        return new Money(money.Amount * multiplier, money.Currency);
    }

    public bool Equals(Money? other)
    {
        if (other is null) return false;
        return Amount == other.Amount && Currency == other.Currency;
    }

    public override bool Equals(object? obj) => Equals(obj as Money);
    public override int  GetHashCode()        => HashCode.Combine(Amount, Currency);
    public override string ToString()         => $"{Currency} {Amount:F2}";
}
