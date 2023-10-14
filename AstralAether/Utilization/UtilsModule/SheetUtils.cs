using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;
using AstralAether.Core.Handlers;
using AstralAether.Utilization.Attributes;
using AstralAether.Core.Singleton;

namespace AstralAether.Utilization.UtilsModule;

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
