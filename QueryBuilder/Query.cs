namespace QueryBuilder;
using QueryBuilder.Interfaces;
using QueryBuilder.Generators;

public class Query : IQuery
{
    private readonly static Column[] _allColumns = new[] { Column.All };

    private IList<Column> _columns = new List<Column>();

    public IEnumerable<Column> Columns => _columns.Any() ? _columns : _allColumns;

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

    public void AddColumn(Column col) =>
        _columns.Add(col);
}