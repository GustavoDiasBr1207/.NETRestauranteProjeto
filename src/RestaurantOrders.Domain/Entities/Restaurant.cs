using RestaurantOrders.Domain.Common;

namespace RestaurantOrders.Domain.Entities;

public class Restaurant : BaseEntity
{
    public string  Name      { get; private set; } = string.Empty;
    public string  Slug      { get; private set; } = string.Empty;
    public string? LogoUrl   { get; private set; }
    public bool    IsActive  { get; private set; } = true;

    private readonly List<Table>        _tables     = [];
    private readonly List<MenuCategory> _categories = [];

    public IReadOnlyCollection<Table>        Tables     => _tables.AsReadOnly();
    public IReadOnlyCollection<MenuCategory> Categories => _categories.AsReadOnly();

    private Restaurant() { } // EF Core

    public static Restaurant Create(string name, string slug, string? logoUrl = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        return new Restaurant
        {
            Name    = name.Trim(),
            Slug    = slug.Trim().ToLowerInvariant(),
            LogoUrl = logoUrl
        };
    }

    public void Update(string name, string? logoUrl)
    {
        Name    = name.Trim();
        LogoUrl = logoUrl;
    }

    public void Deactivate() => IsActive = false;
    public void Activate()   => IsActive = true;
}