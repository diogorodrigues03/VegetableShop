using VegetableShop.Domain.Entities;

namespace VegetableShop.Application.Interfaces
{
    /// <summary>
    /// Defines the checkout service for processing purchases.
    /// </summary>
    public interface ICheckoutService
    {
        /// <summary>
        /// Processes a purchase and generates a receipt.
        /// </summary>
        /// <param name="purchaseItems">Dictionary of product names to quantities.</param>
        /// <returns>A receipt for the purchase.</returns>
        Task<Receipt> ProcessPurchaseAsync(Dictionary<string, int> purchaseItems);
    }
}