using Dapper;
using Microsoft.Data.Sqlite;

namespace apitest;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(SqliteConnection connection)
    {
        await CreateTablesAsync(connection);
        await SeedCategoriesAsync(connection);
        await SeedUsersAsync(connection);
        await SeedAddressesAsync(connection);
        await SeedProductsAsync(connection);
        await SeedOrdersAsync(connection);
        await SeedOrderItemsAsync(connection);
        await SeedReviewsAsync(connection);
    }

    private static async Task CreateTablesAsync(SqliteConnection connection)
    {
        const string sql = """
        PRAGMA foreign_keys = ON;

        CREATE TABLE IF NOT EXISTS Users
        (
            Id          INTEGER PRIMARY KEY AUTOINCREMENT,
            FirstName   TEXT NOT NULL,
            LastName    TEXT NOT NULL,
            Email       TEXT NOT NULL UNIQUE,
            Phone       TEXT,
            CreatedAt   TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS Addresses
        (
            Id          INTEGER PRIMARY KEY AUTOINCREMENT,
            UserId      INTEGER NOT NULL,
            City        TEXT NOT NULL,
            Street      TEXT NOT NULL,
            House       TEXT NOT NULL,
            Apartment   TEXT,
            FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
        );

        CREATE TABLE IF NOT EXISTS Categories
        (
            Id          INTEGER PRIMARY KEY AUTOINCREMENT,
            Name        TEXT NOT NULL UNIQUE
        );

        CREATE TABLE IF NOT EXISTS Products
        (
            Id          INTEGER PRIMARY KEY AUTOINCREMENT,
            Name        TEXT NOT NULL,
            Description TEXT,
            Price       REAL NOT NULL,
            Stock       INTEGER NOT NULL DEFAULT 0,
            CategoryId  INTEGER NOT NULL,
            FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
        );

        CREATE TABLE IF NOT EXISTS Orders
        (
            Id          INTEGER PRIMARY KEY AUTOINCREMENT,
            UserId      INTEGER NOT NULL,
            OrderDate   TEXT NOT NULL,
            Status      TEXT NOT NULL,
            TotalPrice  REAL NOT NULL DEFAULT 0,
            FOREIGN KEY (UserId) REFERENCES Users(Id)
        );

        CREATE TABLE IF NOT EXISTS OrderItems
        (
            Id          INTEGER PRIMARY KEY AUTOINCREMENT,
            OrderId     INTEGER NOT NULL,
            ProductId   INTEGER NOT NULL,
            Quantity    INTEGER NOT NULL,
            UnitPrice   REAL NOT NULL,
            FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
            FOREIGN KEY (ProductId) REFERENCES Products(Id)
        );

        CREATE TABLE IF NOT EXISTS Reviews
        (
            Id          INTEGER PRIMARY KEY AUTOINCREMENT,
            UserId      INTEGER NOT NULL,
            ProductId   INTEGER NOT NULL,
            Rating      INTEGER NOT NULL,
            Comment     TEXT,
            CreatedAt   TEXT NOT NULL,
            FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
            FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
            CHECK (Rating BETWEEN 1 AND 5),
            UNIQUE(UserId, ProductId)
        );
        """;

        await connection.ExecuteAsync(sql);
    }

    private static async Task SeedCategoriesAsync(SqliteConnection connection)
    {
        const string sql = "INSERT OR IGNORE INTO Categories (Name) VALUES (@Name);";
        var categories = new[] { new { Name = "Electronics" }, new { Name = "Books" }, new { Name = "Clothing" } };
        await connection.ExecuteAsync(sql, categories);
    }

    private static async Task SeedUsersAsync(SqliteConnection connection)
    {
        const string sql = "INSERT OR IGNORE INTO Users (FirstName, LastName, Email, Phone, CreatedAt) VALUES (@FirstName, @LastName, @Email, @Phone, @CreatedAt);";
        var users = new[]
        {
            new { FirstName = "Иван", LastName = "Петров", Email = "ivan.petrov@mail.ru", Phone = "+79990000001", CreatedAt = "2025-01-15" },
            new { FirstName = "Анна", LastName = "Соколова", Email = "anna.sokolova@mail.ru", Phone = "+79990000002", CreatedAt = "2025-01-20" }
        };
        await connection.ExecuteAsync(sql, users);
    }

    private static async Task SeedAddressesAsync(SqliteConnection connection)
    {
        const string sql = "INSERT INTO Addresses (UserId, City, Street, House, Apartment) VALUES (@UserId, @City, @Street, @House, @Apartment);";
        var addresses = new[] { new { UserId = 1, City = "Moscow", Street = "Lenina", House = "10", Apartment = "5" } };
        await connection.ExecuteAsync(sql, addresses);
    }

    private static async Task SeedProductsAsync(SqliteConnection connection)
    {
        const string sql = "INSERT INTO Products (Name, Description, Price, Stock, CategoryId) VALUES (@Name, @Description, @Price, @Stock, @CategoryId);";
        var products = new[] { new { Name = "Smartphone", Description = "Flagship", Price = 999.99, Stock = 10, CategoryId = 1 } };
        await connection.ExecuteAsync(sql, products);
    }

    private static async Task SeedOrdersAsync(SqliteConnection connection)
    {
        const string sql = "INSERT INTO Orders (UserId, OrderDate, Status, TotalPrice) VALUES (@UserId, @OrderDate, @Status, @TotalPrice);";
        var orders = new[] { new { UserId = 1, OrderDate = "2025-02-01", Status = "Completed", TotalPrice = 999.99 } };
        await connection.ExecuteAsync(sql, orders);
    }

    private static async Task SeedOrderItemsAsync(SqliteConnection connection)
    {
        const string sql = "INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice);";
        var items = new[] { new { OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 999.99 } };
        await connection.ExecuteAsync(sql, items);
    }

    private static async Task SeedReviewsAsync(SqliteConnection connection)
    {
        const string sql = "INSERT OR IGNORE INTO Reviews (UserId, ProductId, Rating, Comment, CreatedAt) VALUES (@UserId, @ProductId, @Rating, @Comment, @CreatedAt);";
        var reviews = new[] { new { UserId = 1, ProductId = 1, Rating = 5, Comment = "Great!", CreatedAt = "2025-02-02" } };
        await connection.ExecuteAsync(sql, reviews);
    }
}
