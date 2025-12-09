using FluentAssertions;
using Moq;
using VegetableShop.Application.Services;
using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Exceptions;
using VegetableShop.Domain.Interfaces;
using VegetableShop.Application.Interfaces;

namespace VegetableShop.Tests.Application.Services
{
    public class CheckoutServiceTests
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<IPricingService> _pricingServiceMock;
        private readonly Mock<IOfferConfigurationService> _offerConfigMock;
        private readonly CheckoutService _service;

        public CheckoutServiceTests()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _pricingServiceMock = new Mock<IPricingService>();
            _offerConfigMock = new Mock<IOfferConfigurationService>();

            _service = new CheckoutService(
                _productRepoMock.Object,
                _pricingServiceMock.Object,
                _offerConfigMock.Object);
        }

        [Fact]
        public async Task ProcessPurchaseAsync_ShouldThrow_WhenPurchaseListIsEmpty()
        {
            var purchases = new Dictionary<string, int>();

            Func<Task> act = async () => await _service.ProcessPurchaseAsync(purchases);

            await act.Should().ThrowAsync<InvalidPurchaseDataException>()
                .WithMessage("*Purchase must contain at least one item*");
        }

        [Fact]
        public async Task ProcessPurchaseAsync_ShouldThrow_WhenProductRepositoryIsEmpty()
        {
            var purchases = new Dictionary<string, int> { { "Carrot", 1 } };
            _productRepoMock.Setup(r => r.GetAllProductsAsync())
                .ReturnsAsync(new List<Product>());

            Func<Task> act = async () => await _service.ProcessPurchaseAsync(purchases);

            await act.Should().ThrowAsync<InvalidProductDataException>()
                .WithMessage("*No products found*");
        }

        [Fact]
        public async Task ProcessPurchaseAsync_ShouldThrow_WhenProductNotFound()
        {
            var purchases = new Dictionary<string, int> { { "Onion", 1 } };
            var products = new List<Product> { new Product("Carrot", 1.0m) };
            
            _productRepoMock.Setup(r => r.GetAllProductsAsync())
                .ReturnsAsync(products);

            Func<Task> act = async () => await _service.ProcessPurchaseAsync(purchases);

            await act.Should().ThrowAsync<ProductNotFoundException>()
                .Where(e => e.ProductName == "Onion");
        }

        [Fact]
        public async Task ProcessPurchaseAsync_ShouldOrchestrateCheckoutFlowCorrectly()
        {
            var purchases = new Dictionary<string, int> { { "Carrot", 2 } };
            var carrot = new Product("Carrot", 1.0m);
            var products = new List<Product> { carrot };
            var receipt = new Receipt();
            var offers = new List<IPromotionalOffer>();

            _productRepoMock.Setup(r => r.GetAllProductsAsync())
                .ReturnsAsync(products);
            
            _offerConfigMock.Setup(o => o.GetOffers(It.IsAny<ShoppingCart>(), products))
                .Returns(offers);
            
            _pricingServiceMock.Setup(p => p.CalculateReceipt(It.IsAny<ShoppingCart>(), offers))
                .Returns(receipt);

            var result = await _service.ProcessPurchaseAsync(purchases);

            result.Should().BeSameAs(receipt);
            
            _productRepoMock.Verify(r => r.GetAllProductsAsync(), Times.Once);
            _offerConfigMock.Verify(o => o.GetOffers(
                It.Is<ShoppingCart>(c => c.Items.Count() == 1 && c.Items.First().Product.Name == "Carrot"), 
                products), Times.Once);
            _pricingServiceMock.Verify(p => p.CalculateReceipt(It.IsAny<ShoppingCart>(), offers), Times.Once);
        }
    }
}