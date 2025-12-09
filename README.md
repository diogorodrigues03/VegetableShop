# Vegetable Shop - Checkout System

A robust, configurable, and extensible console application built with **.NET 9 (C#)** that simulates a supermarket checkout process. This project demonstrates **Clean Architecture** principles, **SOLID** design, and industry-standard patterns.

## 📋 Project Overview

The application reads a product catalog and a list of purchased items from CSV files, applies various promotional offers, calculates the final total, and generates a detailed receipt.

## 🏗 Architecture & Design

The solution follows a modular **Clean Architecture** (Onion Architecture) approach to separate concerns and ensure testability:

1.  **VegetableShop.Domain**: The core. Contains Enterprise Logic, Entities (`Product`, `ShoppingCart`, `Receipt`), Value Objects, Interfaces (`IPromotionalOffer`), and Custom Exceptions. It has *zero* dependencies on other projects.
2.  **VegetableShop.Application**: Contains Application Logic. It orchestrates the flow using services like `CheckoutService` and `PricingService`. It defines interfaces that the Infrastructure layer must implement.
3.  **VegetableShop.Infrastructure**: Handles external concerns. Implements Repositories (File I/O), Parsers (CSV handling), and concrete implementations of interfaces defined in the core.
4.  **VegetableShop.Console**: The Entry Point. Handles composition (DI), configuration (`appsettings.json`), and user interaction.

### Key Design Patterns

*   **Dependency Injection (DI)**: Used extensively throughout the application. All dependencies are injected via constructor injection, ensuring loose coupling and easy unit testing. The `Microsoft.Extensions.DependencyInjection` container is used for wiring up services.
*   **Strategy Pattern**: The promotional system is built on the Strategy pattern. The `IPromotionalOffer` interface allows different pricing strategies (e.g., "3 for 2", "Buy X Get Y Free") to be applied interchangeably without modifying the `PricingService`.
*   **Repository Pattern**: `IProductRepository` and `IPurchaseRepository` abstract the data access logic. While the current implementation reads from CSV files, this pattern allows swapping to a Database or API source without changing the Application logic.
*   **Factory Pattern**: The `OfferFactory` is responsible for instantiating the correct concrete `IPromotionalOffer` strategies based on configuration, adhering to the Open/Closed Principle.
*   **Options Pattern**: Used `IOptions<FileSettings>` to inject strongly-typed configuration settings from `appsettings.json`.

## 🛠 Features

*   **Dynamic Data Loading**: Loads products and purchase orders from external CSV files.
*   **Robust Logging**: Implements **Serilog** for structured logging to file, separating operational logs from user output.
*   **Graceful Error Handling**: Custom exception handling for product lookups, invalid prices, or missing files, ensuring the app exits gracefully with meaningful messages.
*   **Timestamped Receipts**: Option to save receipts to unique, timestamped text files.

## 🏷️ Supported Offers

The application currently supports the following promotion types:

1.  **Buy X Pay for Y**: (e.g., "Buy 3 Apples, Pay for 2").
2.  **Buy X Get Product Y Free**: (e.g., "Buy 2 Tomatoes, get 1 Curry Sauce free").
3.  **Spend Threshold Discount**: (e.g., "5% off when you spend over €20").

*Offers are applied dynamically based on the state of the Shopping Cart.*

## 🚀 Getting Started

### Prerequisites
*   .NET 9.0 SDK

### Running the Application

You can run the application directly from the CLI. By default, it looks for data in `TestData/products.csv` and `TestData/purchase.csv`.

```bash
# Run with default settings
dotnet run --project src/VegetableShop.Console
```

### Custom Arguments

You can override the input files and save the receipt using arguments:

```bash
dotnet run --project src/VegetableShop.Console -- [path-to-products] [path-to-purchase] [--save]
```

*   **Argument 1**: Path to `products.csv` (optional).
*   **Argument 2**: Path to `purchase.csv` (optional).
*   **--save**: If provided, saves the receipt to a timestamped file (e.g., `receipt_20251209_153000.txt`).

**Example:**
```bash
dotnet run --project src/VegetableShop.Console -- data/my_products.csv data/my_list.csv --save
```

## 📂 File Formats

### Products CSV
Format: `Product name, Price`
```csv
PRODUCT,PRICE
Carrot,0.50
Aubergine,0.70
Tomato,0.30
```

### Purchase CSV
Format: `Product Name, Quantity`
```csv
PRODUCT,QUANTITY
Carrot,3
Aubergine,1
Tomato,5
```

## 🧪 Testing

The solution includes a comprehensive unit testing suite (`VegetableShop.Tests`) using **xUnit** and **Moq**.

```bash
dotnet test
```

The tests cover:
*   Domain logic (Shopping Cart calculations).
*   Offer strategies (verification of discount logic).
*   CSV Parsing edge cases.
*   Service orchestration.
