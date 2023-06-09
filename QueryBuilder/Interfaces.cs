namespace QueryBuilder.Interfaces;

using QueryBuilder.Expressions;

public interface IQuery
{
    void Generate(IQueryGenerator builder);
}

public interface IQueryGenerator
{
    IQueryGenerator Append(object text);
    IQueryGenerator Append(Operator op);
    IQueryGenerator Append(JoinType type);
    IQueryGenerator Join(IEnumerable<IQuery> parts, ConditionType separator);
    IQueryGenerator Join(IEnumerable<IQuery> parts, string separator);
};

public interface IColumnSelector
{
    ColumnExpression this[string? table, string column] { get; }

    ColumnExpression this[string column]
        => this[null, column];
}

public interface ITableSelector
{
    TableSource this[string table] { get; }
}

public interface IConditionBuilder : IColumnSelector
{
}

public interface IWhereConditionBuilder : IConditionBuilder
{
    Condition Where { get; }
}

public interface IJoinConditionBuilder : IConditionBuilder
{

}

public static class QueryGeneratorExtensions
{
    public static IQueryGenerator Generate(this IQueryGenerator generator, IQuery queryable)
    {
        queryable.Generate(generator);
        return generator;
    }
}