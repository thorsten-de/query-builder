using QueryBuilder.Operators;
using QueryBuilder.Expressions;
using QueryBuilder.Interfaces;

namespace QueryBuilder;

public enum Operator
{
    Equal,
    NotEqual,
    Less,
    LessOrEqual,
    Greater,
    GreaterOrEqual,
    Like,
    Between,
    IsNull,
    In

}

public abstract class Expression : IQuery
{
    public Condition References(string table, string column = "ID") =>
        this == new ColumnExpression(column, table);

    public Condition IsNull =>
      new Operators.IsNull(this);

    public Condition In(params Expression[] expressions) =>
      new Operators.In(this, expressions);

    public Condition In<T>(IEnumerable<T> values) =>
        new Operators.In(this, values.Select(v => new ValueExpression<T>(v)));

    public Condition Between(Expression from, Expression to) =>
        new Operators.Between(this, from, to);

    public Condition Like(string pattern) =>
        new Operators.Like(this, pattern);

    public abstract void Generate(IQueryGenerator visitor);

    #region Operator Overloads

    public static implicit operator Expression(string value) => new ValueExpression<string>(value);
    public static implicit operator Expression(int value) => new ValueExpression<int>(value);
    public static implicit operator Expression(decimal value) => new ValueExpression<decimal>(value);
    public static implicit operator Expression(float value) => new ValueExpression<float>(value);
    public static implicit operator Expression(double value) => new ValueExpression<double>(value);
    public static implicit operator Expression(DateTime value) => new ValueExpression<string>(value.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
    public static implicit operator Expression(DateOnly value) => new ValueExpression<string>(value.ToString("O", System.Globalization.CultureInfo.InvariantCulture));


    public static Condition operator ==(Expression lhs, Expression rhs) =>
        new BinaryOperator(Operator.Equal, lhs, rhs);

    public static Condition operator !=(Expression lhs, Expression rhs) =>
        new BinaryOperator(Operator.NotEqual, lhs, rhs);

    public static Condition operator <(Expression lhs, Expression rhs) =>
        new BinaryOperator(Operator.Less, lhs, rhs);

    public static Condition operator >(Expression lhs, Expression rhs) =>
        new BinaryOperator(Operator.Greater, lhs, rhs);

    public static Condition operator <=(Expression lhs, Expression rhs) =>
        new BinaryOperator(Operator.LessOrEqual, lhs, rhs);

    public static Condition operator >=(Expression lhs, Expression rhs) =>
        new BinaryOperator(Operator.GreaterOrEqual, lhs, rhs);

    #endregion
}