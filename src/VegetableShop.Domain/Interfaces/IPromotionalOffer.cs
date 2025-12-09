using VegetableShop.Domain.Entities;

namespace VegetableShop.Domain.Interfaces;

/// <summary>
/// Defines a promotional offer that can be applied to a purchase.
/// </summary>
public interface IPromotionalOffer
{
    string ProductName { get; }
    
    /// <summary>
    /// Calculates the discount for the specified item.
    /// </summary>
    /// <param name="item">The item to evaluate.</param>
    /// <returns>An AppliedOffer if applicable, otherwise null.</returns>
    AppliedOffer? CalculateDiscount(CartItem item);
}