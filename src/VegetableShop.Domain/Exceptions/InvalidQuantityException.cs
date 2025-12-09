namespace VegetableShop.Domain.Exceptions;

/// <summary>
/// Exception thrown when an invalid quantity is found.
/// </summary>
public class InvalidQuantityException : Exception
{
    public string ProductName { get; }
    public string QuantityValue { get; }
    
    public InvalidQuantityException(string productName, string quantityValue)
        : base($"Invalid quantity '{quantityValue}' for product '{productName}'.")
    {
        ProductName = productName;
        QuantityValue = quantityValue;
    }

    public InvalidQuantityException(string productName, string quantityValue, Exception innerException)
        : base($"Invalid quantity '{quantityValue}' for product '{productName}'.", innerException)
    {
        ProductName = productName;
        QuantityValue = quantityValue;
    }
}