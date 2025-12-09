using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VegetableShop.Application.Interfaces;
using VegetableShop.Console.Configuration;
using VegetableShop.Console.Interfaces;
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
        FileRepositoryConfiguration fileRepositoryConfiguration,
        IExceptionHandler exceptionHandler)
        : IVegetableShopApplication
    {
        private readonly ICheckoutService _checkoutService = checkoutService ?? throw new ArgumentNullException(nameof(checkoutService));
        private readonly IPurchaseRepository _purchaseRepository = purchaseRepository ?? throw new ArgumentNullException(nameof(purchaseRepository));
        private readonly IReceiptFormatter _receiptFormatter = receiptFormatter ?? throw new ArgumentNullException(nameof(receiptFormatter));
        private readonly ILogger<VegetableShopApplication> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly FileSettings _fileSettings = fileSettings.Value ?? throw new ArgumentNullException(nameof(fileSettings));
        private readonly FileRepositoryConfiguration _fileRepositoryConfiguration = fileRepositoryConfiguration ?? throw new ArgumentNullException(nameof(fileRepositoryConfiguration));
        private readonly IExceptionHandler _exceptionHandler = exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler));

        public async Task<ExitCodes> RunAsync(string[] args)
        {
            try
            {
                DisplayHeader();

                string productsFile;
                string purchaseFile;
                bool saveToFile = args.Contains("--save");
                if (args.Length == 1 && saveToFile)
                {
                    productsFile = _fileSettings.ProductsFile;
                    purchaseFile = _fileSettings.PurchaseFile;
                }
                else
                {
                    productsFile = args.Length > 0 ? args[0] : _fileSettings.ProductsFile;
                    purchaseFile = args.Length > 1 ? args[1] : _fileSettings.PurchaseFile;
                }

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

                await SaveToFile(saveToFile, outputFile, formattedReceipt);

                return ExitCodes.Success;
            }
            catch (Exception ex)
            {
                return _exceptionHandler.Handle(ex);
            }
        }

        private static void DisplayHeader()
        {
            System.Console.WriteLine(FormattingConstants.Separator);
            System.Console.WriteLine(FormattingConstants.VegetableShopHeader);
            System.Console.WriteLine(FormattingConstants.Separator);
            System.Console.WriteLine();
        }

        private async Task SaveToFile(bool saveToFile, string outputFile, string formattedReceipt)
        {
            try
            {
                // Save to the file if requested
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
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error saving receipt to file: {OutputFile}", outputFile);
                System.Console.WriteLine($"ERROR: Error saving receipt to file: {outputFile}");
            }
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
        InvalidQuantity = 5,
        UnexpectedError = 99,
    }
}