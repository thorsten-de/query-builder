using QueryBuilder.Operators;
using QueryBuilder.Expressions;
using QueryBuilder.Interfaces;

namespace QueryBuilder;

public abstract class Expression : IQuery
{
    public Condition References(string table, string column = "ID") =>
        this == new ColumnExpression(column, table);

    public abstract void Generate(IQueryGenerator visitor);

    public static Condition operator ==(Expression lhs, Expression rhs) =>
      new BinaryOperator("=", lhs, rhs);
    public static Condition operator !=(Expression lhs, Expression rhs) =>
      new BinaryOperator("!=", lhs, rhs);

    public static Condition operator ==(object lhs, Expression rhs) =>
      new BinaryOperator("=", new ValueExpression(lhs), rhs);

    public static Condition operator !=(object lhs, Expression rhs) =>
      new BinaryOperator("!=", new ValueExpression(lhs), rhs);

    public static Condition operator ==(Expression lhs, object rhs) =>
      new BinaryOperator("=", lhs, new ValueExpression(rhs));

    public static Condition operator !=(Expression lhs, object rhs) =>
      new BinaryOperator("!=", lhs, new ValueExpression(rhs));

}