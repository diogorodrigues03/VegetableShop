using CsvHelper.Configuration.Attributes;

namespace VegetableShop.Infrastructure.DTOs;

/// <summary>
/// Represents a product row from a CSV file.
/// </summary>
public class ProductCsvRow
{
    [Name("PRODUCT")]
    public string Product { get; set; } = string.Empty;
    [Name("PRICE")]
    public string Price { get; set; } = string.Empty;
}