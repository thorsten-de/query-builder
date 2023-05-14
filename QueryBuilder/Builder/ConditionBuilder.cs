using QueryBuilder.Expressions;
using QueryBuilder.Interfaces;

namespace QueryBuilder.Builder;

public class ColumnSelector : IColumnSelector
{
    public ColumnExpression this[string column, string? table = null] =>
        new ColumnExpression(column, table);
}

public class WhereConditionBuilder : ColumnSelector, IWhereConditionBuilder
{
    public Condition? Where { get; private set; }

    public WhereConditionBuilder(Condition? baseCondition)
    {
        Where = baseCondition;
    }
}