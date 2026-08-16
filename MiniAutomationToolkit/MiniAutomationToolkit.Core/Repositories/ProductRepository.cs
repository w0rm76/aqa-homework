using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MiniAutomationToolkit.Core.Models;

namespace MiniAutomationToolkit.Core.Repositories;

public static class ProductRepository
{
    public static List<Product> LoadFromCsv(string filePath)
    {
        var products = new List<Product>();

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Файл не найден: {filePath}");
        }

        string[] lines = File.ReadAllLines(filePath);
        int lineNumber = 0;

        foreach (string rawLine in lines)
        {
            lineNumber++; // Считаем каждую физическую строку, включая заголовок (строка 1)

            // Пропускаем заголовок (первая строка)
            if (lineNumber == 1)
            {
                continue;
            }

            // Пропускаем пустые строки
            if (string.IsNullOrWhiteSpace(rawLine))
            {
                continue;
            }

            // Разделяем строку по символу ';'
            string[] parts = rawLine.Split(';');

            // Должно быть строго три поля
            if (parts.Length != 3)
            {
                throw new InvalidDataException($"Ошибка в строке {lineNumber}: неверное количество полей.");
            }

            // Очищаем пробелы по краям
            string name = parts[0].Trim();
            string priceRaw = parts[1].Trim();
            string categoryRaw = parts[2].Trim();

            // Проверяем, что поля не пустые
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(priceRaw) || string.IsNullOrEmpty(categoryRaw))
            {
                throw new InvalidDataException($"Ошибка в строке {lineNumber}: обнаружены пустые поля.");
            }

            // Парсим цену и проверяем, чтобы она была неотрицательной
            if (!decimal.TryParse(priceRaw, out decimal price) || price < 0)
            {
                throw new InvalidDataException($"Ошибка в строке {lineNumber}: некорректная цена '{priceRaw}'.");
            }

            // Парсим категорию без учета регистра
            if (!Enum.TryParse(categoryRaw, true, out ProductCategory category))
            {
                throw new InvalidDataException($"Ошибка в строке {lineNumber}: неизвестная категория '{categoryRaw}'.");
            }

            products.Add(new Product(name, price, category));
        }

        return products;
    }

    public static List<string> GetAffordableProducts(
        IEnumerable<Product> products,
        ProductCategory category,
        decimal maxPrice)
    {
        // Строго одна цепочка LINQ без циклов foreach
        return products
            .Where(p => p.Category == category)                 // Выбираем указанную категорию
            .Where(p => p.Price < maxPrice)                     // Цена строго меньше maxPrice
            .OrderBy(p => p.Price)                             // Сортируем по цене
            .ThenBy(p => p.Name)                                // Затем по названию
            .Select(p => p.Name)                                // Выбираем только свойство Name
            .ToList();                                          // Возвращаем List<string>
    }
}
