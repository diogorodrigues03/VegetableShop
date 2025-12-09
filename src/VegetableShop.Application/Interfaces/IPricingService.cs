using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Interfaces;

namespace VegetableShop.Application.Interfaces
{
    /// <summary>
    /// Defines the pricing engine for calculating totals and applying offers.
    /// </summary>
    public interface IPricingService
    {
        /// <summary>
        /// Applies promotional offers to a shopping cart and generates a receipt.
        /// </summary>
        /// <param name="cart">The shopping cart.</param>
        /// <param name="offers">Available promotional offers.</param>
        /// <returns>A receipt with applied offers.</returns>
        Receipt CalculateReceipt(ShoppingCart cart, IEnumerable<IPromotionalOffer> offers);
    }
}