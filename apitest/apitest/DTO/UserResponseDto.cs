using System.Text.Json.Serialization;

namespace apitest.DTO;

public class UserResponseDto
{
    [JsonPropertyName("data")]
    public UserDataDto Data { get; set; }
}