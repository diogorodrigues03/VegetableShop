using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Exceptions;
using VegetableShop.Domain.Offers;
using Xunit;

namespace VegetableShop.Tests.Domain.Offers
{
    public class BuyXGetProductYFreeOfferTests
    {
        [Fact]
        public void Constructor_ValidInputs_CreatesOffer()
        {
            var cart = new ShoppingCart();
            var products = new List<Product> { new Product("Tomato", 0.75m) };

            var offer = new BuyXGetProductYFreeOffer("Tomato", 2, "Aubergine", 1, cart, products);

            Assert.Equal("Tomato", offer.ProductName);
            Assert.Equal(2, offer.RequiredQuantity);
            Assert.Equal("Aubergine", offer.FreeProductName);
            Assert.Equal(1, offer.FreeProductQuantity);
        }

        [Fact]
        public void Constructor_EmptyProductName_ThrowsArgumentException()
        {
            var cart = new ShoppingCart();
            var products = new List<Product>();

            Assert.Throws<ArgumentException>(() => 
                new BuyXGetProductYFreeOffer("", 2, "Aubergine", 1, cart, products));
        }

        [Fact]
        public void Constructor_EmptyFreeProductName_ThrowsArgumentException()
        {
            var cart = new ShoppingCart();
            var products = new List<Product>();

            Assert.Throws<ArgumentException>(() => 
                new BuyXGetProductYFreeOffer("Tomato", 2, "", 1, cart, products));
        }

        [Fact]
        public void Constructor_NullCart_ThrowsArgumentNullException()
        {
            var products = new List<Product>();

            Assert.Throws<ArgumentNullException>(() => 
                new BuyXGetProductYFreeOffer("Tomato", 2, "Aubergine", 1, null!, products));
        }

        [Fact]
        public void Constructor_NullProducts_ThrowsArgumentNullException()
        {
            var cart = new ShoppingCart();

            Assert.Throws<ArgumentNullException>(() => 
                new BuyXGetProductYFreeOffer("Tomato", 2, "Aubergine", 1, cart, null!));
        }

        [Fact]
        public void CalculateDiscount_WithTriggerProduct_AppliesDiscount()
        {
            var tomato = new Product("Tomato", 0.75m);
            var aubergine = new Product("Aubergine", 0.9m);
            var products = new List<Product> { tomato, aubergine };

            var cart = new ShoppingCart();
            cart.AddProduct(tomato, 2);
            cart.AddProduct(aubergine, 1);

            var offer = new BuyXGetProductYFreeOffer("Tomato", 2, "Aubergine", 1, cart, products);
            var aubergineCartItem = new CartItem(aubergine, 1);

            var result = offer.CalculateDiscount(aubergineCartItem);

            Assert.NotNull(result);
            Assert.Equal(0.9m, result.DiscountAmount);
        }

        [Fact]
        public void CalculateDiscount_WithoutTriggerProduct_ReturnsNull()
        {
            var tomato = new Product("Tomato", 0.75m);
            var aubergine = new Product("Aubergine", 0.9m);
            var products = new List<Product> { tomato, aubergine };

            var cart = new ShoppingCart();
            cart.AddProduct(aubergine, 1);

            var offer = new BuyXGetProductYFreeOffer("Tomato", 2, "Aubergine", 1, cart, products);
            var aubergineCartItem = new CartItem(aubergine, 1);

            var result = offer.CalculateDiscount(aubergineCartItem);

            Assert.Null(result);
        }

        [Fact]
        public void CalculateDiscount_BelowTriggerThreshold_ReturnsNull()
        {
            var tomato = new Product("Tomato", 0.75m);
            var aubergine = new Product("Aubergine", 0.9m);
            var products = new List<Product> { tomato, aubergine };

            var cart = new ShoppingCart();
            cart.AddProduct(tomato, 1);
            cart.AddProduct(aubergine, 1);

            var offer = new BuyXGetProductYFreeOffer("Tomato", 2, "Aubergine", 1, cart, products);
            var aubergineCartItem = new CartItem(aubergine, 1);

            var result = offer.CalculateDiscount(aubergineCartItem);

            Assert.Null(result);
        }

        [Fact]
        public void CalculateDiscount_MultipleTriggers_AppliesMultipleTimes()
        {
            var tomato = new Product("Tomato", 0.75m);
            var aubergine = new Product("Aubergine", 0.9m);
            var products = new List<Product> { tomato, aubergine };

            var cart = new ShoppingCart();
            cart.AddProduct(tomato, 4);
            cart.AddProduct(aubergine, 3);

            var offer = new BuyXGetProductYFreeOffer("Tomato", 2, "Aubergine", 1, cart, products);
            var aubergineCartItem = new CartItem(aubergine, 3);

            var result = offer.CalculateDiscount(aubergineCartItem);

            Assert.NotNull(result);
            Assert.Equal(1.8m, result.DiscountAmount);
        }

        [Fact]
        public void CalculateDiscount_LimitedByPurchasedQuantity_AppliesPartially()
        {
            var tomato = new Product("Tomato", 0.75m);
            var aubergine = new Product("Aubergine", 0.9m);
            var products = new List<Product> { tomato, aubergine };

            var cart = new ShoppingCart();
            cart.AddProduct(tomato, 10);
            cart.AddProduct(aubergine, 2);

            var offer = new BuyXGetProductYFreeOffer("Tomato", 2, "Aubergine", 1, cart, products);
            var aubergineCartItem = new CartItem(aubergine, 2);

            var result = offer.CalculateDiscount(aubergineCartItem);

            Assert.NotNull(result);
            Assert.Equal(1.8m, result.DiscountAmount);
        }

        [Fact]
        public void CalculateDiscount_WrongProduct_ReturnsNull()
        {
            var tomato = new Product("Tomato", 0.75m);
            var aubergine = new Product("Aubergine", 0.9m);
            var carrot = new Product("Carrot", 1m);
            var products = new List<Product> { tomato, aubergine, carrot };

            var cart = new ShoppingCart();
            cart.AddProduct(tomato, 2);

            var offer = new BuyXGetProductYFreeOffer("Tomato", 2, "Aubergine", 1, cart, products);
            var carrotCartItem = new CartItem(carrot, 1);

            var result = offer.CalculateDiscount(carrotCartItem);

            Assert.Null(result);
        }

        [Fact]
        public void CalculateDiscount_NullCartItem_ThrowsArgumentNullException()
        {
            var cart = new ShoppingCart();
            var products = new List<Product> { new Product("Tomato", 0.75m) };
            var offer = new BuyXGetProductYFreeOffer("Tomato", 2, "Aubergine", 1, cart, products);

            Assert.Throws<ArgumentNullException>(() => offer.CalculateDiscount(null!));
        }

        [Fact]
        public void CalculateDiscount_ExerciseScenario_Buy2TomatoesGet1AubergineFree()
        {
            var tomato = new Product("Tomato", 0.75m);
            var aubergine = new Product("Aubergine", 0.9m);
            var products = new List<Product> { tomato, aubergine };

            var cart = new ShoppingCart();
            cart.AddProduct(tomato, 3);
            cart.AddProduct(aubergine, 25);

            var offer = new BuyXGetProductYFreeOffer("Tomato", 2, "Aubergine", 1, cart, products);
            var aubergineCartItem = new CartItem(aubergine, 25);

            var result = offer.CalculateDiscount(aubergineCartItem);

            Assert.NotNull(result);
            Assert.Equal(0.9m, result.DiscountAmount);
        }

        [Fact]
        public void CalculateDiscount_CaseInsensitiveProductMatch_AppliesDiscount()
        {
            var tomato = new Product("Tomato", 0.75m);
            var aubergine = new Product("Aubergine", 0.9m);
            var products = new List<Product> { tomato, aubergine };

            var cart = new ShoppingCart();
            cart.AddProduct(tomato, 2);
            cart.AddProduct(aubergine, 1);

            var offer = new BuyXGetProductYFreeOffer("tomato", 2, "aubergine", 1, cart, products);
            var aubergineCartItem = new CartItem(aubergine, 1);

            var result = offer.CalculateDiscount(aubergineCartItem);

            Assert.NotNull(result);
            Assert.Equal(0.9m, result.DiscountAmount);
        }

        [Fact]
        public void CalculateDiscount_ZeroFreeProducts_ReturnsInvalidQuantityException()
        {
            var tomato = new Product("Tomato", 0.75m);
            var aubergine = new Product("Aubergine", 0.9m);
            var products = new List<Product> { tomato, aubergine };

            var cart = new ShoppingCart();
            cart.AddProduct(tomato, 10);
            cart.AddProduct(aubergine, 1);
            
            Assert.Throws<InvalidQuantityException>(() 
                => new BuyXGetProductYFreeOffer("Tomato", 0, "Aubergine", 1, cart, products));

        }
    }
}