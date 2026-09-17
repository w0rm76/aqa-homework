using System.Data;
using apitest.DTO;
using apitest.Interfaces;
using Dapper;

namespace apitest.DapperRepository;

public class UserRepository : IUserRepository
{
    private readonly DataAccessModule _dbModule;

    public UserRepository(DataAccessModule dbModule)
    {
        _dbModule = dbModule;
    }

    public async Task<UserDataDto?> GetUserByEmailAsync(string email)
    {
        using var connection = _dbModule.CreateConnection();
        await connection.OpenAsync();

        const string sql = "SELECT * FROM Users WHERE Email = @Email;";
        
        // Dapper автоматически разложит данные из БД в поля класса UserDataDto
        return await connection.QueryFirstOrDefaultAsync<UserDataDto>(sql, new { Email = email });
    }
}