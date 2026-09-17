using Microsoft.Extensions.DependencyInjection;

namespace apitest;

public class TestPrecondition
{
    public ServiceProvider Provider { get; }

    public TestPrecondition()
    {
        var services = new ServiceCollection();
        var dbPath = Path.Combine(AppContext.BaseDirectory, "test_shop.db");
        var connString = $"Data Source={dbPath}";
        services.AddDataAccess(connString);
        Provider = services.BuildServiceProvider();
    }
}