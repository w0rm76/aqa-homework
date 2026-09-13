using System.Threading;
using System.Threading.Tasks;

namespace MiniAutomationToolkit.Core.Simulations;

public class LongOperationSimulator
{
    // Синхронный вариант (блокирует поток)
    public string LongOperation()
    {
        Thread.Sleep(2000); // Блокировка на 2 секунды
        return "Done";
    }

    // Асинхронный вариант (освобождает поток во время ожидания)
    public async Task<string> LongOperationAsync()
    {
        await Task.Delay(2000); // Асинхронное ожидание на 2 секунды
        return "Done";
    }
}