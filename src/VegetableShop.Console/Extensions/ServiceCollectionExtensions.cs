using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VegetableShop.Application.Formatters;
using VegetableShop.Application.Interfaces;
using VegetableShop.Application.Services;
using VegetableShop.Console.Configuration;
using VegetableShop.Console.Interfaces;
using VegetableShop.Console.Services;
using VegetableShop.Domain.Interfaces;
using VegetableShop.Infrastructure.Configuration;
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
            
            // Register Runtime Configuration
            services.AddSingleton(new FileRepositoryConfiguration 
            {
                ProductsFilePath = fileSettings.ProductsFile,
                PurchaseFilePath = fileSettings.PurchaseFile
            });
            
            services.AddSingleton<IProductRepository, FileProductRepository>();
            services.AddSingleton<IPurchaseRepository, FilePurchaseRepository>();

            // Register application services here
            services.AddSingleton<IOfferFactory, OfferFactory>();
            services.AddSingleton<IPricingService, PricingService>();
            services.AddSingleton<IOfferConfigurationService, OfferConfigurationService>();
            services.AddTransient<ICheckoutService, CheckoutService>();
            services.AddSingleton<IExceptionHandler, VegetableShopExceptionHandler>();
            
            services.AddSingleton<IReceiptFormatter, ConsoleReceiptFormatter>();

            // Register the main application service
            services.AddTransient<IVegetableShopApplication, VegetableShopApplication>();

            return services;
        }
    }
}