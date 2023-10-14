using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;
using AstralAether.Core.Handlers;
using AstralAether.Utilization.Attributes;
using AstralAether.Core.Singleton;
using Dalamud.Logging;

namespace AstralAether.Utilization.UtilsModule;

[UtilsDeclarable]
internal class SheetUtils : UtilsRegistryType, ISingletonBase<SheetUtils>
{
    ExcelSheet<Action> actions { get; set; } = null!;
    ExcelSheet<Race> race { get; set; } = null!;
    ExcelSheet<Tribe> tribe { get; set; } = null!;
    public static SheetUtils instance { get; set; } = null!;

    internal override void OnRegistered()
    {
        actions = PluginHandlers.DataManager.GetExcelSheet<Action>()!;
        race = PluginHandlers.DataManager.GetExcelSheet<Race>()!;
        tribe = PluginHandlers.DataManager.GetExcelSheet<Tribe>()!;

        foreach (Race r in race)
        {
            PluginLog.Log(r.Feminine + " : " + r.RowId);
        }

        foreach (Tribe t in tribe)
        {
            PluginLog.Log(t.Feminine + " : " + t.RowId);
        }
    }

    public Action? GetAction(uint actionID) => actions?.GetRow(actionID)!;
    public Tribe? GetTribe(uint t) => tribe?.GetRow(t);
    public Race? GetRace(uint r) => race?.GetRow(r);
}
