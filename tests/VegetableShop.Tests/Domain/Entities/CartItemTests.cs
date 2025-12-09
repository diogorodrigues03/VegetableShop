using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Exceptions;

namespace VegetableShop.Tests.Domain.Entities;

public class CartItemTests
{
    [Fact]
        public void Constructor_ValidInputs_CreatesCartItem()
        {
            var product = new Product("Tomato", 0.75m);
            var cartItem = new CartItem(product, 3);

            Assert.Equal(product, cartItem.Product);
            Assert.Equal(3, cartItem.Quantity);
            Assert.Equal(2.25m, cartItem.TotalPrice);
        }

        [Fact]
        public void Constructor_NullProduct_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CartItem(null!, 3));
        }

        [Fact]
        public void Constructor_ZeroQuantity_ThrowsInvalidQuantityException()
        {
            var product = new Product("Tomato", 0.75m);

            Assert.Throws<InvalidQuantityException>(() => new CartItem(product, 0));
        }

        [Fact]
        public void Constructor_NegativeQuantity_ThrowsInvalidQuantityException()
        {
            var product = new Product("Tomato", 0.75m);

            Assert.Throws<InvalidQuantityException>(() => new CartItem(product, -1));
        }

        [Theory]
        [InlineData(1, 0.75)]
        [InlineData(3, 2.25)]
        [InlineData(10, 7.50)]
        public void Total_CalculatesCorrectly(int quantity, decimal expectedTotal)
        {
            var product = new Product("Tomato", 0.75m);
            var cartItem = new CartItem(product, quantity);
            var total = cartItem.TotalPrice;

            Assert.Equal(expectedTotal, total);
        }

        [Fact]
        public void ToString_ReturnsFormattedString()
        {
            var product = new Product("Tomato", 0.75m);
            var cartItem = new CartItem(product, 3);
            var result = cartItem.ToString();

            Assert.Equal("Tomato x3 - €2,25", result);
        }
}