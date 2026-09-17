using apitest.DTO;

namespace apitest.Interfaces;

public interface IUserRepository
{
    // Используем готовый класс из вашего проекта
    Task<UserDataDto?> GetUserByEmailAsync(string email);
}