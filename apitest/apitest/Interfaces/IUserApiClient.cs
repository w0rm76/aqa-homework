using apitest.DTO;
using Refit;
using Microsoft.Extensions.DependencyInjection;

namespace apitest.Interfaces;

[Headers("x-api-key: free_user_3I2p6kivsIonVyzzfjEeWUiivHG")]
public interface IUserApiClient
{
    [Get("/users/{id}")]
    Task<UserResponseDto> GetUserAsync(int id);

    [Post("/users")]
    Task<CreateUserResponseDto> PostUserAsync([Body] CreateUserRequestDto user);
    
    [Put("/users/{id}")]
    Task<CreateUserRequestDto> PutUserAsync(int id, [Body] CreateUserRequestDto user);
    
    [Delete("/users/{id}")]
    Task<ApiResponse<string>> DeleteUserAsync(int id);
}