namespace QueryBuilder.Generators;

using System.Text;
using global::QueryBuilder.Interfaces;

public class SimpleSqlGenerator : IQueryGenerator
{
    private readonly static Dictionary<Operator, string> _operatorNames = new Dictionary<Operator, string>
    {
        [Operator.Equal] = " = ",
        [Operator.NotEqual] = " != ",
        [Operator.Greater] = " > ",
        [Operator.GreaterOrEqual] = " >= ",
        [Operator.Less] = " < ",
        [Operator.LessOrEqual] = " <= ",
        [Operator.In] = " IN ",
        [Operator.Like] = " LIKE ",
        [Operator.IsNull] = " IS NULL",
    };

    private static readonly Dictionary<ConditionType, string> _conditionTypes = new Dictionary<ConditionType, string>
    {
        [ConditionType.And] = " AND ",
        [ConditionType.Or] = " OR ",
        [ConditionType.Not] = "NOT ",
        [ConditionType.Predicate] = ""
    };

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

    public IQueryGenerator Append(Operator op)
    {
        builder.Append(_operatorNames[op]);
        return this;
    }

    public IQueryGenerator Join(IEnumerable<IQuery> parts, ConditionType type) =>
        Join(parts, separator: _conditionTypes[type]);

    public IQueryGenerator Join(IEnumerable<IQuery> parts, string separator)
    {
        foreach (var part in parts)
        {
            part.Generate(this);
            builder.Append(separator);
        }
        if (parts.Any())
        {
            builder.Remove(builder.Length - separator.Length, separator.Length);
        }
        return this;
    }

    public override string ToString() => builder.ToString();
}