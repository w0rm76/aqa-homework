using System;

namespace MiniAutomationToolkit.Core.Models;

public record UserDto
{
    public string Name { get; init; }
    public string Email { get; init; }

    public UserDto(string name, string email)
    {
        // 1. Проверка имени на пустоту
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty.");
        }

        // 2. Проверка email на пустоту
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException($"Invalid email: {email}");
        }

        // 3. Проверка email на наличие символа @
        if (!email.Contains("@"))
        {
            throw new ArgumentException($"Invalid email: {email}");
        }

        // 4. Проверка email на отсутствие пробелов
        if (email.Contains(" "))
        {
            throw new ArgumentException($"Invalid email: {email}");
        }

        Name = name;
        Email = email;
    }
}