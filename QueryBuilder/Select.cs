using QueryBuilder.Interfaces;
using QueryBuilder.Builder;

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

    public Select Column(string name) => BuildColumn(name);
    public Select Column(string columnName, Func<IColumnBuilder, IColumnBuilder> configure) => this;

    public Select From(string table, string? @as = null) => this;

    public Select Join(string table, string @as, Func<IConditionBuilder, Condition> on) => this;
    public Select Join(string table, Func<IConditionBuilder, Condition> on) => this;
    public Select Where(Func<IWhereConditionBuilder, Condition> builder)
    {
        _query.Where = builder(new WhereConditionBuilder(_query.Where));
        return this;
    }

    public Query Build() => _query;


    private Select BuildColumn(string name)
    {
        _query.AddColumn(new Column(name));
        return this;
    }
}