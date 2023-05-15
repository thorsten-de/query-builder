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

    public Select Column(string name) => BuildColumn(null, name);
    public Select Column(string table, string name) => BuildColumn(table, name);
    public Select Column(Func<IColumnSelector, Expression> configure) =>
        BuildColumn(configure(new ColumnSelector()));

    public Select From(string table, string? @as = null) => this;

    public Select Join(string table, string @as, Func<IConditionBuilder, Condition> on) => this;
    public Select Join(string table, Func<IConditionBuilder, Condition> on) => this;
    public Select Where(Func<IWhereConditionBuilder, Condition> builder)
    {
        _query.Where = builder(new WhereConditionBuilder(_query.Where));
        return this;
    }

    public Query Build() => _query;


    private Select BuildColumn(string? table, string column) =>
        BuildColumn(new ColumnExpression(table, column));
    private Select BuildColumn(Expression column)
    {
        _query.AddColumn(column);
        return this;
    }
}