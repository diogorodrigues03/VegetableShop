using FluentAssertions;
using VegetableShop.Infrastructure.Configuration;
using VegetableShop.Infrastructure.Repositories;

namespace VegetableShop.Tests.Infrastructure.Repositories
{
    public class FileProductRepositoryTests : IDisposable
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
        public async Task GetAllProductsAsync_ShouldReturnProducts_WhenFileIsValid()
        {
            var csvContent = "PRODUCT,PRICE\nCarrot,0.50\nTomato,0.30";
            await File.WriteAllTextAsync(_tempFilePath, csvContent);

            var config = new FileRepositoryConfiguration { ProductsFilePath = _tempFilePath };
            var repository = new FileProductRepository(config);

            var products = await repository.GetAllProductsAsync();

            products.Should().HaveCount(2);
            products.Should().Contain(p => p.Name == "Carrot" && p.Price == 0.50m);
            products.Should().Contain(p => p.Name == "Tomato" && p.Price == 0.30m);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnEmpty_WhenFileIsEmpty()
        {
            await File.WriteAllTextAsync(_tempFilePath, "");

            var config = new FileRepositoryConfiguration { ProductsFilePath = _tempFilePath };
            var repository = new FileProductRepository(config);

            var products = await repository.GetAllProductsAsync();

            products.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldThrowException_WhenFileDoesNotExist()
        {
            var nonExistentFile = "non_existent_file.csv";
            var config = new FileRepositoryConfiguration { ProductsFilePath = nonExistentFile };
            var repository = new FileProductRepository(config);

            Func<Task> act = async () => await repository.GetAllProductsAsync();

            await act.Should().ThrowAsync<FileNotFoundException>();
        }
        
        [Fact]
        public async Task GetProductByNameAsync_ShouldReturnProduct_WhenExists()
        {
            var csvContent = "PRODUCT,PRICE\nCarrot,0.50";
            await File.WriteAllTextAsync(_tempFilePath, csvContent);
            
            var config = new FileRepositoryConfiguration { ProductsFilePath = _tempFilePath };
            var repository = new FileProductRepository(config);

            var product = await repository.GetProductByNameAsync("Carrot");

            product.Should().NotBeNull();
            product!.Name.Should().Be("Carrot");
        }
        
        [Fact]
        public async Task GetProductByNameAsync_ShouldReturnNull_WhenNotExists()
        {
            var csvContent = "PRODUCT,PRICE\nCarrot,0.50";
            await File.WriteAllTextAsync(_tempFilePath, csvContent);
            
            var config = new FileRepositoryConfiguration { ProductsFilePath = _tempFilePath };
            var repository = new FileProductRepository(config);

            var product = await repository.GetProductByNameAsync("Potato");

            product.Should().BeNull();
        }
    }
}