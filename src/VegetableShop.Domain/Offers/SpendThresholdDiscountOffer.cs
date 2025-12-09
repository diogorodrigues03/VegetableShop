using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Interfaces;

namespace VegetableShop.Domain.Offers
{
    /// <summary>
    /// Represents an offer where spending a threshold amount gives a discount.
    /// Example: Spend €4 on Tomatoes, get €1 off.
    /// </summary>
    public class SpendThresholdDiscountOffer : IPromotionalOffer
    {
        public string ProductName { get; }
        public decimal SpendThreshold { get; }
        public decimal DiscountAmount { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpendThresholdDiscountOffer"/> class.
        /// </summary>
        /// <param name="productName">The product this offer applies to.</param>
        /// <param name="spendThreshold">The amount that must be spent to trigger the discount.</param>
        /// <param name="discountAmount">The discount amount per threshold reached.</param>
        public SpendThresholdDiscountOffer(string productName, decimal spendThreshold, decimal discountAmount)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Product name cannot be empty.", nameof(productName));
            
            if (spendThreshold <= 0)
                throw new ArgumentException("Spend threshold must be positive.", nameof(spendThreshold));
            
            if (discountAmount <= 0)
                throw new ArgumentException("Discount amount must be positive.", nameof(discountAmount));
            
            if (discountAmount >= spendThreshold)
                throw new ArgumentException("Discount cannot be greater than or equal to spend threshold.", nameof(discountAmount));

            ProductName = productName;
            SpendThreshold = spendThreshold;
            DiscountAmount = discountAmount;
        }

        /// <summary>
        /// Calculates the discount for the given item.
        /// </summary>
        public AppliedOffer? CalculateDiscount(CartItem item)
        {
            ArgumentNullException.ThrowIfNull(item);

            if (!string.Equals(item.Product.Name, ProductName, StringComparison.OrdinalIgnoreCase))
                return null;

            var totalSpent = item.TotalPrice;

            if (totalSpent < SpendThreshold)
                return null;

            var thresholdReached = (int)(totalSpent / SpendThreshold);
            var totalDiscount = thresholdReached * DiscountAmount;

            if (totalDiscount <= 0)
                return null;

            var description = $"{ProductName}: Spend €{SpendThreshold:F2} get €{DiscountAmount:F2} off " +
                            $"(Threshold reached {thresholdReached}x)";

            return new AppliedOffer(description, totalDiscount);
        }
    }
}