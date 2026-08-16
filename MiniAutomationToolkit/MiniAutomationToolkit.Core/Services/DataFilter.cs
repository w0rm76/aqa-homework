using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniAutomationToolkit.Core.Services;

public class DataFilter : IDataFilter
{
    public List<string> FilterAndSort(List<string> input)
    {
        if (input == null)
        {
            return new List<string>();
        }

        return input
            // 1. Убираем null элементы
            .Where(str => str != null)
            // 2. Очищаем пробелы в начале и конце строк
            .Select(str => str.Trim())
            // 3. Отфильтровываем пустые строки
            .Where(str => !string.IsNullOrEmpty(str))
            // 4. Оставляем только те, что начинаются с заглавной латинской буквы (A-Z)
            .Where(str => char.IsUpper(str[0]) && str[0] >= 'A' && str[0] <= 'Z')
            // 5. Сортируем по алфавиту
            .OrderBy(str => str)
            // Превращаем результат обратно в List<string>
            .ToList();
    }
}