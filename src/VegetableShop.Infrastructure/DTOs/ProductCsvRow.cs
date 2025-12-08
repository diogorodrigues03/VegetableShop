namespace VegetableShop.Infrastructure.DTOs;

/// <summary>
/// Represents a product row from a CSV file.
/// </summary>
public class ProductCsvRow
{
    public string Product { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
}