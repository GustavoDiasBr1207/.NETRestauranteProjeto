namespace RestaurantOrders.Domain.ValueObjects;

public class TableQrCode : IEquatable<TableQrCode>
{
    public string Token { get; private set; } = string.Empty;

    private TableQrCode() { } // EF Core

    public TableQrCode(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token não pode ser vazio.", nameof(token));

        Token = token;
    }

    public static TableQrCode Generate() => new(Guid.NewGuid().ToString("N"));

    public bool Equals(TableQrCode? other) => other is not null && Token == other.Token;
    public override bool Equals(object? obj) => Equals(obj as TableQrCode);
    public override int GetHashCode() => Token.GetHashCode();
    public override string ToString() => Token;
}
