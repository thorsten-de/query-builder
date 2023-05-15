using QueryBuilder.Interfaces;
using QueryBuilder.Builder;
using QueryBuilder.Expressions;
namespace QueryBuilder;

public class Select
{

    private Query _query;

    public Select(params string[] columnNames)
    {
        _query = new Query();
        foreach (var col in columnNames)
            Column(col);
    }

    public Select Column(string name) =>
        Column(c => c[name]);
    public Select Column(string table, string name) =>
        Column(c => c[table, name]);

    public Select Column(Func<IColumnSelector, Expression> configure) =>
        BuildColumn(configure(new ColumnSelector()));

    public Select From(string table) =>
        From(t => t[table]);

    public Select From(string table, string @as) =>
        From(t => t[table].As(@as));

    public Select From(Func<ITableSelector, TableSource> builder) =>
        BuildFrom(builder(new TableSelector()));


    public Select Join(string table, Func<IConditionBuilder, Condition> on) =>
        Join(new Table(table), on);

    public Select Join(string table, string @as, Func<IConditionBuilder, Condition> on) =>
        Join(new Table(table).As(@as), on);

    public Select Join(TableSource rhs, Func<IConditionBuilder, Condition> on)
    {
        _query.From[^1] = _query.From[^1].Join(rhs, on);
        return this;
    }
    public Select Where(Func<IWhereConditionBuilder, Condition> builder)
    {
        _query.Where = builder(new WhereConditionBuilder(_query.Where));
        return this;
    }

    public Query Build() => _query;

    private Select BuildColumn(Expression column)
    {
        _query.AddColumn(column);
        return this;
    }

    private Select BuildFrom(TableSource source)
    {
        _query.From.Add(source);
        return this;
    }

}