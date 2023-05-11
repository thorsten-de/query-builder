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

        // Here, we want to support || and && syntax, but build complete 
        // SQL conditions without shortcut. So we always return false.
        // Otherwise you could match on the following True and False literals.
        public static bool operator true(Condition _cond) => false;
        public static bool operator false(Condition _cond) => false;

        public static Condition True { get; } = new Literal("TRUE");
        public static Condition False { get; } = new Literal("FALSE");

        public static implicit operator Condition(bool value) => value ? True : False;


        public abstract void Generate(IQueryGenerator generator);

        public override string ToString()
        {
            var generator = new SimpleSqlGenerator();
            Generate(generator);
            return generator.ToString();
        }

        // Internal class representing condition literals, esp. TRUE and FALSE
        private class Literal : Condition
        {
            private string _literal;
            public Literal(string Literal)
            {
                _literal = Literal;
            }

            public override void Generate(IQueryGenerator generator)
            {
                generator.Append(_literal);
            }

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