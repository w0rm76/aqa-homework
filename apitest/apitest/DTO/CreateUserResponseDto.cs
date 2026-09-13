using System.Text.Json.Serialization;

namespace apitest.DTO;

public class CreateUserResponseDto   
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("job")]
    public string Job { get; set; }
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("createdAt")]
    public string CreatedAt { get; set; }
}