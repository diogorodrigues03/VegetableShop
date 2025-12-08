namespace VegetableShop.Domain.Interfaces;

/// <summary>
/// Defines a repository for accessing purchase data.
/// </summary>
public interface IPurchaseRepository
{
    /// <summary>
    /// Gets purchase items from a file.
    /// </summary>
    /// <returns>A dictionary of product names to quantities.</returns>
    Task<Dictionary<string, int>> GetPurchaseItemsAsync();
}