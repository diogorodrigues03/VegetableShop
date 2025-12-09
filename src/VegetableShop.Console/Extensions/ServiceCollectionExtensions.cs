using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VegetableShop.Application.Formatters;
using VegetableShop.Application.Interfaces;
using VegetableShop.Application.Services;
using VegetableShop.Console.Configuration;
using VegetableShop.Console.Services;
using VegetableShop.Domain.Interfaces;
using VegetableShop.Infrastructure.Repositories;

namespace VegetableShop.Console.Extensions
{
    /// <summary>
    /// Extension methods for configuring services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all application services to the service collection.
        /// </summary>
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.Configure<FileSettings>(configuration.GetSection("FileSettings"));
            var fileSettings = configuration.GetSection("FileSettings").Get<FileSettings>() 
                ?? new FileSettings();
            
            services.AddSingleton<IProductRepository>(sp => 
                new FileProductRepository(fileSettings.ProductsFile));
            
            services.AddSingleton<IPurchaseRepository>(sp => 
                new FilePurchaseRepository(fileSettings.PurchaseFile));

            // Register application services here
            services.AddSingleton<IPricingService, PricingService>();
            services.AddSingleton<IOfferConfigurationService, OfferConfigurationService>();
            services.AddTransient<ICheckoutService, CheckoutService>();
            
            services.AddSingleton<IReceiptFormatter, ConsoleReceiptFormatter>();

            // Register main application service
            services.AddTransient<IVegetableShopApplication, VegetableShopApplication>();

            return services;
        }
    }
}