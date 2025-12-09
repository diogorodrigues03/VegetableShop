using VegetableShop.Application.Interfaces;
using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Interfaces;
using VegetableShop.Domain.Offers;

namespace VegetableShop.Application.Services
{
    /// <summary>
    /// Factory for creating promotional offer instances.
    /// </summary>
    public class OfferFactory : IOfferFactory
    {
        public IPromotionalOffer CreateBuyXPayForYOffer(string productName, int requiredQuantity, int payForQuantity)
        {
            return new BuyXPayForYOffer(productName, requiredQuantity, payForQuantity);
        }

        public IPromotionalOffer CreateBuyXGetProductYFreeOffer(
            string productName,
            int requiredQuantity,
            string freeProductName,
            int freeProductQuantity,
            ShoppingCart cart,
            IEnumerable<Product> allProducts)
        {
            return new BuyXGetProductYFreeOffer(
                productName, 
                requiredQuantity, 
                freeProductName, 
                freeProductQuantity, 
                cart, 
                allProducts);
        }

        public IPromotionalOffer CreateSpendThresholdOffer(string productName, decimal spendThreshold, decimal discountAmount)
        {
            return new SpendThresholdDiscountOffer(productName, spendThreshold, discountAmount);
        }
    }
}