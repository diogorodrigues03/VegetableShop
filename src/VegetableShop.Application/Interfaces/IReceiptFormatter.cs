using VegetableShop.Domain.Entities;

namespace VegetableShop.Application.Interfaces
{
    /// <summary>
    /// Defines a formatter for generating receipt output.
    /// </summary>
    public interface IReceiptFormatter
    {
        /// <summary>
        /// Formats a receipt for display.
        /// </summary>
        /// <param name="receipt">The receipt to format.</param>
        /// <returns>Formatted receipt string.</returns>
        string Format(Receipt receipt);
    }
}