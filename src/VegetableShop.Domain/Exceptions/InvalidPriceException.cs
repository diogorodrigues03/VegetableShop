namespace VegetableShop.Domain.Exceptions;

/// <summary>
/// Exception thrown when an invalid price is found.
/// </summary>
public class InvalidPriceException : Exception
{
    public string ProductName { get; }
    public string PriceValue { get; }
    
    public InvalidPriceException(string productName, string priceValue)
        : base($"Invalid price '{priceValue}' for product '{productName}'.")
    {
        ProductName = productName;
        PriceValue = priceValue;
    }

    public InvalidPriceException(string productName, string priceValue, Exception innerException)
        : base($"Invalid price '{priceValue}' for product '{productName}'.", innerException)
    {
        ProductName = productName;
        PriceValue = priceValue;
    }
}