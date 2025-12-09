using Microsoft.Extensions.Logging;
using VegetableShop.Console.Interfaces;
using VegetableShop.Domain.Exceptions;

namespace VegetableShop.Console.Services
{
    public class VegetableShopExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<VegetableShopExceptionHandler> _logger;
        private readonly Dictionary<Type, Func<Exception, ExitCodes>> _exceptionHandlers;

        public VegetableShopExceptionHandler(ILogger<VegetableShopExceptionHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            _exceptionHandlers = new Dictionary<Type, Func<Exception, ExitCodes>>
            {
                { typeof(InvalidPurchaseDataException), HandleInvalidPurchaseData },
                { typeof(InvalidProductDataException), HandleInvalidProductData },
                { typeof(ProductNotFoundException), HandleProductNotFound },
                { typeof(InvalidPriceException), HandleInvalidPrice },
                { typeof(InvalidQuantityException), HandleInvalidQuantity },
                { typeof(FileNotFoundException), HandleFileNotFound }
            };
        }

        public ExitCodes Handle(Exception ex)
        {
            var type = ex.GetType();

            // In these contained scenarios we are mostly looking at exact matches.
            // For now, exact match + fallback is enough given the known exception hierarchy.
            return _exceptionHandlers.TryGetValue(type, out var handler) ? handler(ex) : HandleUnexpectedError(ex);
        }

        private ExitCodes HandleInvalidPurchaseData(Exception ex)
        {
            LogAndPrintError(ex, ex.Message, "Please check your purchase.csv file.");
            return ExitCodes.InvalidInputData;
        }

        private ExitCodes HandleInvalidProductData(Exception ex)
        {
            LogAndPrintError(ex, ex.Message, "Please check your products.csv file.");
            return ExitCodes.InvalidInputData;
        }

        private ExitCodes HandleProductNotFound(Exception ex)
        {
            var notFoundEx = (ProductNotFoundException)ex;
            _logger.LogError(ex, "Product not found: {ProductName}", notFoundEx.ProductName);
            
            System.Console.WriteLine();
            System.Console.WriteLine($"ERROR: Product '{notFoundEx.ProductName}' was not found in the catalog.");
            System.Console.WriteLine("Please check your products.csv file.");
            
            return ExitCodes.ProductNotFound;
        }

        private ExitCodes HandleInvalidPrice(Exception ex)
        {
            var invalidPriceEx = (InvalidPriceException)ex;
            _logger.LogError(ex, "Invalid price for product: {ProductName}", invalidPriceEx.ProductName);
            
            System.Console.WriteLine();
            System.Console.WriteLine($"ERROR: Invalid price '{invalidPriceEx.PriceValue}' for product '{invalidPriceEx.ProductName}'.");
            
            return ExitCodes.InvalidPrice;
        }

        private ExitCodes HandleInvalidQuantity(Exception ex)
        {
            var invalidQuantityEx = (InvalidQuantityException)ex;
            _logger.LogError(ex, "Invalid quantity for product: {ProductName}", invalidQuantityEx.ProductName);
            
            System.Console.WriteLine();
            System.Console.WriteLine($"ERROR: Invalid quantity '{invalidQuantityEx.QuantityValue}' for product '{invalidQuantityEx.ProductName}'.");
            
            return ExitCodes.InvalidQuantity;
        }

        private ExitCodes HandleFileNotFound(Exception ex)
        {
            _logger.LogError(ex, "File not found");
            
            System.Console.WriteLine();
            System.Console.WriteLine($"ERROR: File not found - {ex.Message}");
            
            return ExitCodes.FileNotFound;
        }

        private ExitCodes HandleUnexpectedError(Exception ex)
        {
            _logger.LogCritical(ex, "Unexpected error occurred");
            
            System.Console.WriteLine();
            System.Console.WriteLine($"UNEXPECTED ERROR: {ex.Message}");
            System.Console.WriteLine();
            System.Console.WriteLine("Stack trace:");
            System.Console.WriteLine(ex.StackTrace);
            
            return ExitCodes.UnexpectedError;
        }

        private void LogAndPrintError(Exception ex, string message, string? extraInfo = null)
        {
            _logger.LogError(ex, message);
            System.Console.WriteLine();
            System.Console.WriteLine($"ERROR: {message}");
            if (!string.IsNullOrEmpty(extraInfo))
            {
                System.Console.WriteLine(extraInfo);
            }
        }
    }
}
