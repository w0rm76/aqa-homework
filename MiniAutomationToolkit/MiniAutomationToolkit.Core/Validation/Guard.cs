namespace MiniAutomationToolkit.Core.Validation;

public static class Guard
{
    // Метод принимает число и имя параметра с дефолтным значением "number"
    public static void EnsurePositive(int number, string parameterName = "number")
    {
        // Пропускаем положительные (> 0), для нуля и отрицательных выбрасываем ошибку
        if (number <= 0)
        {
            throw new ValidationException($"Validation failed: {parameterName} must be positive. Value: {number}");
        }
    }
}