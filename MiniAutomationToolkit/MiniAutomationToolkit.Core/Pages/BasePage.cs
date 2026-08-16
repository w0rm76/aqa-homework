using System;

namespace MiniAutomationToolkit.Core.Pages;

public abstract class BasePage
{
    public abstract string Url { get; }
    public abstract string PageName { get; }

    public virtual void Load()
    {
        Console.WriteLine($"Loading page: {PageName} at {Url}");
    }
}