namespace VegetableShop.Domain.Entities;

/// <summary>
/// Represents a product available in the store.
/// </summary>
public class Product
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }

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
    
    public override string ToString() => $"{Name} - €{Price:F2}";
}