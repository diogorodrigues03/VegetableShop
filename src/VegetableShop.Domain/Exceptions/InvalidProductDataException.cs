namespace VegetableShop.Domain.Exceptions;

public class InvalidProductDataException : Exception
{
    public InvalidProductDataException() 
        : base("Invalid products data. Please provide data according to the format.")
    {
    }
    
    public InvalidProductDataException(string message)
        : base(message)
    {
    }

    public InvalidProductDataException(Exception innerException)
        : base("Invalid product data. Please provide data according to the format.", innerException)
    {
    }
    
    public InvalidProductDataException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}