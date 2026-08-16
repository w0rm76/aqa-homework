using System;
using System.Collections.Generic;
using System.IO;

namespace MiniAutomationToolkit.Core.Helpers;

public static class FileSearcher
{
    public static List<string> SearchFiles(string directoryPath, string searchPattern)
    {
        var foundFiles = new List<string>();

        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine($"Ошибка: Директория '{directoryPath}' не существует.");
            return foundFiles;
        }

        try
        {
            string[] files = Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
            foundFiles.AddRange(files);
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"Предупреждение: Отказ в доступе к директории или её подпапкам: '{directoryPath}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла непредвиденная ошибка при поиске: {ex.Message}");
        }

        return foundFiles;
    }
}