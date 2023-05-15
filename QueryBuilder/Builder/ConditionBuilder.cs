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

public class JoinConditionBuilder : IJoinConditionBuilder
{
    public required TableSource JoinTable { get; init; }

    public ColumnExpression this[string? table, string column] =>
        new ColumnExpression(table ?? JoinTable.Identifier, column);
}