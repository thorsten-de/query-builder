using QueryBuilder.Builder;
using QueryBuilder.Extensions;

var builder = new QueryBuilder.Builder.QueryBuilder();

var t = new ColumnSelector();
var condition =
      t["col1", table: "other"] == t["col2"] && t["col3"] != 3 ||
      5 == t["col2"] && t["post_id"].References("post");

Console.WriteLine(condition);

builder
  .AddColumn("column1", col => col.As("col_name"))
  .AddColumn("col2")
  .Join("table", on: aim =>
      aim["bla"] == aim["test"] & aim["other"] != 3 | 5 == aim["bla"] & !aim.IsActive())
  .Join("comments", on: ari => ari["post_id"].References("post") & ari.IsActive())
  .Where(t =>
    (t["col1"] == 12 | !(t["col4"] != "bla")) & t.IsActive());

Console.WriteLine(builder.WhereCondition);