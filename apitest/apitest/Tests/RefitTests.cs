using System.Net;
using apitest.DTO;
using apitest.Interfaces;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace apitest;

public class RefitTests
{
    private IUserApiClient _client;
    private DataAccessModule _dbModule;
    private TestPrecondition _precondition;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _precondition = new TestPrecondition();
        _dbModule = _precondition.Provider.GetRequiredService<DataAccessModule>();
        await _dbModule.SetupDatabaseAsync();

        var services = new ServiceCollection();
        services.AddRefitClient<IUserApiClient>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri("https://reqres.in/api");
            });
        var provider = services.BuildServiceProvider();
        _client = provider.GetRequiredService<IUserApiClient>();
    }

    [Test]
    public async Task Test1()
    {
        var response = await _client.GetUserAsync(2);
        Assert.Multiple(() =>
        {
            Assert.That(response.Data.Id, Is.EqualTo(2));
            Assert.That(response.Data.Email, Is.Not.Null);
        });
    }

    [Test]
    public async Task Test2()
    {
        var newUser = new CreateUserRequestDto { Name = "Alex", Job = "Samsung" };
        var response = await _client.PostUserAsync(newUser);
        Assert.That(response.Name, Is.EqualTo("Alex"));
    }

    [Test]
    public async Task Test3()
    {
        var updateUser = new CreateUserRequestDto { Name = "Alex", Job = "Apple" };
        var response = await _client.PutUserAsync(2, updateUser);
        Assert.That(response.Job, Is.EqualTo("Apple"));
    }

    [Test]
    public async Task Test4()
    {
        var response = await _client.DeleteUserAsync(2);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task Test_Database_GetOrCreateUser()
    {
        using var connection = _dbModule.CreateConnection();
        await connection.OpenAsync();

        const string sql = "SELECT * FROM Users WHERE Email = @Email;";
        var dbUser = await connection.QueryFirstOrDefaultAsync(sql, new { Email = "ivan.petrov@mail.ru" });

        Assert.Multiple(() =>
        {
            Assert.That(dbUser, Is.Not.Null, "Пользователь не найден в базе данных!");
            Assert.That(dbUser.FirstName, Is.EqualTo("Иван"));
            Assert.That(dbUser.LastName, Is.EqualTo("Петров"));
        });
    }
}
