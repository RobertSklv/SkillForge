﻿using Microsoft.EntityFrameworkCore.Infrastructure;
using SkillForge.Attributes;
using System.Linq.Expressions;
using System.Reflection;

namespace SkillForge.Areas.Admin.Services;

public class EntitySearchService : IEntitySearchService
{
    private readonly IEntityFilterService filterService;
    private readonly IEntityHelperService helper;

    public EntitySearchService(IEntityFilterService filterService, IEntityHelperService helper)
    {
        this.filterService = filterService;
        this.helper = helper;
    }

    public IQueryable<T> GenerateSearchFilters<T>(IQueryable<T> queryable, string? searchPhrase)
    {
        if (searchPhrase != null)
        {
            Type entityType = typeof(T);
            List<Expression> expressions = new();
            string[] tokens = searchPhrase.Split();
            ParameterExpression param = Expression.Parameter(entityType, "x");

            foreach (PropertyInfo propInfo in GetSearchableProperties(entityType))
            {
                string propertyName = propInfo.Name;

                SelectOptionAttribute? selectOptionAttr = propInfo.PropertyType.GetCustomAttribute<SelectOptionAttribute>();

                if (selectOptionAttr != null)
                {
                    if (helper.CanPropertyBeMapped(entityType, propertyName + '_' + selectOptionAttr.LabelProperty))
                    {
                        propertyName += '_' + selectOptionAttr.LabelProperty;
                    }
                    else if (entityType.GetProperty("Name") != null)
                    {
                        propertyName += '_' + "Name";
                    }
                }

                if (!helper.CanPropertyBeMapped(entityType, propertyName))
                {
                    continue;
                }

                try
                {
                    MemberExpression property = helper.ParseMemberExpression(param, propertyName);
                    Expression propertyAsString = property.Type == typeof(string)
                        ? property
                        : PropertyToString(property, propInfo.PropertyType);

                    foreach (string token in tokens)
                    {
                        Expression constant = Expression.Constant(token);
                        Expression exprBody = filterService.BuildFilterPredicate(propertyAsString, EntityFilterService.OPERATOR_CONTAINS, constant);
                        expressions.Add(exprBody);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"An error has occured while generating the search filter for property '{propInfo.Name}'.", e);
                }
            }

            queryable = JoinExpressions(queryable, expressions, param);
        }

        return queryable;
    }

    public MethodCallExpression PropertyToString(MemberExpression property, Type propertyType)
    {
        MethodCallExpression castExpression = Expression.Call(
            property,
            propertyType.GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance, Type.EmptyTypes)
                ?? throw new Exception($"Type '{propertyType.ShortDisplayName()}' has no 'ToString' method defined.")
        );

        return castExpression;
    }

    public IQueryable<T> JoinExpressions<T>(IQueryable<T> queryable, List<Expression> expressions, ParameterExpression param)
    {
        if (expressions.Count > 0)
        {
            Expression body = expressions.First();

            for (int i = 1; i < expressions.Count; i++)
            {
                Expression expr = expressions[i];

                body = Expression.OrElse(body, expr);
            }

            Expression<Func<T, bool>> combinedExpression = Expression.Lambda<Func<T, bool>>(body, param);
            queryable = queryable.Where(combinedExpression);
        }

        return queryable;
    }

    public List<PropertyInfo> GetSearchableProperties(Type type)
    {
        List<PropertyInfo> properties = new();

        foreach (PropertyInfo property in type.GetProperties())
        {
            TableColumnAttribute? colAttr = property.GetCustomAttribute<TableColumnAttribute>();

            if (colAttr == null || !colAttr.Searchable)
            {
                continue;
            }

            properties.Add(property);
        }

        return properties;
    }
}
