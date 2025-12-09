using VegetableShop.Application.Interfaces;
using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Interfaces;
using VegetableShop.Domain.Offers;

namespace VegetableShop.Application.Services
{
    /// <summary>
    /// Configures promotional offers based on business rules.
    /// </summary>
    public class OfferConfigurationService : IOfferConfigurationService
    {
        /// <summary>
        /// Gets all configured promotional offers.
        /// </summary>
        public IEnumerable<IPromotionalOffer> GetOffers(ShoppingCart cart, IEnumerable<Product> products)
        {
            ArgumentNullException.ThrowIfNull(cart);
            ArgumentNullException.ThrowIfNull(products);

            // These can be adjusted as needed
            var offers = new List<IPromotionalOffer>
            {
                // Offer 1: Buy 3 Aubergines and pay for 2
                new BuyXPayForYOffer("Aubergine", 3, 2),
                // Offer 2: Get a free Aubergine for every 2 Tomatoes you buy
                new BuyXGetProductYFreeOffer("Tomato", 2, "Aubergine", 1, cart, products),
                // Offer 3: For every 4€ spent on Tomatoes, deduct 1€ from final invoice
                new SpendThresholdDiscountOffer("Tomato", 4m, 1m)
            };

            return offers;
        }
    }
}