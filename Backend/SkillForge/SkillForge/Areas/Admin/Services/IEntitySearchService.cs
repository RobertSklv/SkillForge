using System.Linq.Expressions;
using System.Reflection;

namespace SkillForge.Areas.Admin.Services;

public interface IEntitySearchService
{
    IQueryable<T> GenerateSearchFilters<T>(IQueryable<T> queryable, string? searchPhrase);

    IQueryable<T> JoinExpressions<T>(IQueryable<T> queryable, List<Expression> expressions, ParameterExpression param);

    MethodCallExpression PropertyToString(MemberExpression property, Type propertyType);

    List<PropertyInfo> GetSearchableProperties(Type type);
}