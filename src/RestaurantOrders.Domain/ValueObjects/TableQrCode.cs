namespace RestaurantOrders.Domain.ValueObjects;

/// <summary>
/// Value object representing a unique QR code token for a table
/// </summary>
public class TableQrCode : IEquatable<TableQrCode>
{
    public string Token { get; init; }
    
    public TableQrCode(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token cannot be empty or whitespace", nameof(token));
        
        Token = token;
    }
    
    public static TableQrCode Generate() => new(Guid.NewGuid().ToString("N"));
    
    public bool Equals(TableQrCode? other)
    {
        if (other is null) return false;
        return Token == other.Token;
    }
    
    public override bool Equals(object? obj) => Equals(obj as TableQrCode);
    
    public override int GetHashCode() => Token.GetHashCode();
    
    public override string ToString() => Token;
}
