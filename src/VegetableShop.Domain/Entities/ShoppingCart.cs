using VegetableShop.Domain.Exceptions;

namespace VegetableShop.Domain.Entities;

/// <summary>
/// Represents a shopping cart containing products to purchase.
/// </summary>
public class ShoppingCart
{
    private readonly Dictionary<Product, int> _items = new();
    public IEnumerable<CartItem> Items => _items.Select(kvp => new CartItem(kvp.Key, kvp.Value));

    public void AddProduct(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        
        if (quantity <= 0)
        {
            throw new InvalidQuantityException(product.Name, quantity.ToString());
        }

        if (!_items.TryAdd(product, quantity))
        {
            _items[product] += quantity;
        }
    }

    public int GetQuantity(Product product) => _items.GetValueOrDefault(product, 0);
    public void Clear() => _items.Clear();
}