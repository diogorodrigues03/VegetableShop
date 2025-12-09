namespace VegetableShop.Domain.Entities;

/// <summary>
/// Represents a product available in the store.
/// </summary>
public class Product
{
    public string Name { get; }
    public decimal Price { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Product"/> class.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="price">The unit price.</param>
    /// <exception cref="ArgumentException">Thrown when the name is empty or the price is negative.</exception>
    public Product(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name cannot be null or empty.", nameof(name));
        }

        if (price < 0)
        {
            throw new ArgumentException("Product price cannot be negative.", nameof(price));
        }

        Name = name.Trim();
        Price = price;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is null || GetType() != obj.GetType()) return false;
        
        var other = (Product)obj;
        return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return Name.ToLowerInvariant().GetHashCode();
    }

    public override string ToString() => $"{Name} - €{Price.ToString("F2", System.Globalization.CultureInfo.CurrentCulture)}";
}