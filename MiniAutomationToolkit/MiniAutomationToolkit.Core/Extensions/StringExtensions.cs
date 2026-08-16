using System;

namespace MiniAutomationToolkit.Core.Extensions;

public static class StringExtensions
{
    public static bool HasHttpScheme(this string? input)
    {
        // 1. Возвращаем false, если строка равна null, пустая или состоит из пробелов
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        // 2. Проверяем, начинается ли строка с http:// или https:// без учета регистра
        return input.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
               input.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
    }
}