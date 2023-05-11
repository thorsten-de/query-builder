using QueryBuilder.Interfaces;

namespace QueryBuilder.Builder;

public class QueryBuilder
{

    public Condition WhereCondition { get; private set; }

    public QueryBuilder AddColumn(string name) => this;
    public QueryBuilder AddColumn(string columnName, Func<IColumnBuilder, IColumnBuilder> configure) => this;
    public QueryBuilder Join(string table, string @as, Func<IConditionBuilder, Condition> on) => this;
    public QueryBuilder Join(string table, Func<IConditionBuilder, Condition> on) => this;
    public QueryBuilder Where(Func<IConditionBuilder, Condition> builder)
    {

        WhereCondition = builder(new WhereConditionBuilder());
        return this;
    }
}