namespace QueryBuilder.Generators;

using System.Text;
using global::QueryBuilder.Interfaces;

public class SimpleSqlGenerator : IQueryGenerator
{
    private readonly StringBuilder builder;

    public SimpleSqlGenerator()
    {
        builder = new StringBuilder();
    }

    public IQueryGenerator Append(object text)
    {
        builder.Append(text);
        return this;
    }

    public IQueryGenerator Join(IEnumerable<IQuery> parts, string separator)
    {
        foreach (var part in parts)
        {
            part.Generate(this);
            builder.Append(separator);
        }
        if (parts.Any())
            builder.Remove(builder.Length - separator.Length, separator.Length);
        return this;
    }

    public override string ToString() => builder.ToString();
}