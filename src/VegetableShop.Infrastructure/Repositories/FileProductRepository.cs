using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Interfaces;
using VegetableShop.Infrastructure.Configuration;
using VegetableShop.Infrastructure.Parsers;

namespace VegetableShop.Infrastructure.Repositories;

/// <summary>
/// Repository for loading products from a CSV file.
/// </summary>
public class FileProductRepository(FileRepositoryConfiguration configuration) : IProductRepository
{
    private readonly FileRepositoryConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    private List<Product>? _cachedProducts;
    private string _lastLoadedPath = string.Empty;

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        var currentPath = _configuration.ProductsFilePath;
        if (_cachedProducts is null || !string.Equals(currentPath, _lastLoadedPath, StringComparison.OrdinalIgnoreCase))
        {
            await LoadProductsAsync(currentPath);
            _lastLoadedPath = currentPath;
        }

        return _cachedProducts!;
    }

    public async Task<Product?> GetProductByNameAsync(string productName)
    {
        var products = await GetAllProductsAsync();
        return products.FirstOrDefault(p => string.Equals(p.Name, productName, StringComparison.OrdinalIgnoreCase));
    }
    
    private Task LoadProductsAsync(string path)
    {
        return Task.Run(() =>
        {
            var parser = new CsvProductParser();
            _cachedProducts = parser.ParseProducts(path);
        });
    }
}