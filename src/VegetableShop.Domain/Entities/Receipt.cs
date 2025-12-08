namespace VegetableShop.Domain.Entities;

/// <summary>
/// Represents a purchase receipt.
/// </summary>
public class Receipt
{
    private readonly List<CartItem> _items;
    private readonly List<AppliedOffer> _appliedOffers;
    
    public decimal SubTotal { get; private set; }
    public decimal TotalDiscount { get; private set; }
    public decimal TotalPrice { get; private set; }

    public Receipt()
    {
        _items = new List<CartItem>();
        _appliedOffers = new List<AppliedOffer>();
    }

    public void AddItem(CartItem item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        _items.Add(item);
        RecalculateSubTotal();
    }

    public void AddItems(IEnumerable<CartItem> items)
    {
        if (items is null || !items.Any())
        {
            throw new ArgumentNullException(nameof(items));
        }

        foreach (var item in items)
        {
            _items.Add(item);
        }
        RecalculateSubTotal();
    }

    public void ApplyOffer(AppliedOffer offer)
    {
        ArgumentNullException.ThrowIfNull(offer);

        _appliedOffers.Add(offer);
        RecalculateTotals();
    }

    public void ApplyOffers(IEnumerable<AppliedOffer> offers)
    {
        if (offers is null || !offers.Any())
        {
            throw new ArgumentNullException(nameof(offers));
        }

        foreach (var offer in offers)
        {
            _appliedOffers.Add(offer);
        }
        RecalculateTotals();
    }

    public void RecalculateSubTotal()
    {
        SubTotal = _items.Sum(item => item.TotalPrice);
    }

    public void RecalculateTotals()
    {
        TotalDiscount = _appliedOffers.Sum(offer => offer.DiscountAmount);
        TotalPrice = Math.Max(0, SubTotal - TotalDiscount);
    }
}