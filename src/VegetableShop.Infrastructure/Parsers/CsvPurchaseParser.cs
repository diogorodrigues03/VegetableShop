using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using VegetableShop.Infrastructure.DTOs;

namespace VegetableShop.Infrastructure.Parsers;

/// <summary>
/// Parses purchase data from a CSV file.
/// </summary>
public class CsvPurchaseParser
{
    public Dictionary<string, int> ParsePurchases(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Purchase file not found: {filePath}");
        }

        var purchases = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            TrimOptions = TrimOptions.Trim
        };

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);

        var records = csv.GetRecords<PurchaseCsvRow>();

        foreach (var record in records)
        {
            if(string.IsNullOrWhiteSpace(record.Product)) continue;

            if (!int.TryParse(record.Quantity, out var quantity) || quantity <= 0)
            {
                throw new ArgumentException($"Invalid quantity '{record.Quantity}' for product '{record.Product}'.");
            }

            if (!purchases.TryAdd(record.Product, quantity))
            {
                purchases[record.Product] += quantity;
            }
        }

        return purchases;
    }
}