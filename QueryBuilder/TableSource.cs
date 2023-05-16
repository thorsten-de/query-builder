using QueryBuilder.Interfaces;
using QueryBuilder.Builder;

namespace QueryBuilder;

public enum JoinType
{
    Default,
    Inner,
    Left,
    Right,
    Full
}

public abstract record TableSource : IQuery
{
    public TableSource As(string? alias) =>
        alias != null
            ? new TableAlias(this, alias)
            : this;

    public TableSource Join(TableSource rhs, Func<IJoinConditionBuilder, Condition> on, JoinType type = JoinType.Default) =>
        Join(rhs, on(new JoinConditionBuilder { JoinTable = rhs }), type);

    public TableSource Join(TableSource rhs, Condition on, JoinType type = JoinType.Default) =>
        new Join(this, rhs, on, type);

    public abstract void Generate(IQueryGenerator builder);

    public abstract string Identifier { get; }
}

public class TableSelector : ITableSelector
{
    public TableSource this[string table] => new Table(table);
}

public record Table(string Name) : TableSource
{
    public override string Identifier => Name;

    public override void Generate(IQueryGenerator builder) =>
        builder.Append(Name);
}

public record TableAlias(TableSource Source, string Alias) : TableSource
{
    public override string Identifier => Alias;
    public override void Generate(IQueryGenerator builder) =>
        builder
            .Generate(Source)
            .Append(" ")
            .Append(Alias);
}

public record Join(TableSource lhs, TableSource rhs, Condition On, JoinType Type = JoinType.Default) : TableSource
{
    public override string Identifier => rhs.Identifier;

    public override void Generate(IQueryGenerator builder) =>
        builder
            .Generate(lhs)
            .Append(Type)
            .Append(" JOIN ")
            .Generate(rhs)
            .Append(" ON ")
            .Generate(On);
}

