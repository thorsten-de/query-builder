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

    public abstract void Generate(IQueryGenerator visitor);

    #region Operator Overloads

    public static Condition operator ==(Expression lhs, Expression rhs) =>
      new BinaryOperator(Operator.Equal, lhs, rhs);
    public static Condition operator !=(Expression lhs, Expression rhs) =>
      new BinaryOperator(Operator.NotEqual, lhs, rhs);

    public static Condition operator ==(object lhs, Expression rhs) =>
      new BinaryOperator(Operator.Equal, new ValueExpression(lhs), rhs);

    public static Condition operator !=(object lhs, Expression rhs) =>
      new BinaryOperator(Operator.NotEqual, new ValueExpression(lhs), rhs);

    public static Condition operator ==(Expression lhs, object rhs) =>
      new BinaryOperator(Operator.Equal, lhs, new ValueExpression(rhs));

    public static Condition operator !=(Expression lhs, object rhs) =>
      new BinaryOperator(Operator.NotEqual, lhs, new ValueExpression(rhs));

    #endregion

}