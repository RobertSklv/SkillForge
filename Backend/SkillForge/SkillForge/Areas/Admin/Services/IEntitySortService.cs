using System.Reflection;

namespace SkillForge.Areas.Admin.Services;

public interface IEntitySortService
{
    List<PropertyInfo> GetOrderableProperties(Type type);

    IQueryable<T> OrderBy<T>(IQueryable<T> queryable, string propertyName, bool descending);
}