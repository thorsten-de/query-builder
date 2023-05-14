using QueryBuilder.Interfaces;
using QueryBuilder.Generators;
using QueryBuilder.Expressions;

namespace QueryBuilder
{
    public enum ConditionType
    {
        None,
        And,
        Or,
        Not,
        Predicate
    }

    public abstract class Condition : IQuery
    {
        public ConditionType Type { get; protected set; } = ConditionType.Predicate;

        public static Condition operator &(Condition lhs, Condition? rhs) =>
            Conditions.And(lhs, rhs);

        public static Condition operator |(Condition lhs, Condition? rhs) =>
            Conditions.Or(lhs, rhs);

        public static Condition operator !(Condition condition) =>
            Conditions.Not(condition);

        // Here, we want to support || and && syntax, but build complete 
        // SQL conditions without shortcut. So we always return false.
        // Otherwise you could match on the following True and False literals.
        public static bool operator true(Condition _cond) => false;
        public static bool operator false(Condition _cond) => false;

        public static Condition True { get; } = Conditions.Predicate("TRUE");
        public static Condition False { get; } = Conditions.Predicate("FALSE");

        public static Condition None { get; } = new EmptyCondition();

        public static implicit operator Condition(bool value) => value ? True : False;


        public abstract void Generate(IQueryGenerator generator);

        public override string ToString()
        {
            var generator = new SimpleSqlGenerator();
            Generate(generator);
            return generator.ToString();
        }

        // Internal class representing condition literals, esp. TRUE and FALSE

    }

    public static class Conditions
    {
        public static Condition And(params Condition[] conditions) =>
          new ListCondition(ConditionType.And, conditions.Where(c => c != Condition.None));

        public static Condition Or(params Condition[] conditions) =>
          new ListCondition(ConditionType.Or, conditions.Where(c => c != Condition.None));

        public static Condition Not(Condition condition) =>
          new NotCondition(condition);

        public static Condition Predicate(string sqlFragment) =>
            new Predicate(sqlFragment);

        public static Expression Literal<T>(T literal) where T : notnull =>
            new ValueExpression<T>(literal);

        public static Expression Param<T>(T value, string name = null) =>
            new ValueExpression<T>(value);
    }

    public class Predicate : Condition
    {
        private string _literal;
        public Predicate(string Literal)
        {
            _literal = Literal;
        }

        public override void Generate(IQueryGenerator generator)
        {
            generator.Append(_literal);
        }

    }

    public class ListCondition : Condition
    {

        private readonly IEnumerable<Condition> _conditions;

        public ListCondition(ConditionType op, IEnumerable<Condition> conditions)
        {
            _conditions = conditions;
            Type = op;
        }

        public override void Generate(IQueryGenerator builder)
        {
            bool hasMultipleConditions = _conditions.Count() > 1;

            if (hasMultipleConditions)
                builder.Append("(");

            builder.Join(_conditions, Type);

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
            Type = ConditionType.Not;
        }

        public override void Generate(IQueryGenerator builder)
        {
            builder.Append("NOT ");
            Condition.Generate(builder);
        }
    }

    public class EmptyCondition : Condition
    {
        public EmptyCondition()
        {
            Type = ConditionType.None;
        }
        public override void Generate(IQueryGenerator generator)
        {
        }
    }
}