namespace QueryBuilderTests;

using QueryBuilder;
using QueryBuilder.Generators;
using static QueryBuilder.Conditions;

public class QueryTests
{

    [Fact]
    public void Query_without_where_conditions()
    {
        string expected = "SELECT col FROM table";
        var builder = new Select("col")
            .From("table");

        Assert.Equal(expected, Generate(builder));
    }

    [Fact]
    public void Query_with_where_conditions()
    {
        string expected = "SELECT col FROM table WHERE (col1 = 5 OR NOT col2 IS NULL)";

        var builder = new Select("col")
            .From("table")
            .Where(q => q["col1"] == 5 || !(q["col2"].IsNull));

        Assert.Equal(expected, Generate(builder));
    }

    [Fact]
    public void Query_compose_where_conditions()
    {
        string expected = "SELECT col FROM table WHERE (col > 0 AND col < 10)";

        var builder = new Select("col")
            .From("table")
            .Where(q => q["col"] > 0)
            .Where(q => q.Where && q["col"] < 10);

        Assert.Equal(expected, Generate(builder));
    }

    [Fact]
    public void Query_compose_with_no_base_where()
    {
        string expected = "SELECT col FROM table WHERE col < 10";

        var builder = new Select("col")
            .From("table")
            .Where(q => q.Where && q["col"] < 10);

        Assert.Equal(expected, Generate(builder));
    }

    [Fact]
    public void Query_defaults_to_star_column()
    {
        string expected = "SELECT * FROM table";

        var builder = new Select()
            .From("table");

        Assert.Equal(expected, Generate(builder));
    }

    [Fact]
    public void Query_with_multiple_columns_and_aliases()
    {
        string expected = "SELECT c1, c2, c3, t1.c4 AS other_column, 12 AS number FROM table";

        var builder = new Select("c1", "c2")
            .Column("c3")
            .Column(c => c["t1", "c4"].As("other_column"))
            .Column(c => Literal(12).As("number"))
            .From("table");

        Assert.Equal(expected, Generate(builder));
    }

    private string Generate(Select builder)
    {
        var generator = new SimpleSqlGenerator();
        builder.Build().Generate(generator);
        return generator.ToString();
    }

}