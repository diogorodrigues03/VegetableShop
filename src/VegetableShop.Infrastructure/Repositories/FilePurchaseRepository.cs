using VegetableShop.Domain.Interfaces;
using VegetableShop.Infrastructure.Configuration;
using VegetableShop.Infrastructure.Parsers;

namespace VegetableShop.Infrastructure.Repositories;

/// <summary>
/// Repository for loading purchase data from a CSV file.
/// </summary>
public class FilePurchaseRepository(FileRepositoryConfiguration configuration) : IPurchaseRepository
{
    private readonly FileRepositoryConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

    public async Task<Dictionary<string, int>> GetPurchaseItemsAsync()
    {
        var path = _configuration.PurchaseFilePath;
        return await Task.Run(() =>
        {
            var parser = new CsvPurchaseParser();
            return parser.ParsePurchases(path);
        });
    }
}