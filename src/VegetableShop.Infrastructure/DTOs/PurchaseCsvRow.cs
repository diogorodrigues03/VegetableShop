using CsvHelper.Configuration.Attributes;

namespace VegetableShop.Infrastructure.DTOs;

/// <summary>
/// Represents a purchase item row from a CSV file.
/// </summary>
public class PurchaseCsvRow
{
    [Name("PRODUCT")]
    public string Product { get; set; } = string.Empty;
    [Name("QUANTITY")]
    public string Quantity { get; set; } = string.Empty;
}