using System.Net;
using apitest.DTO;
using apitest.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace apitest;

public class RefitTests
{
    private IUserApiClient _client;

    [OneTimeSetUp]
    public void Setup()
    {
        var services = new ServiceCollection();
        services.AddRefitClient<IUserApiClient>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri("https://reqres.in/api");
            });
        var provider = services.BuildServiceProvider();
        _client = provider.GetRequiredService<IUserApiClient>();
        //client.DefaultRequestHeaders.Add("x-api-key", "free_user_3I2p6kivsIonVyzzfjEeWUiivHG");
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
        var newUser = new CreateUserRequestDto {Name = "Alex", Job = "Samsung"};
        var response = await _client.PostUserAsync(newUser);
        Assert.That(response.Name, Is.EqualTo("Alex"));
    }
    
    [Test]
    public async Task Test3()
    {
        var updateUser = new CreateUserRequestDto {Name = "Alex", Job = "Apple"};
        var response = await _client.PutUserAsync(2,  updateUser);
        Assert.That(response.Job, Is.EqualTo("Apple"));
    }
    
    [Test]
    public async Task Test4()
    {
        var response = await _client.DeleteUserAsync(2);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
}