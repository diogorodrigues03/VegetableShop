using VegetableShop.Domain.Exceptions;
using VegetableShop.Infrastructure.Parsers;
using Xunit;

namespace VegetableShop.Tests.Infrastructure.Parsers
{
    public class CsvProductParserTests : IDisposable
    {
        private readonly string _tempFilePath = Path.GetTempFileName();

        public void Dispose()
        {
            if (File.Exists(_tempFilePath))
            {
                File.Delete(_tempFilePath);
            }
        }

        [Fact]
        public void ParseProducts_ValidFile_ReturnsProducts()
        {
            File.WriteAllText(_tempFilePath, "PRODUCT,PRICE\nTomato,0.75\nCarrot,1.0");
            var parser = new CsvProductParser();

            var products = parser.ParseProducts(_tempFilePath);

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
            File.WriteAllText(_tempFilePath, "PRODUCT,PRICE\nTomato,invalid");
            var parser = new CsvProductParser();

            Assert.Throws<InvalidPriceException>(() => parser.ParseProducts(_tempFilePath));
        }

        [Fact]
        public void ParseProducts_EmptyRows_SkipsThem()
        {
            File.WriteAllText(_tempFilePath, "PRODUCT,PRICE\nTomato,0.75\n,\nCarrot,1.0");
            var parser = new CsvProductParser();

            var products = parser.ParseProducts(_tempFilePath);

            Assert.Equal(2, products.Count);
            Assert.Contains(products, p => p.Name == "Carrot");
            Assert.Contains(products, p => p.Name == "Tomato");
            Assert.DoesNotContain(products, p => p.Name == "");
        }
    }
}