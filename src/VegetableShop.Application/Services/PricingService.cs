using VegetableShop.Application.Interfaces;
using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Interfaces;

namespace VegetableShop.Application.Services
{
    /// <summary>
    /// Engine for calculating prices and applying promotional offers.
    /// </summary>
    public class PricingService : IPricingService
    {
        /// <summary>
        /// Applies promotional offers to a shopping cart and generates a receipt.
        /// </summary>
        public Receipt CalculateReceipt(ShoppingCart cart, IEnumerable<IPromotionalOffer> offers)
        {
            ArgumentNullException.ThrowIfNull(cart);
            ArgumentNullException.ThrowIfNull(offers);

            var receipt = new Receipt();
            var cartItems = cart.Items.ToList();
            receipt.AddItems(cartItems);

            var appliedOffers = new List<AppliedOffer>();

            foreach (var offer in offers)
            {
                appliedOffers.AddRange(cartItems.Select(cartItem => offer.CalculateDiscount(cartItem)).OfType<AppliedOffer>());
            }

            if (appliedOffers.Count != 0)
            {
                receipt.ApplyOffers(appliedOffers);
            }

            return receipt;
        }
    }
}