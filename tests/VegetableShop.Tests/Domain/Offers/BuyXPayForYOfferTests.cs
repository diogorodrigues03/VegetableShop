using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Offers;
using Xunit;

namespace VegetableShop.Tests.Domain.Offers
{
    public class BuyXPayForYOfferTests
    {
        [Fact]
        public void Constructor_ValidInputs_CreatesOffer()
        {
            var offer = new BuyXPayForYOffer("Aubergine", 3, 2);

            Assert.Equal("Aubergine", offer.ProductName);
            Assert.Equal(3, offer.RequiredQuantity);
            Assert.Equal(2, offer.PayForQuantity);
        }

        [Fact]
        public void Constructor_EmptyProductName_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new BuyXPayForYOffer("", 3, 2));
        }

        [Fact]
        public void Constructor_PayForQuantityGreaterThanRequired_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new BuyXPayForYOffer("Aubergine", 3, 4));
        }

        [Fact]
        public void Calculate_ExactQuantity_AppliesDiscount()
        {
            var product = new Product("Aubergine", 0.9m);
            var cartItem = new CartItem(product, 3);
            var offer = new BuyXPayForYOffer("Aubergine", 3, 2);

            var result = offer.CalculateDiscount(cartItem);

            Assert.NotNull(result);
            Assert.Equal(0.9m, result.DiscountAmount);
        }

        [Fact]
        public void Calculate_DoubleQuantity_AppliesTwice()
        {
            var product = new Product("Aubergine", 0.9m);
            var cartItem = new CartItem(product, 6);
            var offer = new BuyXPayForYOffer("Aubergine", 3, 2);

            var result = offer.CalculateDiscount(cartItem);

            Assert.NotNull(result);
            Assert.Equal(1.8m, result.DiscountAmount);
        }

        [Fact]
        public void Calculate_QuantityBelowThreshold_ReturnsNull()
        {
            var product = new Product("Aubergine", 0.9m);
            var cartItem = new CartItem(product, 2);
            var offer = new BuyXPayForYOffer("Aubergine", 3, 2);

            var result = offer.CalculateDiscount(cartItem);

            Assert.Null(result);
        }

        [Fact]
        public void Calculate_DifferentProduct_ReturnsNull()
        {
            var product = new Product("Tomato", 0.75m);
            var cartItem = new CartItem(product, 3);
            var offer = new BuyXPayForYOffer("Aubergine", 3, 2);

            var result = offer.CalculateDiscount(cartItem);

            Assert.Null(result);
        }

        [Fact]
        public void Calculate_PartialQuantity_AppliesPartially()
        {
            var product = new Product("Aubergine", 0.9m);
            var cartItem = new CartItem(product, 5);
            var offer = new BuyXPayForYOffer("Aubergine", 3, 2);

            var result = offer.CalculateDiscount(cartItem);

            Assert.NotNull(result);
            Assert.Equal(0.9m, result.DiscountAmount);
        }

        [Theory]
        [InlineData(3, 0.9)]
        [InlineData(6, 1.8)]
        [InlineData(9, 2.7)]
        [InlineData(25, 7.2)]
        public void Calculate_VariousQuantities_CalculatesCorrectly(int quantity, decimal expectedDiscount)
        {
            var product = new Product("Aubergine", 0.9m);
            var cartItem = new CartItem(product, quantity);
            var offer = new BuyXPayForYOffer("Aubergine", 3, 2);

            var result = offer.CalculateDiscount(cartItem);

            Assert.NotNull(result);
            Assert.Equal(expectedDiscount, result.DiscountAmount);
        }
    }
}