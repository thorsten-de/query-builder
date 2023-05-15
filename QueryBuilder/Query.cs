namespace QueryBuilder;
using QueryBuilder.Interfaces;
using QueryBuilder.Generators;
using QueryBuilder.Expressions;

public class Query : IQuery
{
    private readonly static Expression[] _allColumns = new[] { new ColumnExpression("*") };

    private IList<Expression> _columns = new List<Expression>();

    public IEnumerable<Expression> Columns => _columns.Any() ? _columns : _allColumns;

    public Condition Where { get; set; } = Condition.None;

    public void Generate(IQueryGenerator builder)
    {
        builder
            .Append("SELECT ")
            .Join(Columns, ", ")
            .Append(" FROM table");

        if (Where != Condition.None)
            builder.Append(" WHERE ").Append(Where);
    }

    public override string ToString()
    {
        var generator = new SimpleSqlGenerator();
        Generate(generator);
        return generator.ToString();
    }

    public void AddColumn(Expression col) =>
        _columns.Add(col);
}