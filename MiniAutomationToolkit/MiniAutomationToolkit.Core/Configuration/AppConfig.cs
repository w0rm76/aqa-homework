using System;
using System.Collections.Generic;
using System.IO;

namespace MiniAutomationToolkit.Core.Configuration;

public class AppConfig
{
    private readonly Dictionary<string, string> _settings = new();

    public AppConfig(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Файл конфигурации не найден: {filePath}");
        }

        // Читаем файл построчно
        string[] lines = File.ReadAllLines(filePath);

        foreach (string rawLine in lines)
        {
            // 1. Игнорируем пустые строки
            if (string.IsNullOrWhiteSpace(rawLine))
            {
                continue;
            }

            string line = rawLine.Trim();

            // 2. Игнорируем строки-комментарии, начинающиеся с #
            if (line.StartsWith("#"))
            {
                continue;
            }

            // 3. Разделяем строку по первому символу '=' (максимум на 2 части)
            string[] parts = line.Split('=', 2);

            // 4. Валидация: строка без '=' или пустой ключ
            if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[0]))
            {
                throw new InvalidDataException($"Некорректный формат строки конфигурации: '{rawLine}'");
            }

            // 5. Удаляем пробелы по краям ключа и значения
            string key = parts[0].Trim();
            string value = parts[1].Trim();

            // 6. Валидация: повторяющийся ключ
            if (_settings.ContainsKey(key))
            {
                throw new InvalidDataException($"Повторяющийся ключ в конфигурации: '{key}'");
            }

            _settings.Add(key, value);
        }
    }

    public T GetSetting<T>(string key)
    {
        // 7. Проверка отсутствующего ключа
        if (!_settings.TryGetValue(key, out string? value))
        {
            throw new KeyNotFoundException($"Ключ '{key}' не найден в конфигурации.");
        }

        try
        {
            // 8. Преобразуем значение к запрошенному типу T
            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch (Exception ex) when (ex is FormatException or InvalidCastException or OverflowException)
        {
            // 9. Ошибка преобразования должна содержать ключ и ожидаемый тип
            throw new InvalidDataException($"Не удалось преобразовать ключ '{key}' к типу '{typeof(T).Name}'.", ex);
        }
    }
}
