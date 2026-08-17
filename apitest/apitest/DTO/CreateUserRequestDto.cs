using System.Text.Json.Serialization;

namespace apitest.DTO;

public class CreateUserRequestDto   
{
    [JsonPropertyName("name")]
    public int Name { get; set; }
    [JsonPropertyName("email")]
    public string Job { get; set; }
}