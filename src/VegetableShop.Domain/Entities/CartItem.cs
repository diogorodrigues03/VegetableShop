namespace VegetableShop.Domain.Entities;

/// <summary>
/// Represents an item in a shopping cart or receipt.
/// </summary>
public class CartItem
{
   public Product Product { get; private set; }
   public int Quantity { get; private set; }
   public decimal TotalPrice => Product.Price * Quantity;

   public CartItem(Product product, int quantity)
   {
      Product = product ?? throw new ArgumentNullException(nameof(product));

      if (quantity <= 0)
      {
         throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
      }

      Quantity = quantity;
   }

   public override string ToString() => $"{Product.Name} x{Quantity} - {TotalPrice:F2}";
}