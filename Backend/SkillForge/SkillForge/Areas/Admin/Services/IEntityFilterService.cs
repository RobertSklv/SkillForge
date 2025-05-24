using System.Linq.Expressions;
using System.Reflection;
using SkillForge.Areas.Admin.Models.Components.Grid;

namespace SkillForge.Areas.Admin.Services;

public interface IEntityFilterService
{
    object? ParseValue(Type type, string value);

    List<PropertyInfo> GetFilterableProperties(Type type);

    IQueryable<T> FilterBy<T>(IQueryable<T> queryable, Dictionary<string, TableFilter>? filters);

    Expression<Func<T, bool>> BuildFilterPredicate<T>(string propertyName, string @operator, dynamic value, dynamic? secondaryValue = null);

    Expression BuildFilterPredicate(Expression subject, string @operator, Expression constant, Expression? secondaryConstant = null);
}