using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Interfaces;
using VegetableShop.Infrastructure.Parsers;

namespace VegetableShop.Infrastructure.Repositories;

/// <summary>
/// Repository for loading products from a CSV file.
/// </summary>
public class FileProductRepository(string filePath) : IProductRepository
{
    private readonly string _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    private List<Product>? _cachedProducts;

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        if (_cachedProducts is null)
        {
            await LoadProductsAsync();
        }

        return _cachedProducts!;
    }

    public async Task<Product?> GetProductByNameAsync(string productName)
    {
        var products = await GetAllProductsAsync();
        return products.FirstOrDefault(p => string.Equals(p.Name, productName, StringComparison.OrdinalIgnoreCase));
    }
    
    private Task LoadProductsAsync()
    {
        return Task.Run(() =>
        {
            var parser = new CsvProductParser();
            _cachedProducts = parser.ParseProducts(_filePath);
        });
    }
}