using VegetableShop.Domain.Entities;
using Xunit;

namespace VegetableShop.Tests.Domain.Entities;

public class AppliedOfferTests
{
    [Fact]
    public void Constructor_ValidInputs_CreatesAppliedOffer()
    {
        var offer = new AppliedOffer("Buy 2 Get 1 Free", 0.75m);

        Assert.Equal("Buy 2 Get 1 Free", offer.Description);
        Assert.Equal(0.75m, offer.DiscountAmount);
    }

    [Fact]
    public void Constructor_EmptyDescription_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new AppliedOffer("", 0.75m));
    }

    [Fact]
    public void Constructor_NullDescription_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new AppliedOffer(null!, 0.75m));
    }

    [Fact]
    public void Constructor_NegativeDiscount_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new AppliedOffer("Discount", -0.75m));
    }

    [Fact]
    public void Constructor_ZeroDiscount_CreatesAppliedOffer()
    {
        var offer = new AppliedOffer("No Discount", 0m);

        Assert.Equal(0m, offer.DiscountAmount);
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        var offer = new AppliedOffer("Buy 2 Get 1 Free", 0.75m);
        var result = offer.ToString();

        Assert.Equal("Buy 2 Get 1 Free - €0.75 discount", result);
    }
}