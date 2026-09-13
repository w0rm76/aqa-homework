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
        if (user.Id == 2)
        {
            
        } 
        else 
        {
            throw new Exception();
        }
    }
    
    [Test]
    public async Task Test3()
    {
        // POST-request 
        var newUser = new { name = "Alex", job = "Sloter" };
        //var newUser = new CreateUserRequestDto();

        // req
        using HttpResponseMessage response = await client.PostAsJsonAsync("users", newUser);
        response.EnsureSuccessStatusCode();

        // resp
        string jsonPost = await response.Content.ReadAsStringAsync();
        CreateUserResponseDto userResponce = JsonSerializer.Deserialize<CreateUserResponseDto>(jsonPost);
        Console.WriteLine($"User has been created with id = {userResponce.Id}");
    }
    
    [Test]
    public async Task Test4()
    {
        // PUT-request
        var updatedUser = new { job = "Other Sloter" };

        // req
        using HttpResponseMessage response = await client.PutAsJsonAsync("users/2", updatedUser);
        response.EnsureSuccessStatusCode();

        // resp
        string jsonPut = await response.Content.ReadAsStringAsync();
        UserResponseDto userResponce = JsonSerializer.Deserialize<UserResponseDto>(jsonPut);
    }
    
    [Test]
    public async Task Test5()
    {
        // DELETE-request
        using HttpResponseMessage response = await client.DeleteAsync("users/25");
        // var a = response.Content;
        response.EnsureSuccessStatusCode();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        client.Dispose();
    }
}

// free_user_3I2p6kivsIonVyzzfjEeWUiivHG