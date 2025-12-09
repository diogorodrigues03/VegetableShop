namespace VegetableShop.Domain.Exceptions;

public class InvalidPurchaseDataException : Exception
{
    public InvalidPurchaseDataException() 
        : base("Invalid purchase data. Please provide data according to the format.")
    {
    }
    
    public InvalidPurchaseDataException(string message)
        : base(message)
    {
    }

    public InvalidPurchaseDataException(Exception innerException)
        : base("Invalid purchase data. Please provide data according to the format.", innerException)
    {
    }
    
    public InvalidPurchaseDataException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}