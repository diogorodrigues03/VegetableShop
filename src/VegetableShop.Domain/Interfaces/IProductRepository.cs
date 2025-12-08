using VegetableShop.Domain.Entities;

namespace VegetableShop.Domain.Interfaces;

/// <summary>
/// Defines a repository for accessing product data.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Gets all products.
    /// </summary>
    /// <returns>A list of all products.</returns>
    Task<IEnumerable<Product>> GetAllProductsAsync();
    
    /// <summary>
    /// Gets a product by name.
    /// </summary>
    /// <param name="productName">The name of the product.</param>
    /// <returns>The product if found, otherwise null.</returns>
    Task<Product?> GetProductByNameAsync(string productName);
}