using VegetableShop.Application.Interfaces;
using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Interfaces;

namespace VegetableShop.Application.Services
{
    /// <summary>
    /// Configures promotional offers based on business rules.
    /// </summary>
    public class OfferConfigurationService(IOfferFactory offerFactory) : IOfferConfigurationService
    {
        private readonly IOfferFactory _offerFactory = offerFactory ?? throw new ArgumentNullException(nameof(offerFactory));

        /// <summary>
        /// Gets all configured promotional offers.
        /// </summary>
        public IEnumerable<IPromotionalOffer> GetOffers(ShoppingCart cart, IEnumerable<Product> products)
        {
            // Safety measure for processing offers only when we have non-null cart and products
            ArgumentNullException.ThrowIfNull(cart);
            ArgumentNullException.ThrowIfNull(products);

            // These can be adjusted as needed
            // If the system was using a database, we would change the implementation
            // to retrieve offers from the database instead of hardcoding them here
            var offers = new List<IPromotionalOffer>
            {
                // Offer 1: Buy 3 Aubergines and pay for 2
                _offerFactory.CreateBuyXPayForYOffer("Aubergine", 3, 2),
                // Offer 2: Get a free Aubergine for every 2 Tomatoes you buy
                _offerFactory.CreateBuyXGetProductYFreeOffer("Tomato", 2, "Aubergine", 1, cart, products),
                // Offer 3: For every 4€ spent on Tomatoes, deduct 1€ from the final invoice
                _offerFactory.CreateSpendThresholdOffer("Tomato", 4m, 1m)
            };

            return offers;
        }
    }
}