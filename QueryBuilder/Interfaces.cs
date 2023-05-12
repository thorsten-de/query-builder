namespace QueryBuilder.Interfaces;

using QueryBuilder.Expressions;

public interface IQuery
{
    void Generate(IQueryGenerator generator);
}

public interface IQueryGenerator
{
    IQueryGenerator Append(object text);
    IQueryGenerator Append(Operator op);
    IQueryGenerator Join(IEnumerable<IQuery> parts, ConditionType separator);
};

public interface IColumnSelector
{
    ColumnExpression this[string column, string? table = null] { get; }
}

public interface IConditionBuilder : IColumnSelector
{
}