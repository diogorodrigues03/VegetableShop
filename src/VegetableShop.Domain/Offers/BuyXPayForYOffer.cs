using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Interfaces;

namespace VegetableShop.Domain.Offers
{
    /// <summary>
    /// Represents a "Buy X Pay for Y" promotional offer.
    /// Example: Buy 3, pay for 2.
    /// </summary>
    public class BuyXPayForYOffer : IPromotionalOffer
    {
        public string ProductName { get; }
        public int RequiredQuantity { get; }
        public int PayForQuantity { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuyXPayForYOffer"/> class.
        /// </summary>
        /// <param name="productName">The product this offer applies to.</param>
        /// <param name="requiredQuantity">The quantity that triggers the offer.</param>
        /// <param name="payForQuantity">The quantity to pay for.</param>
        public BuyXPayForYOffer(string productName, int requiredQuantity, int payForQuantity)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Product name cannot be empty.", nameof(productName));
            
            if (requiredQuantity <= 0)
                throw new ArgumentException("Required quantity must be positive.", nameof(requiredQuantity));
            
            if (payForQuantity <= 0)
                throw new ArgumentException("Pay for quantity must be positive.", nameof(payForQuantity));
            
            if (payForQuantity >= requiredQuantity)
                throw new ArgumentException("Pay for quantity must be less than required quantity.", nameof(payForQuantity));

            ProductName = productName;
            RequiredQuantity = requiredQuantity;
            PayForQuantity = payForQuantity;
        }

        /// <summary>
        /// Calculates the discount for the given item.
        /// </summary>
        public AppliedOffer? CalculateDiscount(CartItem item)
        {
            ArgumentNullException.ThrowIfNull(item);
            
            if (!string.Equals(item.Product.Name, ProductName, StringComparison.OrdinalIgnoreCase))
                return null;
            
            if (item.Quantity < RequiredQuantity)
                return null;

            var offerApplications = item.Quantity / RequiredQuantity;
            var freeItems = offerApplications * (RequiredQuantity - PayForQuantity);
            var discountAmount = freeItems * item.Product.Price;

            if (discountAmount <= 0)
                return null;

            var description = $"{ProductName}: Buy {RequiredQuantity} Pay for {PayForQuantity} " +
                            $"(Applied {offerApplications}x, {freeItems} free)";

            return new AppliedOffer(description, discountAmount);
        }
    }
}