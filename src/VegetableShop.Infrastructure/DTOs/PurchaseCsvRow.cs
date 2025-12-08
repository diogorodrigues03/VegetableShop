namespace VegetableShop.Infrastructure.DTOs;

/// <summary>
/// Represents a purchase item row from a CSV file.
/// </summary>
public class PurchaseCsvRow
{
    public string Product { get; set; } = string.Empty;
    public string Quantity { get; set; } = string.Empty;
}