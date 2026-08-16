using System;
using MiniAutomationToolkit.Core.Models;
using MiniAutomationToolkit.Core.Services;

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