namespace QueryBuilder;
using QueryBuilder.Interfaces;
using QueryBuilder.Generators;

public class Query : IQuery
{
    public Condition Where { get; set; } = Condition.None;

    public void Generate(IQueryGenerator builder)
    {
        builder
            .Append("SELECT col")
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
}