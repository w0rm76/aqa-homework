using System.Collections.Generic;

namespace MiniAutomationToolkit.Core.Services;

public interface IDataFilter
{
    List<string> FilterAndSort(List<string> input);
}