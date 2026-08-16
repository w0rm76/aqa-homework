using System;
using MiniAutomationToolkit.Core.Models;
using MiniAutomationToolkit.Core.Services;
using MiniAutomationToolkit.Core.Pages;

Console.WriteLine("MiniAutomationToolkit started");
Console.WriteLine();

// Тест-кейсы по условию задания
var testCases = new (ClientType Type, decimal Amount)[]
{
    (ClientType.Vip, 500m),
    (ClientType.Vip, 2000m),
    (ClientType.Premium, 800m),
    (ClientType.Premium, 1000m),
    (ClientType.Premium, 1500m),
    (ClientType.Regular, 500m),
    (ClientType.Regular, 1500m),
    (ClientType.Regular, 1000m)
};

foreach (var test in testCases)
{
    decimal discount = DiscountCalculator.CalculateDiscount(test.Amount, test.Type);
    Console.WriteLine($"Client: {test.Type}, amount: {test.Amount}, discount: {discount}");
}

// Тест на проверку выброса ошибки при отрицательной сумме
try
{
    DiscountCalculator.CalculateDiscount(-100m, ClientType.Regular);
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"\nПерехвачено ожидаемое исключение: {ex.Message}");
}

// --- Тестирование Задания 3 ---
Console.WriteLine("\n--- Тест Задания 3: Поиск в хаосе ---");

var chaoticList = new List<string>
{
    "  Apple ", 
    "banana", 
    "  ", 
    null, 
    "Cat", 
    "box", 
    "Dog", 
    "  Elephant"
};

IDataFilter filter = new DataFilter();
List<string> resultList = filter.FilterAndSort(chaoticList);

Console.WriteLine("Результат фильтрации и сортировки:");
foreach (var item in resultList)
{
    Console.WriteLine($"'{item}'");
}

// --- Тестирование Задания 4: Неизменяемый пользователь ---
Console.WriteLine("\n--- Тест Задания 4: Неизменяемый пользователь ---");

// 1. Успешное создание пользователя Alex Smith
try
{
    var user1 = new UserDto("Alex Smith", "alex@example.com");
    Console.WriteLine($"Успешно создан: {user1}");
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка при валидном создании: {ex.Message}");
}

// 2. Равенство двух объектов с одинаковыми значениями
var userA = new UserDto("Alex Smith", "alex@example.com");
var userB = new UserDto("Alex Smith", "alex@example.com");
Console.WriteLine($"userA == userB: {userA == userB} (Ожидается: True)");
Console.WriteLine($"userA.Equals(userB): {userA.Equals(userB)} (Ожидается: True)");

// 3. Демонстрация невозможности изменить свойства (закомментировано, так как вызывает ошибку компиляции)
// userA.Name = "New Name"; // Ошибка Rider: Property or indexer 'UserDto.Name' cannot be assigned to -- it is read only

// 4. Демонстрация ошибочных сценариев
Console.WriteLine("\n--- Демонстрация ошибочных сценариев ---");

// Сценарий А: пустое имя и корректный email
try
{
    var badUser = new UserDto("", "alex@example.com");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Перехвачено исключение (пустое имя): {ex.Message}");
}

// Сценарий Б: корректное имя и пустой email
try
{
    var badUser = new UserDto("Alex Smith", "   ");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Перехвачено исключение (пустой email): {ex.Message}");
}

// Сценарий В: корректное имя и email без символа @
try
{
    var badUser = new UserDto("Alex Smith", "alex_example.com");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Перехвачено исключение (нет @): {ex.Message}");
}

// Сценарий Г: корректное имя и email с пробелом
try
{
    var badUser = new UserDto("Alex Smith", "alex @example.com");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Перехвачено исключение (есть пробел): {ex.Message}");
}

// --- Тестирование Задания 5: Базовая страница ---
Console.WriteLine("\n--- Тест Задания 5: Базовая страница ---");

// 1. Создаем список страниц
var pages = new List<BasePage>
{
    new LoginPage(),
    new HomePage()
};

// 2. Вызываем метод Load для каждой страницы
Console.WriteLine("Загрузка страниц:");
foreach (var page in pages)
{
    page.Load();
}

// 3. Проверяем уникальность URL с помощью LINQ
try
{
    int totalCount = pages.Count;
    // Select вытаскивает все URL, Distinct убирает дубликаты, Count считает уникальные
    int uniqueCount = pages.Select(p => p.Url).Distinct().Count();

    if (uniqueCount < totalCount)
    {
        throw new InvalidOperationException("Обнаружены дубликаты URL среди страниц!");
    }

    Console.WriteLine("All page URLs are unique.");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}
