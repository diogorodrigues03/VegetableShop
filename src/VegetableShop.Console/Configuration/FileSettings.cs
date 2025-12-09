namespace VegetableShop.Console.Configuration
{
    /// <summary>
    /// Configuration settings for file paths.
    /// </summary>
    public class FileSettings
    {
        public string ProductsFile { get; init; } = string.Empty;
        public string PurchaseFile { get; init; } = string.Empty;
        public string ReceiptsDirectory { get; init; } = string.Empty;
        public string ReceiptOutputFile { get; init; } = string.Empty;
    }
}