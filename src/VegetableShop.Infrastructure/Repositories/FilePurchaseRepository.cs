using VegetableShop.Domain.Interfaces;
using VegetableShop.Infrastructure.Parsers;

namespace VegetableShop.Infrastructure.Repositories;

/// <summary>
/// Repository for loading purchase data from a CSV file.
/// </summary>
public class FilePurchaseRepository(string filePath) : IPurchaseRepository
{
    private readonly string _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));

    public async Task<Dictionary<string, int>> GetPurchaseItemsAsync()
    {
        return await Task.Run(() =>
        {
            var parser = new CsvPurchaseParser();
            return parser.ParsePurchases(_filePath);
        });
    }
}