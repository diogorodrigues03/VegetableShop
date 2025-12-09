using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Exceptions;

namespace VegetableShop.Tests.Domain.Entities
{
    public class ShoppingCartTests
    {
        [Fact]
        public void Constructor_InitializesEmptyCart()
        {
            var cart = new ShoppingCart();

            Assert.Empty(cart.Items);
        }

        [Fact]
        public void AddProduct_ValidProduct_AddsToCart()
        {
            var cart = new ShoppingCart();
            var product = new Product("Tomato", 0.75m);

            cart.AddProduct(product, 3);

            Assert.Single(cart.Items);
            Assert.Equal(3, cart.GetQuantity(product));
        }

        [Fact]
        public void AddProduct_NullProduct_ThrowsArgumentNullException()
        {
            var cart = new ShoppingCart();

            Assert.Throws<ArgumentNullException>(() => cart.AddProduct(null!, 3));
        }

        [Fact]
        public void AddProduct_ZeroQuantity_ThrowsInvalidQuantityException()
        {
            var cart = new ShoppingCart();
            var product = new Product("Tomato", 0.75m);

            Assert.Throws<InvalidQuantityException>(() => cart.AddProduct(product, 0));
        }

        [Fact]
        public void AddProduct_NegativeQuantity_ThrowsInvalidQuantityException()
        {
            var cart = new ShoppingCart();
            var product = new Product("Tomato", 0.75m);

            Assert.Throws<InvalidQuantityException>(() => cart.AddProduct(product, -1));
        }

        [Fact]
        public void AddProduct_SameProductTwice_CombinesQuantity()
        {
            var cart = new ShoppingCart();
            var product = new Product("Tomato", 0.75m);

            cart.AddProduct(product, 3);
            cart.AddProduct(product, 2);

            Assert.Single(cart.Items);
            Assert.Equal(5, cart.GetQuantity(product));
        }

        [Fact]
        public void AddProduct_DifferentProducts_AddsMultipleItems()
        {
            var cart = new ShoppingCart();
            var tomato = new Product("Tomato", 0.75m);
            var carrot = new Product("Carrot", 1.0m);

            cart.AddProduct(tomato, 3);
            cart.AddProduct(carrot, 2);

            Assert.Equal(2, cart.Items.Count());
            Assert.Equal(3, cart.GetQuantity(tomato));
            Assert.Equal(2, cart.GetQuantity(carrot));
        }

        [Fact]
        public void GetQuantity_ProductNotInCart_ReturnsZero()
        {
            var cart = new ShoppingCart();
            var product = new Product("Tomato", 0.75m);

            var quantity = cart.GetQuantity(product);

            Assert.Equal(0, quantity);
        }

        [Fact]
        public void Clear_RemovesAllItems()
        {
            var cart = new ShoppingCart();
            var product = new Product("Tomato", 0.75m);
            cart.AddProduct(product, 3);

            cart.Clear();

            Assert.Empty(cart.Items);
        }

        [Fact]
        public void Items_ReturnsLineItems()
        {
            var cart = new ShoppingCart();
            var tomato = new Product("Tomato", 0.75m);
            var carrot = new Product("Carrot", 1.0m);
            cart.AddProduct(tomato, 3);
            cart.AddProduct(carrot, 2);

            var items = cart.Items.ToList();

            Assert.Equal(2, items.Count);
            Assert.All(items, item => Assert.IsType<CartItem>(item));
        }
    }
}