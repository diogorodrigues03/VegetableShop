using FluentAssertions;
using Moq;
using VegetableShop.Application.Services;
using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Interfaces;

namespace VegetableShop.Tests.Application.Services
{
    public class PricingServiceTests
    {
        private readonly PricingService _service = new();

        [Fact]
        public void CalculateReceipt_ShouldCalculateTotalCorrectly_WithoutOffers()
        {
            var cart = new ShoppingCart();
            var p1 = new Product("Carrot", 10m);
            var p2 = new Product("Tomato", 5m);
            cart.AddProduct(p1, 2);
            cart.AddProduct(p2, 1);
            
            var offers = new List<IPromotionalOffer>();

            var receipt = _service.CalculateReceipt(cart, offers);

            receipt.TotalPrice.Should().Be(25m);
            receipt.AppliedOffers.Should().BeEmpty();
            receipt.SubTotal.Should().Be(25m);
        }

        [Fact]
        public void CalculateReceipt_ShouldApplyOffersAndReduceTotal()
        {
            var cart = new ShoppingCart();
            var p1 = new Product("A", 10m);
            cart.AddProduct(p1, 1);

            var offerMock = new Mock<IPromotionalOffer>();
            offerMock.Setup(o => o.CalculateDiscount(It.IsAny<CartItem>()))
                .Returns(new AppliedOffer("Test Offer", 2m));

            var offers = new List<IPromotionalOffer> { offerMock.Object };

            var receipt = _service.CalculateReceipt(cart, offers);

            receipt.SubTotal.Should().Be(10m);
            receipt.TotalPrice.Should().Be(8m);
            receipt.AppliedOffers.Should().HaveCount(1);
            receipt.AppliedOffers.First().DiscountAmount.Should().Be(2m);
        }
        
        [Fact]
        public void CalculateReceipt_ShouldNotApplyOffer_WhenDiscountIsZero()
        {
            var cart = new ShoppingCart();
            var p1 = new Product("A", 10m);
            cart.AddProduct(p1, 1); 

            var offerMock = new Mock<IPromotionalOffer>();
            offerMock.Setup(o => o.CalculateDiscount(It.IsAny<CartItem>()))
                .Returns((AppliedOffer?)null); 

            var offers = new List<IPromotionalOffer> { offerMock.Object };

            var receipt = _service.CalculateReceipt(cart, offers);

            receipt.TotalPrice.Should().Be(10m);
            receipt.AppliedOffers.Should().BeEmpty();
        }
    }
}