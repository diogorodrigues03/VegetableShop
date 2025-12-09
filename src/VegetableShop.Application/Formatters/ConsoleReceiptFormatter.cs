using System.Text;
using VegetableShop.Application.Constants;
using VegetableShop.Application.Interfaces;
using VegetableShop.Domain.Entities;
using VegetableShop.Domain.Utils;

namespace VegetableShop.Application.Formatters
{
    /// <summary>
    /// Formats receipts for console output.
    /// </summary>
    public class ConsoleReceiptFormatter : IReceiptFormatter
    {
        /// <summary>
        /// Formats a receipt for console display.
        /// </summary>
        public string Format(Receipt receipt)
        {
            ArgumentNullException.ThrowIfNull(receipt);

            var sb = new StringBuilder();

            // Header
            sb.AppendLine(FormattingConstants.Separator);
            sb.AppendLine(FormattingConstants.ReceiptHeader);
            sb.AppendLine(FormattingConstants.Separator);
            sb.AppendLine();

            // Items
            sb.AppendLine("ITEMS PURCHASED:");
            sb.AppendLine(FormattingConstants.LineSeparator);

            if (receipt.Items.Any())
            {
                foreach (var item in receipt.Items)
                {
                    sb.AppendLine($"{item.Product.Name,-15} x{item.Quantity,-5} €{FormattingUtils.FormatCurrency(item.Product.Price)} each   €{FormattingUtils.FormatCurrency(item.TotalPrice)}");
                }
            }
            else
            {
                sb.AppendLine("No items purchased.");
            }

            sb.AppendLine(FormattingConstants.LineSeparator);
            sb.AppendLine($"{"SUBTOTAL:",-35} €{FormattingUtils.FormatCurrency(receipt.SubTotal)}");
            sb.AppendLine();

            // Offers
            if (receipt.AppliedOffers.Any())
            {
                sb.AppendLine("OFFERS APPLIED:");
                sb.AppendLine(FormattingConstants.LineSeparator);
                
                foreach (var offer in receipt.AppliedOffers)
                {
                    sb.AppendLine($"• {TextWrap(offer.Description, FormattingConstants.MaxReceiptLineLength)}");
                    sb.AppendLine($"• Discount: -€{FormattingUtils.FormatCurrency(offer.DiscountAmount)}");
                    sb.AppendLine();
                }
                
                sb.AppendLine(FormattingConstants.LineSeparator);
                sb.AppendLine($"{"TOTAL SAVINGS:",-35} €{FormattingUtils.FormatCurrency(receipt.TotalDiscount)}");
                sb.AppendLine();
            }
            else
            {
                sb.AppendLine("No offers applied.");
                sb.AppendLine();
            }

            // Total
            sb.AppendLine(FormattingConstants.Separator);
            sb.AppendLine($"{"TOTAL TO PAY:",-35} €{FormattingUtils.FormatCurrency(receipt.TotalPrice)}");
            sb.AppendLine(FormattingConstants.Separator);

            return sb.ToString();
        }
        
        private static string TextWrap(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value) || maxLength < 1) return value;
            var sb = new StringBuilder();
            for (int i = 0; i < value.Length; i += maxLength)
            {
                int len = Math.Min(maxLength, value.Length - i);
                sb.AppendLine(value.Substring(i, len));
            }
            return sb.ToString().TrimEnd();
        }
    }
}