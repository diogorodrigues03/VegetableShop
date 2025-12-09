using VegetableShop.Domain.Exceptions;
using VegetableShop.Infrastructure.Parsers;
using Xunit;

namespace VegetableShop.Tests.Infrastructure.Parsers
{
    public class CsvProductParserTests
    {
        private readonly string _testDataPath;

        public CsvProductParserTests()
        {
            var projectRoot = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
            _testDataPath = Path.Combine(projectRoot ?? string.Empty, "TestData");
            Directory.CreateDirectory(_testDataPath);
        }

        [Fact]
        public void ParseProducts_ValidFile_ReturnsProducts()
        {
            var filePath = Path.Combine(_testDataPath, "products_valid.csv");
            File.WriteAllText(filePath, "PRODUCT,PRICE\nTomato,0.75\nCarrot,1.0");
            var parser = new CsvProductParser();

            var products = parser.ParseProducts(filePath);

            Assert.Equal(2, products.Count);
            Assert.Equal("Tomato", products[0].Name);
            Assert.Equal(0.75m, products[0].Price);
        }

        [Fact]
        public void ParseProducts_FileNotFound_ThrowsFileNotFoundException()
        {
            var parser = new CsvProductParser();
            const string nonExistentFile = "nonexistent.csv";

            Assert.Throws<FileNotFoundException>(() => parser.ParseProducts(nonExistentFile));
        }

        [Fact]
        public void ParseProducts_InvalidPrice_ThrowsInvalidPriceException()
        {
            var filePath = Path.Combine(_testDataPath, "products_invalid_price.csv");
            File.WriteAllText(filePath, "PRODUCT,PRICE\nTomato,invalid");
            var parser = new CsvProductParser();

            Assert.Throws<InvalidPriceException>(() => parser.ParseProducts(filePath));
        }

        [Fact]
        public void ParseProducts_EmptyRows_SkipsThem()
        {
            var filePath = Path.Combine(_testDataPath, "products_with_empty.csv");
            File.WriteAllText(filePath, "PRODUCT,PRICE\nTomato,0.75\n,\nCarrot,1.0");
            var parser = new CsvProductParser();

            var products = parser.ParseProducts(filePath);

            Assert.Equal(2, products.Count);
            Assert.Contains(products, p => p.Name == "Carrot");
            Assert.Contains(products, p => p.Name == "Tomato");
            Assert.DoesNotContain(products, p => p.Name == "");
        }
    }
}