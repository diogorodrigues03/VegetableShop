using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Exceptions;
using VegetableShop.Infrastructure.DTOs;

namespace VegetableShop.Infrastructure.Parsers;

/// <summary>
/// Parses product data from CSV files.
/// </summary>
public class CsvProductParser
{
    public List<Product> ParseProducts(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Product file not found: {filePath}");
        }

        var products = new List<Product>();
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim
        };

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);

        var records = csv.GetRecords<ProductCsvRow>();

        foreach (var record in records)
        {
            if (string.IsNullOrWhiteSpace(record.Product)) continue;

            if (!decimal.TryParse(record.Price, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
            {
                throw new InvalidPriceException(record.Product, record.Price);
            }
            
            products.Add(new Product(record.Product, price));
        }

        return products;
    }
}