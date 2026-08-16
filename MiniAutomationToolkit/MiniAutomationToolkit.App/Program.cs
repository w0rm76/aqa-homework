using System;
using MiniAutomationToolkit.Core.Models;
using MiniAutomationToolkit.Core.Services;
using MiniAutomationToolkit.Core.Pages;
using MiniAutomationToolkit.Core.Configuration;
using MiniAutomationToolkit.Core.Extensions;
using MiniAutomationToolkit.Core.Simulations;
using System.Diagnostics;



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


// ===================================================================
// --- Тестирование Задания 6: Умная конфигурация --------------------
// ===================================================================
Console.WriteLine("\n--- Тест Задания 6: Умная конфигурация ---");

// 1. Формируем базовый путь в папке bin/Debug/...
string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "appsettings.txt");

// 2. Резервный план для macOS/Rider: если файл не скопировался в bin, читаем его напрямую из проекта
if (!File.Exists(configPath))
{
    string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", ".."));
    configPath = Path.Combine(projectRoot, "MiniAutomationToolkit.App", "data", "appsettings.txt");
}

try
{
    // Инициализируем конфигурацию и читаем файл
    var config = new AppConfig(configPath);

    // Читаем параметры в строго заданных типах данных
    string baseUrl = config.GetSetting<string>("baseUrl");
    int timeout = config.GetSetting<int>("timeout");
    bool headless = config.GetSetting<bool>("headless");
    int retryCount = config.GetSetting<int>("retryCount");

    // Выводим результаты в консоль
    Console.WriteLine("Успешно прочитано из конфигурации:");
    Console.WriteLine($"- baseUrl (string): {baseUrl}");
    Console.WriteLine($"- timeout (int): {timeout}");
    Console.WriteLine($"- headless (bool): {headless}");
    Console.WriteLine($"- retryCount (int): {retryCount}");

    // Демонстрируем обработку отсутствующего ключа, как просит задание
    Console.WriteLine("\nПопытка получить несуществующий ключ 'missingKey'...");
    config.GetSetting<string>("missingKey");
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine($"Перехвачено ожидаемое исключение (Ключ не найден): {ex.Message}");
}
catch (InvalidDataException ex)
{
    Console.WriteLine($"Ошибка в формате данных конфигурации: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Непредвиденная системная ошибка: {ex.Message}");
}

// ===================================================================
// --- Тестирование Задания 7: Расширяем возможности строк -----------
// ===================================================================
Console.WriteLine("\n--- Тест Задания 7: Расширяем возможности строк ---");

// Список тестовых значений из условия задания
var testStrings = new string?[]
{
    "https://google.com",
    "http://example.org",
    "ftp://files.example.com",
    null,
    "HTTPS://SITE.EXAMPLE.COM"
};

foreach (var str in testStrings)
{
    // Вызываем метод как метод экземпляра строки 
    bool hasScheme = str.HasHttpScheme();
    
    // Выводим результат в консоль
    string displayStr = str ?? "<null>";
    Console.WriteLine($"'{displayStr}' → {hasScheme}");
}

// ===================================================================
// --- Тестирование Задания 8: Имитация длительной операции ----------
// ===================================================================
Console.WriteLine("\n--- Тест Задания 8: Имитация длительной операции ---");

var simulator = new LongOperationSimulator();
var stopwatch = new Stopwatch();

Console.WriteLine("Запуск асинхронной операции...");

stopwatch.Start();

// Вызываем асинхронный вариант через await (без использования .Result или .Wait())
string asyncResult = await simulator.LongOperationAsync();

stopwatch.Stop();

Console.WriteLine($"Результат выполнения: {asyncResult}");
Console.WriteLine($"Время выполнения асинхронной операции: {stopwatch.ElapsedMilliseconds} мс");
