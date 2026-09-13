using Microsoft.Data.Sqlite;

namespace apitest;

public class DataAccessModule
{
    private readonly string _connectionString;

    public DataAccessModule(string connectionString = "Data Source=test_shop.db")
    {
        _connectionString = connectionString;
    }

    public SqliteConnection CreateConnection()
    {
        return new SqliteConnection(_connectionString);
    }

    public async Task SetupDatabaseAsync()
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();
        await DatabaseInitializer.InitializeAsync(connection);
    }
}