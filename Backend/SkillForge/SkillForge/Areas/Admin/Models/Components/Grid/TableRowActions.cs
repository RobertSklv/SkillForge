using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Models.Components.Grid;

public class TableRowActions
{
    public IBaseEntity Item { get; }

    public List<RowAction> Actions { get; set; } = new();

    public TableRowActions(IBaseEntity item, List<RowAction> actions)
    {
        Item = item;
        Actions = actions;
    }
}
