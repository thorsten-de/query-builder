using QueryBuilder.Interfaces;
using QueryBuilder.Builder;

namespace QueryBuilder;

public abstract record TableSource : IQuery
{
    public TableSource As(string alias) =>
        new TableAlias(this, alias);

    public TableSource Join(TableSource rhs, Func<IJoinConditionBuilder, Condition> builder) =>
        new Join(this, rhs, builder(new JoinConditionBuilder { JoinTable = rhs }));

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

public record Join(TableSource lhs, TableSource rhs, Condition On) : TableSource
{
    public override string Identifier => rhs.Identifier;

    public override void Generate(IQueryGenerator builder) =>
        builder
            .Generate(lhs)
            .Append(" JOIN ")
            .Generate(rhs)
            .Append(" ON ")
            .Generate(On);
}

