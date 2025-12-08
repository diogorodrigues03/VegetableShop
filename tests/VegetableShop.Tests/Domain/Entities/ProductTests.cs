using VegetableShop.Domain.Entities;
using Xunit;

namespace VegetableShop.Tests.Domain.Entities;

public class ProductTests
{
    [Fact]
    public void Constructor_ValidInputs_CreatesProduct()
    {
        var product = new Product("Tomato", 0.75m);
        
        Assert.Equal("Tomato", product.Name);
        Assert.Equal(0.75m, product.Price);
    }
    
    [Fact]
    public void Constructor_EmptyName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Product("", 0.75m));
    }

    [Fact]
    public void Constructor_NullName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Product(null!, 0.75m));
    }
    
    [Fact]
    public void Constructor_NegativePrice_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Product("Tomato", -0.75m));
    }

    [Fact]
    public void Constructor_WhitespaceName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Product("   ", 0.75m));
    }

    [Fact]
    public void Constructor_TrimsWhitespace()
    {
        var product = new Product("  Tomato  ", 0.75m);

        Assert.Equal("Tomato", product.Name);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(0.01)]
    [InlineData(100.99)]
    public void Constructor_ValidPrice_CreatesProduct(decimal price)
    {
        var product = new Product("Tomato", price);

        Assert.Equal(price, product.Price);
    }

    [Fact]
    public void Equals_SameProductName_ReturnsTrue()
    {
        var product1 = new Product("Tomato", 0.75m);
        var product2 = new Product("Tomato", 0.80m); // Different price

        Assert.Equal(product1, product2);
    }
    
    [Fact]
    public void Equals_DifferentProductName_ReturnsFalse()
    {
        var product1 = new Product("Tomato", 0.75m);
        var product2 = new Product("Carrot", 0.75m);
        
        Assert.NotEqual(product1, product2);
    }
    
    [Fact]
    public void Equals_CaseInsensitive_ReturnsTrue()
    {
        var product1 = new Product("Tomato", 0.75m);
        var product2 = new Product("tomato", 0.75m);
        
        Assert.Equal(product1, product2);
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        var product = new Product("Tomato", 0.75m);
        var result = product.ToString();
        
        Assert.Equal("Tomato - €0.75", result);
    }
}