namespace QueryBuilder.Expressions;

using global::QueryBuilder.Interfaces;

public class ColumnExpression : Expression
{
    private readonly string? _table;
    private string? _alias;
    private readonly string _column;

    public ColumnExpression(string column) : this(null, column) { }

    public ColumnExpression(string? table, string column)
    {
        _table = table;
        _column = column;
    }

    public ColumnExpression As(string alias)
    {
        _alias = alias;
        return this;
    }

    public override void Generate(IQueryGenerator builder)
    {
        if (!string.IsNullOrWhiteSpace(_table))
        {
            builder.Append(_table).Append(".");
        }
        builder.Append(_column);
        if (!string.IsNullOrEmpty(_alias))
        {
            builder.Append(" AS ").Append(_alias);
        }
    }
}