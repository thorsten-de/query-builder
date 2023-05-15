using QueryBuilder;
using QueryBuilder.Builder;
using QueryBuilder.Extensions;
using QueryBuilder.Interfaces;

var builder = new Select();

IColumnSelector t = new ColumnSelector();
var condition =
      t[table: "other", "col1"] == t["col2"] && t["col3"] != 3 ||
      5 == t["col2"] && t["post_id"].References("post");

Console.WriteLine(condition);

var query = builder
  .Column("column1", col => col.As("col_name"))
  .Column("col2")
  .Join("table", on: aim =>
      aim["bla"] == aim["test"] & aim["other"] != 3 | 5 == aim["bla"] & !aim.IsActive())
  .Join("comments", on: ari => ari["post_id"].References("post") & ari.IsActive())
  .Where(t =>
    t.Where || (t["col1"] == 12 | !(t["col4"] != "bla")) & t.IsActive())
  .Build();

Console.WriteLine(query);