using QueryBuilder.Interfaces;
using QueryBuilder.Generators;

namespace QueryBuilder
{

    public abstract class Condition : IQuery
    {
        public static Condition operator &(Condition lhs, Condition rhs) =>
            Conditions.And(lhs, rhs);

        public static Condition operator |(Condition lhs, Condition rhs) =>
            Conditions.Or(lhs, rhs);

        public static Condition operator !(Condition condition) =>
            Conditions.Not(condition);

        public abstract void Generate(IQueryGenerator visitor);

        public override string ToString()
        {
            var visitor = new SimpleSqlGenerator();
            Generate(visitor);
            return visitor.ToString();
        }
    }

    public static class Conditions
    {
        public static Condition And(params Condition[] conditions) =>
          new ListCondition(ConditionOperator.And, conditions);

        public static Condition Or(params Condition[] conditions) =>
          new ListCondition(ConditionOperator.Or, conditions);

        public static Condition Not(Condition condition) =>
          new NotCondition(condition);
    }

    public enum ConditionOperator
    {
        And,
        Or
    }

    public class ListCondition : Condition
    {
        private static readonly Dictionary<ConditionOperator, string> _opNames = new Dictionary<ConditionOperator, string>
        {
            [ConditionOperator.And] = " AND ",
            [ConditionOperator.Or] = " OR "
        };

        private readonly IReadOnlyList<Condition> _conditions;
        private readonly ConditionOperator _operator;

        public ListCondition(ConditionOperator op, params Condition[] conditions)
        {
            _conditions = conditions;
            _operator = op;
        }

        public override void Generate(IQueryGenerator builder)
        {
            bool hasMultipleConditions = _conditions.Count > 1;

            if (hasMultipleConditions)
                builder.Append("(");

            builder.Join(_conditions, _opNames[_operator]);

            if (hasMultipleConditions)
                builder.Append(")");
        }
    }

    public class NotCondition : Condition
    {
        public Condition Condition { get; }

        public NotCondition(Condition condition)
        {
            Condition = condition;
        }

        public override void Generate(IQueryGenerator builder)
        {
            builder.Append("NOT ");
            Condition.Generate(builder);
        }
    }
}