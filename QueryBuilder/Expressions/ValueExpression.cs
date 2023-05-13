namespace QueryBuilder.Expressions;
using QueryBuilder.Interfaces;

public class ValueExpression<T> : Expression
{
    private readonly T _value;

    public ValueExpression(T value)
    {
        _value = value;
    }

    public override void Generate(IQueryGenerator builder) =>
        builder.Append(_value);
}