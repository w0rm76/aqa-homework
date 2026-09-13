using System.Text.Json.Serialization;

namespace apitest.DTO;

public class CreateUserRequestDto   
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("email")]
    public string Job { get; set; }
}