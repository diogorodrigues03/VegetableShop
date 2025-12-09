namespace VegetableShop.Console.Services
{
    /// <summary>
    /// Main application service for the vegetable shop.
    /// </summary>
    public interface IVegetableShopApplication
    {
        /// <summary>
        /// Runs the application with the provided arguments.
        /// </summary>
        Task<ExitCodes> RunAsync(string[] args);
    }
}