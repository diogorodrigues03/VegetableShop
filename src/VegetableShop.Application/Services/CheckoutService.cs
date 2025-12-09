using VegetableShop.Application.Interfaces;
using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Exceptions;
using VegetableShop.Domain.Interfaces;

namespace VegetableShop.Application.Services
{
    /// <summary>
    /// Service for processing checkout operations.
    /// </summary>
    public class CheckoutService(
        IProductRepository productRepository,
        IPricingService pricingService,
        IOfferConfigurationService offerConfigurationService)
        : ICheckoutService
    {
        private readonly IProductRepository _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        private readonly IPricingService _pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));
        private readonly IOfferConfigurationService _offerConfigurationService = offerConfigurationService ?? throw new ArgumentNullException(nameof(offerConfigurationService));

        /// <summary>
        /// Processes a purchase and generates a receipt.
        /// </summary>
        /// <param name="purchaseItems">Dictionary of product names to quantities.</param>
        public async Task<Receipt> ProcessPurchaseAsync(Dictionary<string, int> purchaseItems)
        {
            ArgumentNullException.ThrowIfNull(purchaseItems);
            if (purchaseItems.Count == 0)
            {
                throw new InvalidPurchaseDataException("Purchase must contain at least one item.");
            }

            var allProducts = (await _productRepository.GetAllProductsAsync()).ToList();
            if (allProducts.Count == 0)
            {
                throw new InvalidProductDataException("No products found in the catalog.");
            }
            
            var cart = new ShoppingCart();

            foreach (var purchaseItem in purchaseItems)
            {
                var product = allProducts.FirstOrDefault(p => 
                    string.Equals(p.Name, purchaseItem.Key, StringComparison.OrdinalIgnoreCase));

                if (product is null)
                {
                    throw new ProductNotFoundException(purchaseItem.Key);
                }

                cart.AddProduct(product, purchaseItem.Value);
            }

            var offers = _offerConfigurationService.GetOffers(cart, allProducts);

            var receipt = _pricingService.CalculateReceipt(cart, offers);

            return receipt;
        }
    }
}