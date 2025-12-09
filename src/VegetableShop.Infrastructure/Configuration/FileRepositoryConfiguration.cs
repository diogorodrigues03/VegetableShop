namespace VegetableShop.Infrastructure.Configuration;

/// <summary>
/// Runtime configuration for file-based repositories.
/// This allows the application to update file paths at runtime (e.g., from CLI arguments).
/// </summary>
public class FileRepositoryConfiguration
{
    public string ProductsFilePath { get; set; } = string.Empty;
    public string PurchaseFilePath { get; set; } = string.Empty;
}
