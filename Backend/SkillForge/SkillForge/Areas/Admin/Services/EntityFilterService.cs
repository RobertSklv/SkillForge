﻿using SkillForge.Areas.Admin.Models.Components.Grid;
using SkillForge.Attributes;
using SkillForge.Models.Database;
using System.Linq.Expressions;
using System.Reflection;

namespace SkillForge.Areas.Admin.Services;

public class EntityFilterService : IEntityFilterService
{
    public const string OPERATOR_EQUAL = "eq";
    public const string OPERATOR_NOT_EQUAL = "neq";
    public const string OPERATOR_CONTAINS = "con";
    public const string OPERATOR_STARTS_WITH = "swi";
    public const string OPERATOR_ENDS_WITH = "ewi";
    public const string OPERATOR_LESS_THAN = "lt";
    public const string OPERATOR_LESS_THAN_OR_EQUAL = "lte";
    public const string OPERATOR_GREATER_THAN = "gt";
    public const string OPERATOR_GREATER_THAN_OR_EQUAL = "gte";
    public const string BETWEEN = "btw";

    public const string OPERATOR_BEFORE = "bf";
    public const string OPERATOR_AFTER = "af";

    public static readonly Dictionary<string, string> OperatorLabelMap = new()
    {
        { OPERATOR_EQUAL, "Equal to" },
        { OPERATOR_NOT_EQUAL, "Not equal to" },
        { OPERATOR_CONTAINS, "Contains" },
        { OPERATOR_STARTS_WITH, "Starts with" },
        { OPERATOR_ENDS_WITH, "Ends with" },
        { OPERATOR_LESS_THAN, "Less than" },
        { OPERATOR_LESS_THAN_OR_EQUAL, "Less than or equal" },
        { OPERATOR_GREATER_THAN, "Greater than" },
        { OPERATOR_GREATER_THAN_OR_EQUAL, "Greater than or equal" },
        { OPERATOR_BEFORE, "Before" },
        { OPERATOR_AFTER, "After" },
        { BETWEEN, "Between" },
    };

    private readonly IEntityHelperService helper;

    public EntityFilterService(IEntityHelperService helper)
    {
        this.helper = helper;
    }

    public object? ParseValue(Type type, string value)
    {
        if (string.IsNullOrEmpty(value)) return null;
        else if (type.Equals(typeof(long))) return long.Parse(value);
        else if (type.Equals(typeof(ulong))) return ulong.Parse(value);
        else if (type.Equals(typeof(int))) return int.Parse(value);
        else if (type.Equals(typeof(uint))) return uint.Parse(value);
        else if (type.Equals(typeof(short))) return short.Parse(value);
        else if (type.Equals(typeof(ushort))) return ushort.Parse(value);
        else if (type.Equals(typeof(sbyte))) return sbyte.Parse(value);
        else if (type.Equals(typeof(byte))) return byte.Parse(value);
        else if (type.Equals(typeof(double))) return double.Parse(value);
        else if (type.Equals(typeof(float))) return float.Parse(value);
        else if (type.Equals(typeof(decimal))) return decimal.Parse(value);
        else if (type.Equals(typeof(DateTime))) return DateTime.Parse(value);
        else if (type.Equals(typeof(bool))) return bool.Parse(value);
        else if (type.IsSubclassOf(typeof(BaseEntity))) return int.TryParse(value, out int modelId) && modelId > 0 ? modelId : null;
        else return value;
    }

    public List<PropertyInfo> GetFilterableProperties(Type type)
    {
        List<PropertyInfo> properties = new();

        foreach (PropertyInfo property in type.GetProperties())
        {
            TableColumnAttribute? colAttr = property.GetCustomAttribute<TableColumnAttribute>();

            if (colAttr == null || !colAttr.Filterable)
            {
                continue;
            }

            properties.Add(property);
        }

        return properties;
    }

    public IQueryable<T> FilterBy<T>(IQueryable<T> queryable, Dictionary<string, TableFilter>? filters)
    {
        if (filters != null)
        {
            foreach (PropertyInfo prop in GetFilterableProperties(typeof(T)))
            {
                string propertyName = prop.Name;

                if (!filters.ContainsKey(propertyName))
                {
                    continue;
                }

                TableFilter filter = filters[propertyName];

                object? parsedValue = ParseValue(prop.PropertyType, filter.Value);
                object? parsedSecondaryValue = ParseValue(prop.PropertyType, filter.SecondaryValue);

                if (parsedValue == null)
                {
                    continue;
                }

                SelectOptionAttribute? selectOptionAttr = prop.PropertyType.GetCustomAttribute<SelectOptionAttribute>();

                if (selectOptionAttr != null)
                {
                    propertyName += '_' + selectOptionAttr.IdentityProperty;
                }

                try
                {
                    Expression<Func<T, bool>> expr = BuildFilterPredicate<T>(propertyName, filter.Operator, parsedValue, parsedSecondaryValue);

                    queryable = queryable.Where(expr);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        return queryable;
    }

    public Expression<Func<T, bool>> BuildFilterPredicate<T>(string propertyName, string @operator, dynamic value, dynamic? secondaryValue = null)
    {
        ParameterExpression param = Expression.Parameter(typeof(T), "x");
        MemberExpression property = helper.ParseMemberExpression(param, propertyName);
        Expression constant = Expression.Constant(value);
        Expression? secondaryConstant = null;

        if (secondaryValue != null)
        {
            secondaryConstant = Expression.Constant(secondaryValue);
        }

        Expression body = BuildFilterPredicate(property, @operator, constant, secondaryConstant);

        return Expression.Lambda<Func<T, bool>>(body, param);
    }

    public Expression BuildFilterPredicate(Expression subject, string @operator, Expression constant, Expression? secondaryConstant = null)
    {
        Expression body;

        switch (@operator)
        {
            case OPERATOR_EQUAL:
                body = Expression.Equal(subject, constant);
                break;
            case OPERATOR_NOT_EQUAL:
                body = Expression.NotEqual(subject, constant);
                break;
            case OPERATOR_CONTAINS:
                body = Expression.Call(subject, "Contains", null, constant);
                break;
            case OPERATOR_STARTS_WITH:
                body = Expression.Call(subject, "StartsWith", null, constant);
                break;
            case OPERATOR_ENDS_WITH:
                body = Expression.Call(subject, "EndsWith", null, constant);
                break;
            case OPERATOR_LESS_THAN:
            case OPERATOR_BEFORE:
                body = Expression.LessThan(subject, constant);
                break;
            case OPERATOR_LESS_THAN_OR_EQUAL:
                body = Expression.LessThanOrEqual(subject, constant);
                break;
            case OPERATOR_GREATER_THAN:
            case OPERATOR_AFTER:
                body = Expression.GreaterThan(subject, constant);
                break;
            case OPERATOR_GREATER_THAN_OR_EQUAL:
                body = Expression.GreaterThanOrEqual(subject, constant);
                break;
            case BETWEEN:
                Expression greaterThanOrEqual = Expression.GreaterThanOrEqual(subject, constant);

                if (secondaryConstant == null)
                {
                    body = greaterThanOrEqual;
                    break;
                }

                Expression lessThanOrEqual = Expression.LessThanOrEqual(subject, secondaryConstant);
                body = Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);
                break;
            default: throw new InvalidOperationException($"Operator '{@operator}' is not supported!");
        }

        return body;
    }
}
