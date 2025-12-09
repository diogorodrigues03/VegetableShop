using System.Globalization;

namespace VegetableShop.Domain.Utils;

public static class FormattingUtils
{
    public static string FormatCurrency(decimal value) => value.ToString("F2", CultureInfo.CurrentCulture);
}