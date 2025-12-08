namespace VegetableShop.Domain.Exceptions;

/// <summary>
/// Exception thrown when a requested product is not found.
/// </summary>
public class ProductNotFoundException : Exception
{
    public string ProductName { get; }

    public ProductNotFoundException(string productName) : base($"Product {productName} was not found in the catalog.")
    {
        ProductName = productName;
    }

    public ProductNotFoundException(string productName, Exception innerException) : base(
        $"Product {productName} was not found in the catalog.", innerException)
    {
        ProductName = productName;
    }
}