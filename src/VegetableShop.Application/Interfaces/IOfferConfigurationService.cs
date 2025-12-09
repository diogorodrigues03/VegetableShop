using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Interfaces;

namespace VegetableShop.Application.Interfaces
{
    /// <summary>
    /// Service for configuring promotional offers.
    /// </summary>
    public interface IOfferConfigurationService
    {
        /// <summary>
        /// Gets all configured promotional offers.
        /// </summary>
        /// <param name="cart">The shopping cart (needed for some offer types).</param>
        /// <param name="products">All available products.</param>
        /// <returns>List of promotional offers.</returns>
        IEnumerable<IPromotionalOffer> GetOffers(ShoppingCart cart, IEnumerable<Product> products);
    }
}