namespace VegetableShop.Console.Configuration
{
    /// <summary>
    /// Configuration settings for file paths.
    /// </summary>
    public class FileSettings
    {
        public string ProductsFile { get; set; } = string.Empty;
        public string PurchaseFile { get; set; } = string.Empty;
        public string ReceiptsDirectory { get; set; } = string.Empty;
        public string ReceiptOutputFile { get; set; } = string.Empty;
    }
}