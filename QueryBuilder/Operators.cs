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

public class Between : Condition
{
    private readonly Expression _lhs;
    private readonly Expression _from;
    private readonly Expression _to;

    public Between(Expression lhs, Expression from, Expression to)
    {
        _lhs = lhs;
        _from = from;
        _to = to;
    }

    public override void Generate(IQueryGenerator builder)
    {
        builder
            .Generate(_lhs)
            .Append(" BETWEEN ")
            .Generate(_from)
            .Append(" AND ")
            .Generate(_to);
    }
}

public class Like : Condition
{
    private readonly Expression _lhs;
    private readonly string _pattern;

    public Like(Expression lhs, string pattern)
    {
        _lhs = lhs;
        _pattern = pattern;
    }

    public override void Generate(IQueryGenerator builder)
    {
        builder
            .Generate(_lhs)
            .Append(" LIKE '")
            .Append(_pattern)
            .Append("'");
    }
}