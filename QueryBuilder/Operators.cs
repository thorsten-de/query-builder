using QueryBuilder.Interfaces;

namespace QueryBuilder.Operators;

public class BinaryOperator : Condition
{
    private readonly Expression _lhs;
    private readonly Expression _rhs;

    private readonly Operator _op;

    public BinaryOperator(Operator op, Expression lhs, Expression rhs)
    {
        _lhs = lhs;
        _rhs = rhs;
        _op = op;
    }

    public override void Generate(IQueryGenerator builder)
    {
        _lhs.Generate(builder);
        builder.Append(_op);
        _rhs.Generate(builder);
    }
}