using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Offers;
using Xunit;

namespace VegetableShop.Tests.Domain.Offers
{
    public class SpendThresholdDiscountOfferTests
    {
        [Fact]
        public void Constructor_ValidInputs_CreatesOffer()
        {
            var offer = new SpendThresholdDiscountOffer("Tomato", 4m, 1m);

            Assert.Equal("Tomato", offer.ProductName);
            Assert.Equal(4m, offer.SpendThreshold);
            Assert.Equal(1m, offer.DiscountAmount);
        }

        [Fact]
        public void Constructor_DiscountGreaterThanThreshold_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => 
                new SpendThresholdDiscountOffer("Tomato", 4m, 5m));
        }

        [Fact]
        public void CalculateDiscount_ExactThreshold_AppliesDiscount()
        {
            var product = new Product("Tomato", 0.75m);
            var cartItem = new CartItem(product, 6);
            var offer = new SpendThresholdDiscountOffer("Tomato", 4m, 1m);

            var result = offer.CalculateDiscount(cartItem);

            Assert.NotNull(result);
            Assert.Equal(1m, result.DiscountAmount);
        }

        [Fact]
        public void CalculateDiscount_DoubleThreshold_AppliesTwice()
        {
            var product = new Product("Tomato", 0.75m);
            var cartItem = new CartItem(product, 12);
            var offer = new SpendThresholdDiscountOffer("Tomato", 4m, 1m);

            var result = offer.CalculateDiscount(cartItem);

            Assert.NotNull(result);
            Assert.Equal(2m, result.DiscountAmount);
        }

        [Fact]
        public void CalculateDiscount_BelowThreshold_ReturnsNull()
        {
            var product = new Product("Tomato", 0.75m);
            var cartItem = new CartItem(product, 3);
            var offer = new SpendThresholdDiscountOffer("Tomato", 4m, 1m);

            var result = offer.CalculateDiscount(cartItem);

            Assert.Null(result);
        }

        [Fact]
        public void CalculateDiscount_DifferentProduct_ReturnsNull()
        {
            var product = new Product("Carrot", 1m);
            var cartItem = new CartItem(product, 5);
            var offer = new SpendThresholdDiscountOffer("Tomato", 4m, 1m);

            var result = offer.CalculateDiscount(cartItem);

            Assert.Null(result);
        }

        [Theory]
        [InlineData(3, 0)]
        [InlineData(6, 1)]
        [InlineData(12, 2)]
        [InlineData(20, 3)]
        public void CalculateDiscount_VariousQuantities_CalculateDiscountsCorrectly(int quantity, decimal expectedDiscount)
        {
            var product = new Product("Tomato", 0.75m);
            var cartItem = new CartItem(product, quantity);
            var offer = new SpendThresholdDiscountOffer("Tomato", 4m, 1m);

            var result = offer.CalculateDiscount(cartItem);
            
            if (expectedDiscount == 0)
            {
                Assert.Null(result);
            }
            else
            {
                Assert.NotNull(result);
                Assert.Equal(expectedDiscount, result.DiscountAmount);
            }
        }
    }
}