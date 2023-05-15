namespace QueryBuilder;

using QueryBuilder.Interfaces;

public class Column : IQuery
{
    private string _name;

    public Column(string name)
    {
        _name = name;
    }

    public void Generate(IQueryGenerator builder)
    {
        builder.Append(_name);
    }

    public static Column All { get; } = new Column("*");
}
