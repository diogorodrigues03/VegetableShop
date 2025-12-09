using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VegetableShop.Application.Interfaces;
using VegetableShop.Console.Configuration;
using VegetableShop.Domain.Exceptions;
using VegetableShop.Domain.Interfaces;
using VegetableShop.Domain.Utils;
using VegetableShop.Application.Constants;
using VegetableShop.Infrastructure.Configuration;

namespace VegetableShop.Console.Services
{
    /// <summary>
    /// Main application service that orchestrates the checkout process.
    /// </summary>
    public class VegetableShopApplication(
        ICheckoutService checkoutService,
        IPurchaseRepository purchaseRepository,
        IReceiptFormatter receiptFormatter,
        ILogger<VegetableShopApplication> logger,
        IOptions<FileSettings> fileSettings,
        FileRepositoryConfiguration fileRepositoryConfiguration)
        : IVegetableShopApplication
    {
        private readonly ICheckoutService _checkoutService = checkoutService ?? throw new ArgumentNullException(nameof(checkoutService));
        private readonly IPurchaseRepository _purchaseRepository = purchaseRepository ?? throw new ArgumentNullException(nameof(purchaseRepository));
        private readonly IReceiptFormatter _receiptFormatter = receiptFormatter ?? throw new ArgumentNullException(nameof(receiptFormatter));
        private readonly ILogger<VegetableShopApplication> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly FileSettings _fileSettings = fileSettings?.Value ?? throw new ArgumentNullException(nameof(fileSettings));
        private readonly FileRepositoryConfiguration _fileRepositoryConfiguration = fileRepositoryConfiguration ?? throw new ArgumentNullException(nameof(fileRepositoryConfiguration));

        public async Task<ExitCodes> RunAsync(string[] args)
        {
            try
            {
                DisplayHeader();

                string productsFile = args.Length > 0 ? args[0] : _fileSettings.ProductsFile;
                string purchaseFile = args.Length > 1 ? args[1] : _fileSettings.PurchaseFile;
                bool saveToFile = args.Contains("--save");
                string outputFile = GenerateTimestampedOutputFileName();

                // Update runtime configuration with potentially overridden paths
                _fileRepositoryConfiguration.ProductsFilePath = productsFile;
                _fileRepositoryConfiguration.PurchaseFilePath = purchaseFile;

                _logger.LogInformation("Products file: {ProductsFile}", productsFile);
                _logger.LogInformation("Purchase file: {PurchaseFile}", purchaseFile);

                if (!File.Exists(productsFile))
                {
                    _logger.LogError("Products file not found: {ProductsFile}", productsFile);
                    System.Console.WriteLine($"ERROR: Products file not found: {productsFile}");
                    return ExitCodes.FileNotFound;
                }

                if (!File.Exists(purchaseFile))
                {
                    _logger.LogError("Purchase file not found: {PurchaseFile}", purchaseFile);
                    System.Console.WriteLine($"ERROR: Purchase file not found: {purchaseFile}");
                    return ExitCodes.FileNotFound;
                }

                System.Console.WriteLine();
                _logger.LogInformation("Loading purchase data...");
                System.Console.WriteLine("Loading purchase data...");

                // Load purchase data
                var purchaseItems = await _purchaseRepository.GetPurchaseItemsAsync();

                _logger.LogInformation("Processing checkout for {ItemCount} unique items...", purchaseItems.Count);
                System.Console.WriteLine("Processing checkout...");

                // Process checkout
                var receipt = await _checkoutService.ProcessPurchaseAsync(purchaseItems);

                _logger.LogInformation("Checkout completed. Total: €{FormatCurrency}",
                    FormattingUtils.FormatCurrency(receipt.TotalPrice));

                // Format and display receipt
                System.Console.WriteLine();
                string formattedReceipt = _receiptFormatter.Format(receipt);
                System.Console.WriteLine(formattedReceipt);

                // Save to file if requested
                if (saveToFile)
                {
                    var outputDir = Path.GetDirectoryName(outputFile);
                    if (!string.IsNullOrEmpty(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    await File.WriteAllTextAsync(outputFile, formattedReceipt);
                    System.Console.WriteLine($"Receipt saved to: {outputFile}");
                    _logger.LogInformation("Receipt saved to: {OutputFile}", outputFile);
                }

                return ExitCodes.Success;
            }
            catch (InvalidPurchaseDataException ex)
            {
                _logger.LogError(ex, ex.Message);
                System.Console.WriteLine();
                System.Console.WriteLine($"ERROR: {ex.Message}");
                System.Console.WriteLine("Please check your purchase.csv file.");
                return ExitCodes.InvalidInputData;
            }
            catch (InvalidProductDataException ex)
            {
                _logger.LogError(ex, ex.Message);
                System.Console.WriteLine();
                System.Console.WriteLine($"ERROR: {ex.Message}");
                System.Console.WriteLine("Please check your products.csv file.");
                return ExitCodes.InvalidInputData;
            }
            catch (ProductNotFoundException ex)
            {
                _logger.LogError(ex, "Product not found: {ProductName}", ex.ProductName);
                System.Console.WriteLine();
                System.Console.WriteLine($"ERROR: Product '{ex.ProductName}' was not found in the catalog.");
                System.Console.WriteLine("Please check your products.csv file.");
                return ExitCodes.ProductNotFound;
            }
            catch (InvalidPriceException ex)
            {
                _logger.LogError(ex, "Invalid price for product: {ProductName}", ex.ProductName);
                System.Console.WriteLine();
                System.Console.WriteLine($"ERROR: Invalid price '{ex.PriceValue}' for product '{ex.ProductName}'.");
                return ExitCodes.InvalidPrice;
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "File not found");
                System.Console.WriteLine();
                System.Console.WriteLine($"ERROR: File not found - {ex.Message}");
                return ExitCodes.FileNotFound;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unexpected error occurred");
                System.Console.WriteLine();
                System.Console.WriteLine($"UNEXPECTED ERROR: {ex.Message}");
                System.Console.WriteLine();
                System.Console.WriteLine("Stack trace:");
                System.Console.WriteLine(ex.StackTrace);
                return ExitCodes.UnexpectedError;
            }
        }

        private static void DisplayHeader()
        {
            System.Console.WriteLine(FormattingConstants.Separator);
            System.Console.WriteLine(FormattingConstants.VegetableShopHeader);
            System.Console.WriteLine(FormattingConstants.Separator);
            System.Console.WriteLine();
        }

        private string GenerateTimestampedOutputFileName()
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var originalFileName = Path.GetFileNameWithoutExtension(_fileSettings.ReceiptOutputFile);
            var extension = Path.GetExtension(_fileSettings.ReceiptOutputFile);
            var directory = _fileSettings.ReceiptsDirectory;

            if (string.IsNullOrWhiteSpace(directory))
            {
                directory = Directory.GetCurrentDirectory();
            }

            return Path.Combine(directory, $"{originalFileName}_{timestamp}{extension}");
        }
    }

    /// <summary>
    /// Application exit codes.
    /// </summary>
    public enum ExitCodes
    {
        Success = 0,
        FileNotFound = 1,
        ProductNotFound = 2,
        InvalidPrice = 3,
        InvalidInputData = 4,
        UnexpectedError = 99,
    }
}