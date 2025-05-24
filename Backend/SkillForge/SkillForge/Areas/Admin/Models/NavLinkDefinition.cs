using System.Reflection;
using SkillForge.Attributes;

namespace SkillForge.Areas.Admin.Models;

public class NavLinkDefinition
{
    public Type ControllerType { get; set; }

    public MethodInfo Method { get; set; }

    public AdminNavLinkAttribute Attribute { get; set; }
}