using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Exceptions;
using VegetableShop.Domain.Interfaces;

namespace VegetableShop.Domain.Offers
{
    /// <summary>
    /// Represents an offer where buying X of product A gives you product B for free.
    /// Example: Buy 2 Tomatoes, get 1 Aubergine free.
    /// </summary>
    public class BuyXGetProductYFreeOffer : IPromotionalOffer
    {
        public string ProductName { get; }
        public int RequiredQuantity { get; }
        public string FreeProductName { get; }
        public int FreeProductQuantity { get; }

        private readonly ShoppingCart _cart;
        private readonly IEnumerable<Product> _allProducts;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuyXGetProductYFreeOffer"/> class.
        /// </summary>
        /// <param name="productName">The product this offer applies to.</param>
        /// <param name="requiredQuantity">The quantity that triggers the offer.</param>
        /// <param name="freeProductName">The product that is given free.</param>
        /// <param name="freeProductQuantity">The quantity of the free product.</param>
        /// <param name="cart">The shopping cart.</param>
        /// <param name="allProducts">All products in the store.</param>
        public BuyXGetProductYFreeOffer(
            string productName, 
            int requiredQuantity, 
            string freeProductName, 
            int freeProductQuantity,
            ShoppingCart cart,
            IEnumerable<Product> allProducts)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Product name cannot be empty.", nameof(productName));
            
            if (string.IsNullOrWhiteSpace(freeProductName))
                throw new ArgumentException("Free product name cannot be empty.", nameof(freeProductName));
            
            if (requiredQuantity <= 0)
                throw new InvalidQuantityException("Required quantity must be positive.", nameof(requiredQuantity));
            
            if (freeProductQuantity <= 0)
                throw new InvalidQuantityException("Free product quantity must be positive.", nameof(freeProductQuantity));

            ProductName = productName;
            RequiredQuantity = requiredQuantity;
            FreeProductName = freeProductName;
            FreeProductQuantity = freeProductQuantity;
            _cart = cart ?? throw new ArgumentNullException(nameof(cart));
            _allProducts = allProducts ?? throw new ArgumentNullException(nameof(allProducts));
        }

        /// <summary>
        /// Calculates the discount for the given item.
        /// </summary>
        public AppliedOffer? CalculateDiscount(CartItem item)
        {
            ArgumentNullException.ThrowIfNull(item);

            if (!string.Equals(item.Product.Name, FreeProductName, StringComparison.OrdinalIgnoreCase))
                return null;

            var triggerProduct = _allProducts.FirstOrDefault(p => string.Equals(p.Name, ProductName, StringComparison.OrdinalIgnoreCase));

            if (triggerProduct == null)
                return null;

            var triggerQuantity = _cart.GetQuantity(triggerProduct);

            if (triggerQuantity < RequiredQuantity)
                return null;

            var offerApplications = triggerQuantity / RequiredQuantity;
            var totalFreeItems = offerApplications * FreeProductQuantity;
            var discountedItems = Math.Min(totalFreeItems, item.Quantity);

            if (discountedItems <= 0)
                return null;

            var discountAmount = discountedItems * item.Product.Price;

            var description = $"{FreeProductName}: Free {FreeProductName} for buying {ProductName} " +
                            $"(Buy {RequiredQuantity} {ProductName}, get {FreeProductQuantity} {FreeProductName} free - {discountedItems} free items)";

            return new AppliedOffer(description, discountAmount);
        }
    }
}