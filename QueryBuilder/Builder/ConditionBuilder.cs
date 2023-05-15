using QueryBuilder.Expressions;
using QueryBuilder.Interfaces;

namespace QueryBuilder.Builder;

public class ColumnSelector : IColumnSelector
{
    public ColumnExpression this[string? table, string column] =>
        new ColumnExpression(table, column);
}

public class WhereConditionBuilder : ColumnSelector, IWhereConditionBuilder
{
    public Condition Where { get; private set; }

    public WhereConditionBuilder(Condition baseCondition)
    {
        Where = baseCondition;
    }
}