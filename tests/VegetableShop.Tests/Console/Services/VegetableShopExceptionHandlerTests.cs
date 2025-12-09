using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using VegetableShop.Console.Services;
using VegetableShop.Domain.Exceptions;

namespace VegetableShop.Tests.Console.Services
{
    public class VegetableShopExceptionHandlerTests
    {
        private readonly Mock<ILogger<VegetableShopExceptionHandler>> _loggerMock;
        private readonly VegetableShopExceptionHandler _handler;

        public VegetableShopExceptionHandlerTests()
        {
            _loggerMock = new Mock<ILogger<VegetableShopExceptionHandler>>();
            _handler = new VegetableShopExceptionHandler(_loggerMock.Object);
        }

        [Fact]
        public void Handle_ShouldReturnInvalidInputData_WhenInvalidPurchaseDataExceptionIsThrown()
        {
            var ex = new InvalidPurchaseDataException("Invalid purchase");
            var result = _handler.Handle(ex);
            result.Should().Be(ExitCodes.InvalidInputData);
        }

        [Fact]
        public void Handle_ShouldReturnInvalidInputData_WhenInvalidProductDataExceptionIsThrown()
        {
            var ex = new InvalidProductDataException("Invalid product");
            var result = _handler.Handle(ex);
            result.Should().Be(ExitCodes.InvalidInputData);
        }

        [Fact]
        public void Handle_ShouldReturnProductNotFound_WhenProductNotFoundExceptionIsThrown()
        {
            var ex = new ProductNotFoundException("Tomato");
            var result = _handler.Handle(ex);
            result.Should().Be(ExitCodes.ProductNotFound);
        }

        [Fact]
        public void Handle_ShouldReturnInvalidPrice_WhenInvalidPriceExceptionIsThrown()
        {
            var ex = new InvalidPriceException("Tomato", "abc");
            var result = _handler.Handle(ex);
            result.Should().Be(ExitCodes.InvalidPrice);
        }

        [Fact]
        public void Handle_ShouldReturnInvalidQuantity_WhenInvalidQuantityExceptionIsThrown()
        {
            var ex = new InvalidQuantityException("Tomato", "abc");
            var result = _handler.Handle(ex);
            result.Should().Be(ExitCodes.InvalidQuantity);
        }

        [Fact]
        public void Handle_ShouldReturnFileNotFound_WhenFileNotFoundExceptionIsThrown()
        {
            var ex = new FileNotFoundException("File not found");
            var result = _handler.Handle(ex);
            result.Should().Be(ExitCodes.FileNotFound);
        }

        [Fact]
        public void Handle_ShouldReturnUnexpectedError_WhenGenericExceptionIsThrown()
        {
            var ex = new Exception("Something went wrong");
            var result = _handler.Handle(ex);
            result.Should().Be(ExitCodes.UnexpectedError);
        }
    }
}
