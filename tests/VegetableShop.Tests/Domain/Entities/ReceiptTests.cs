using VegetableShop.Domain.Entities;
using Xunit;

namespace VegetableShop.Tests.Domain.Entities;

public class ReceiptTests
{
    [Fact]
        public void Constructor_InitializesEmptyReceipt()
        {
            var receipt = new Receipt();

            Assert.Empty(receipt.Items);
            Assert.Empty(receipt.AppliedOffers);
            Assert.Equal(0m, receipt.SubTotal);
            Assert.Equal(0m, receipt.TotalDiscount);
            Assert.Equal(0m, receipt.TotalPrice);
        }

        [Fact]
        public void AddItem_ValidItem_AddsToReceipt()
        {
            var receipt = new Receipt();
            var product = new Product("Tomato", 0.75m);
            var cartItem = new CartItem(product, 3);

            receipt.AddItem(cartItem);

            Assert.Single(receipt.Items);
            Assert.Equal(2.25m, receipt.SubTotal);
            Assert.Equal(2.25m, receipt.TotalPrice);
        }

        [Fact]
        public void AddItem_NullItem_ThrowsArgumentNullException()
        {
            var receipt = new Receipt();

            Assert.Throws<ArgumentNullException>(() => receipt.AddItem(null!));
        }

        [Fact]
        public void AddItems_MultipleItems_CalculatesSubtotalCorrectly()
        {
            var receipt = new Receipt();
            var product1 = new Product("Tomato", 0.75m);
            var product2 = new Product("Carrot", 1.0m);
            var items = new List<CartItem>
            {
                new (product1, 3),
                new (product2, 2)
            };

            receipt.AddItems(items);

            Assert.Equal(2, receipt.Items.Count);
            Assert.Equal(4.25m, receipt.SubTotal);
            Assert.Equal(4.25m, receipt.TotalPrice);
        }

        [Fact]
        public void AddItems_NullCollection_ThrowsArgumentNullException()
        {
            var receipt = new Receipt();

            Assert.Throws<ArgumentNullException>(() => receipt.AddItems(null!));
        }

        [Fact]
        public void ApplyOffer_ValidOffer_AppliesDiscount()
        {
            var receipt = new Receipt();
            var product = new Product("Tomato", 0.75m);
            var cartItem = new CartItem(product, 3);
            receipt.AddItem(cartItem);

            var offer = new AppliedOffer("Buy 2 Get 1 Free", 0.75m);

            receipt.ApplyOffer(offer);

            Assert.Single(receipt.AppliedOffers);
            Assert.Equal(0.75m, receipt.TotalDiscount);
            Assert.Equal(1.50m, receipt.TotalPrice);
        }

        [Fact]
        public void ApplyOffer_NullOffer_ThrowsArgumentNullException()
        {
            var receipt = new Receipt();

            Assert.Throws<ArgumentNullException>(() => receipt.ApplyOffer(null!));
        }

        [Fact]
        public void ApplyOffers_MultipleOffers_AppliesAllDiscounts()
        {
            var receipt = new Receipt();
            var product = new Product("Tomato", 0.75m);
            receipt.AddItem(new CartItem(product, 5));

            var offers = new List<AppliedOffer>
            {
                new ("Offer 1", 0.75m),
                new ("Offer 2", 0.50m)
            };

            receipt.ApplyOffers(offers);

            Assert.Equal(2, receipt.AppliedOffers.Count);
            Assert.Equal(1.25m, receipt.TotalDiscount);
            Assert.Equal(2.50m, receipt.TotalPrice);
        }

        [Fact]
        public void ApplyOffers_NullCollection_ThrowsArgumentNullException()
        {
            var receipt = new Receipt();

            Assert.Throws<ArgumentNullException>(() => receipt.ApplyOffers(null!));
        }

        [Fact]
        public void Total_NeverNegative_WhenDiscountExceedsSubtotal()
        {
            var receipt = new Receipt();
            var product = new Product("Tomato", 0.75m);
            var cartItem = new CartItem(product, 1);
            receipt.AddItem(cartItem);

            var offer = new AppliedOffer("Huge Discount", 10.0m);

            receipt.ApplyOffer(offer);

            Assert.Equal(0m, receipt.TotalPrice);
            Assert.Equal(0.75m, receipt.SubTotal);
            Assert.Equal(10.0m, receipt.TotalDiscount);
        }

        [Fact]
        public void Items_ReturnsReadOnlyCollection()
        {
            var receipt = new Receipt();
            var product = new Product("Tomato", 0.75m);
            receipt.AddItem(new CartItem(product, 3));

            var items = receipt.Items;

            Assert.IsAssignableFrom<IReadOnlyList<CartItem>>(items);
        }

        [Fact]
        public void AppliedOffers_ReturnsReadOnlyCollection()
        {
            var receipt = new Receipt();
            var offer = new AppliedOffer("Test Offer", 1.0m);
            receipt.ApplyOffer(offer);

            var offers = receipt.AppliedOffers;

            Assert.IsAssignableFrom<IReadOnlyList<AppliedOffer>>(offers);
        }
}