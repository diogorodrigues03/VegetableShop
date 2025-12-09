using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Interfaces;

namespace VegetableShop.Application.Interfaces
{
    /// <summary>
    /// Factory for creating promotional offers.
    /// </summary>
    public interface IOfferFactory
    {
        /// <summary>
        /// Creates a "Buy X Pay for Y" offer.
        /// </summary>
        /// <param name="productName">The product this offer applies to.</param>
        /// <param name="requiredQuantity">The quantity that triggers the offer.</param>
        /// <param name="payForQuantity">The quantity to pay for.</param>
        /// <returns>An <see cref="IPromotionalOffer"/> representing the "Buy X Pay for Y" offer.</returns>
        IPromotionalOffer CreateBuyXPayForYOffer(string productName, int requiredQuantity, int payForQuantity);

        /// <summary>
        /// Creates a "Buy X Get Product Y Free" offer.
        /// </summary>
        /// <param name="productName">The product this offer applies to.</param>
        /// <param name="requiredQuantity">The quantity that triggers the offer.</param>
        /// <param name="freeProductName">The product that is given free.</param>
        /// <param name="freeProductQuantity">The quantity of the free product.</param>
        /// <param name="cart">The shopping cart.</param>
        /// <param name="allProducts">All products in the store.</param>
        /// <returns>An <see cref="IPromotionalOffer"/> representing the "Buy X Get Product Y Free" offer.</returns>
        IPromotionalOffer CreateBuyXGetProductYFreeOffer(
            string productName, 
            int requiredQuantity, 
            string freeProductName, 
            int freeProductQuantity,
            ShoppingCart cart,
            IEnumerable<Product> allProducts);

        /// <summary>
        /// Creates a "Spend Threshold Discount" offer.
        /// </summary>
        /// <param name="productName">The product this offer applies to.</param>
        /// <param name="spendThreshold">The amount that must be spent to trigger the discount.</param>
        /// <param name="discountAmount">The discount amount per threshold reached.</param>
        /// <returns>An <see cref="IPromotionalOffer"/> representing the "Spend Threshold Discount" offer.</returns>
        IPromotionalOffer CreateSpendThresholdOffer(string productName, decimal spendThreshold, decimal discountAmount);
    }
}