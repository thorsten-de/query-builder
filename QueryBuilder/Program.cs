using QueryBuilder.Builder;
using QueryBuilder.Expressions;
using QueryBuilder.Extensions;
using static QueryBuilder.Conditions;

Console.WriteLine("Hello, World!");
var aim = new TestColumnSelector();


var builder = new QueryBuilder.Builder.QueryBuilder();

var c = And(new ColumnExpression("col") == 12, aim["col"] == 13);

var condition =
      aim["bla"] == aim["test"] &
      aim["other"] != 3 |
      5 == aim["bla"]
      & aim["objectid"].References("Report");

builder
  .AddColumn("column1", col => col.As("colName"))
  .AddColumn("col2")

  .Join("table", on: aim =>
      aim["bla"] == aim["test"] & aim["other"] != 3 | 5 == aim["bla"] & !aim.IsActive())


  .Join("AppRoleInstance", on: ari => ari["instanceid"].References("ai") & ari.IsActive())

  .Where(t =>
    (t["col1"] == 12 | !(t["col4"] != "bla")) & t.IsActive());

Console.WriteLine(builder.WhereCondition);