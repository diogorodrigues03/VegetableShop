namespace VegetableShop.Domain.Entities;

/// <summary>
/// Represents an offer applied to the purchase.
/// </summary>
public class AppliedOffer
{
    public string Description { get; private set; }
    public decimal DiscountAmount { get; private set; }
    
    public AppliedOffer(string description, decimal discountAmount)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Offer description cannot be null or empty.", nameof(description));
        }

        if (discountAmount < 0)
        {
            throw new ArgumentException("Offer discount amount cannot be negative.", nameof(discountAmount));
        }
        
        Description = description;
        DiscountAmount = discountAmount;
    }

public override string ToString() => $"{Description} - €{DiscountAmount.ToString("F2", System.Globalization.CultureInfo.CurrentCulture)} discount";
}