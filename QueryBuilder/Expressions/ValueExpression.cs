namespace QueryBuilder.Expressions;
using global::QueryBuilder.Interfaces;

public class ValueExpression : Expression
{
    private readonly object _value;

    public ValueExpression(object value)
    {
        _value = value;
    }

    public override void Generate(IQueryGenerator builder) =>
        builder.Append(_value);
}