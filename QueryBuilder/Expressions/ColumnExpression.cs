namespace QueryBuilder.Expressions;

using QueryBuilder.Interfaces;

public class ColumnExpression : Expression
{
    private readonly string? _table;
    private readonly string _column;

    public ColumnExpression(string column) : this(null, column) { }

    public ColumnExpression(string? table, string column)
    {
        _table = table;
        _column = column;
    }

    public override void Generate(IQueryGenerator builder)
    {
        if (!string.IsNullOrWhiteSpace(_table))
        {
            builder.Append(_table).Append(".");
        }
        builder.Append(_column);
    }
}

public class Alias : Expression
{
    private readonly Expression _lhs;

    private readonly string _alias;

    public Alias(Expression lhs, string alias)
    {
        _lhs = lhs;
        _alias = alias;
    }

    public override void Generate(IQueryGenerator builder) =>
        builder
            .Generate(_lhs)
            .Append(" AS ")
            .Append(_alias);
}