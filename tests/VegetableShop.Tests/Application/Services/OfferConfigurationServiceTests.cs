using FluentAssertions;
using VegetableShop.Application.Services;
using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Offers;

namespace VegetableShop.Tests.Application.Services
{
    public class OfferConfigurationServiceTests
    {
        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new OfferConfigurationService(null!));
        }

        [Fact]
        public void GetOffers_ShouldReturnExpectedOffers()
        {
            var factory = new OfferFactory();
            var service = new OfferConfigurationService(factory);
            var cart = new ShoppingCart();
            var products = new List<Product>
            {
                new ("Aubergine", 1m),
                new ("Tomato", 0.5m)
            };

            var offers = service.GetOffers(cart, products);

            offers.Should().HaveCount(3);
            offers.Should().ContainItemsAssignableTo<BuyXPayForYOffer>();
            offers.Should().ContainItemsAssignableTo<BuyXGetProductYFreeOffer>();
            offers.Should().ContainItemsAssignableTo<SpendThresholdDiscountOffer>();
        }
        
        [Fact]
        public void GetOffers_ShouldThrowArgumentNullException_WhenCartIsNull()
        {
            var factory = new OfferFactory();
            var service = new OfferConfigurationService(factory);
            var products = new List<Product>();
            ShoppingCart? cart = null;
            
            Assert.Throws<ArgumentNullException>(() => service.GetOffers(cart!, products));
        }
        
        [Fact]
        public void GetOffers_ShouldThrowArgumentNullException_WhenProductsIsNull()
        {
            var factory = new OfferFactory();
            var service = new OfferConfigurationService(factory);
            List<Product>? products = null;
            var cart = new ShoppingCart();
            
            Assert.Throws<ArgumentNullException>(() => service.GetOffers(cart, products!));
        }
    }
}