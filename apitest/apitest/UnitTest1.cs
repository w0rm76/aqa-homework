using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using apitest.DTO;

namespace apitest;

public class Tests
{
    private static HttpClient client;
    
    // works only for current class Tests
    [OneTimeSetUp]
    public void Setup()
    {
        client = new HttpClient()
        {
            BaseAddress = new Uri("https://reqres.in/api/")
        };
        client.DefaultRequestHeaders.Add("x-api-key", "free_user_3I2p6kivsIonVyzzfjEeWUiivHG");
    }

    [Test]
    public async Task Test1()
    {
        // GET-request
        using HttpResponseMessage response = await client.GetAsync("users/2");
        // var a = response.Content;
        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task Test2()
    {
        using HttpResponseMessage response = await client.GetAsync("users/2");
        string jsonGet = await response.Content.ReadAsStringAsync();
        UserResponseDto userResponce = JsonSerializer.Deserialize<UserResponseDto>(jsonGet);
        UserDataDto user = userResponce.Data;
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        client.Dispose();
    }
}

// free_user_3I2p6kivsIonVyzzfjEeWUiivHG