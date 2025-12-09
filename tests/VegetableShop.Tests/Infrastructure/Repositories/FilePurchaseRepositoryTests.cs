using FluentAssertions;
using VegetableShop.Infrastructure.Configuration;
using VegetableShop.Infrastructure.Repositories;

namespace VegetableShop.Tests.Infrastructure.Repositories
{
    public class FilePurchaseRepositoryTests : IDisposable
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
        public async Task GetPurchaseItemsAsync_ShouldReturnItems_WhenFileIsValid()
        {
            var csvContent = "PRODUCT,QUANTITY\nCarrot,3\nTomato,5";
            await File.WriteAllTextAsync(_tempFilePath, csvContent);

            var config = new FileRepositoryConfiguration { PurchaseFilePath = _tempFilePath };
            var repository = new FilePurchaseRepository(config);

            var items = await repository.GetPurchaseItemsAsync();

            items.Should().HaveCount(2);
            items.Should().ContainKey("Carrot").WhoseValue.Should().Be(3);
            items.Should().ContainKey("Tomato").WhoseValue.Should().Be(5);
        }

        [Fact]
        public async Task GetPurchaseItemsAsync_ShouldAggregateQuantities_WhenDuplicatesExist()
        {
            var csvContent = "PRODUCT,QUANTITY\nCarrot,3\nCarrot,2";
            await File.WriteAllTextAsync(_tempFilePath, csvContent);

            var config = new FileRepositoryConfiguration { PurchaseFilePath = _tempFilePath };
            var repository = new FilePurchaseRepository(config);

            var items = await repository.GetPurchaseItemsAsync();

            items.Should().HaveCount(1);
            items.Should().ContainKey("Carrot").WhoseValue.Should().Be(5);
        }

        [Fact]
        public async Task GetPurchaseItemsAsync_ShouldThrowException_WhenFileDoesNotExist()
        {
            var nonExistentFile = "non_existent_file.csv";
            var config = new FileRepositoryConfiguration { PurchaseFilePath = nonExistentFile };
            var repository = new FilePurchaseRepository(config);

            Func<Task> act = async () => await repository.GetPurchaseItemsAsync();

            await act.Should().ThrowAsync<FileNotFoundException>();
        }
    }
}