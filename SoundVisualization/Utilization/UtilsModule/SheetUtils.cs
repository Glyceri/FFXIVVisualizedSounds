using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;
using SoundVisualization.Core.Handlers;
using SoundVisualization.Utilization.Attributes;
using SoundVisualization.Core.Singleton;
using Dalamud.Logging;
using System.IO;

namespace SoundVisualization.Utilization.UtilsModule;

[UtilsDeclarable]
internal class SheetUtils : UtilsRegistryType, ISingletonBase<SheetUtils>
{
    ExcelSheet<Action> actions { get; set; } = null!;
    public static SheetUtils instance { get; set; } = null!;

    internal override void OnRegistered()
    {
        actions = PluginHandlers.DataManager.GetExcelSheet<Action>()!;           
    }

    public Action? GetAction(uint actionID) => actions?.GetRow(actionID)!;
}
