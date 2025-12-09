using VegetableShop.Domain.Exceptions;
using VegetableShop.Domain.Utils;

namespace VegetableShop.Domain.Entities;

/// <summary>
/// Represents an item in a shopping cart or receipt.
/// </summary>
public class CartItem
{
   public Product Product { get; }
   public int Quantity { get; }
   public decimal TotalPrice => Product.Price * Quantity;

   public CartItem(Product product, int quantity)
   {
      Product = product ?? throw new ArgumentNullException(nameof(product));

      if (quantity <= 0)
      {
         throw new InvalidQuantityException(product.Name, quantity.ToString());
      }

      Quantity = quantity;
   }

   public override string ToString() => $"{Product.Name} x{Quantity} - €{FormattingUtils.FormatCurrency(TotalPrice)}";
}