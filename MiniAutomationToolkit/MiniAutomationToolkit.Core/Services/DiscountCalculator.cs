using System;
using MiniAutomationToolkit.Core.Models;

namespace MiniAutomationToolkit.Core.Services;

public static class DiscountCalculator
{
    public static decimal CalculateDiscount(decimal orderAmount, ClientType clientType)
    {
        if (orderAmount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(orderAmount), "Сумма заказа не может быть отрицательной.");
        }

        return clientType switch
        {
            ClientType.Vip => orderAmount * 0.15m,
            ClientType.Premium when orderAmount > 1000 => orderAmount * 0.10m,
            ClientType.Premium => orderAmount * 0.05m,
            ClientType.Regular when orderAmount > 1000 => orderAmount * 0.05m,
            ClientType.Regular => 0m,
            _ => throw new ArgumentException("Неизвестный тип клиента.", nameof(clientType))
        };
    }
}