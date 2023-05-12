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

public class IsNull : Condition
{
    private readonly Expression _expr;

    public IsNull(Expression expr)
    {
        _expr = expr;
    }

    public override void Generate(IQueryGenerator builder)
    {
        _expr.Generate(builder);
        builder.Append(Operator.IsNull);
    }
}

public class In : Condition
{
    private readonly Expression _lhs;
    private readonly IEnumerable<Expression> _expressions;

    public In(Expression lhs, IEnumerable<Expression> expressions)
    {
        _lhs = lhs;
        _expressions = expressions;
    }

    public override void Generate(IQueryGenerator builder)
    {
        _lhs.Generate(builder);
        builder.Append(Operator.In).Append("(").Join(_expressions, ",").Append(")");
    }
}