using System;
using System.IO;

namespace MiniAutomationToolkit.Core.Services;

public class ErrorLogger
{
    public string? TryReadFile(string sourceFilePath, string logFilePath)
    {
        try
        {
            // Пытаемся прочитать файл целиком
            return File.ReadAllText(sourceFilePath);
        }
        catch (Exception ex) when (ex is FileNotFoundException or UnauthorizedAccessException)
        {
            // Форматируем строку лога по условию задания
            string logRecord = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {ex.GetType().Name} | {ex.Message}{Environment.NewLine}";
            
            // Записываем ошибку в лог (файл создается автоматически, старые записи не затираются)
            File.AppendAllText(logFilePath, logRecord);
            
            // При ошибке возвращаем null
            return null;
        }
    }
}