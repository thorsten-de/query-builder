namespace QueryBuilderTests;

using QueryBuilder.Interfaces;
using QueryBuilder.Expressions;
using QueryBuilder.Builder;
using QueryBuilder.Generators;
using static QueryBuilder.Conditions;

public class ConditionTests
{
    private IColumnSelector t = new ColumnSelector();
    private IQueryGenerator generator = new SimpleSqlGenerator();

    [Theory]
    [InlineData("AttributeId")]
    [InlineData("attribute_id")]
    [InlineData("ATTRIBUTE_ID")]
    [InlineData("*")]
    public void Column_with_name_only(string name)
    {
        var e = t[name];
        var sql = Generate(e);
        Assert.Equal(name, sql);
    }

    [Theory]
    [InlineData("col", "table", "table.col")]
    [InlineData("*", "table", "table.*")]
    [InlineData("col", "", "col")]
    [InlineData("col", "  ", "col")]
    [InlineData("col", null, "col")]
    public void Column_with_name_and_table(string column, string? table, string expected)
    {
        var e = t[column, table];
        Assert.Equal(expected, Generate(e));
    }

    [Fact]
    public void Column_with_name_table_alias()
    {
        var e = t["col", "table"].As("colAlias");
        var sql = Generate(e);
        Assert.Equal("table.col AS colAlias", sql);
    }

    [Theory]
    [InlineData(true, "True")]
    [InlineData("Test", "Test")]
    [InlineData(42, "42")]
    public void ValueExpression_returns_itsef_as_string(object value, string expected)
    {
        var e = new ValueExpression(value);
        var sql = Generate(e);
        Assert.Equal(expected, sql);
    }

    [Fact]
    public void Equals_builds_sql_expression()
    {
        string expected = "col1 = col2";
        var e = t["col1"] == t["col2"];
        var sql = Generate(e);
        Assert.Equal(expected, sql);
    }

    [Fact]
    public void Inequality_builds_sql_expression()
    {
        string expected = "col1 != col2";
        var e = t["col1"] != t["col2"];
        var sql = Generate(e);
        Assert.Equal(expected, sql);
    }

    [Fact]
    public void Not_operator_composes_condition()
    {
        string expected = "NOT col = 5";
        var e = !(t["col"] == 5);

        var sql = Generate(e);
        Assert.Equal(expected, sql);

    }

    [Fact]
    public void And_operator_composes_two_conditions()
    {
        string expected = "(5 = t1.col AND 4 != t2.col)";
        var e = (5 == t["col", "t1"]) && (4 != t["col", "t2"]);

        var sql = Generate(e);
        Assert.Equal(expected, sql);
    }

    [Fact]
    public void Or_operator_composes_two_conditions()
    {
        string expected = "(col = 5 OR col != t2.col3)";
        var e = (t["col"] == 5) || (t["col"] != t["col3", "t2"]);

        var sql = Generate(e);
        Assert.Equal(expected, sql);
    }

    [Fact]
    public void And_operator_handles_many_conditions()
    {
        string expected = "(c1 = 5 AND c2 = 3 AND c3 = 3 AND c4 = 4)";

        var e = And(t["c1"] == 5, t["c2"] == 3, t["c3"] == 3, t["c4"] == 4);

        Assert.Equal(expected, Generate(e));
    }

    [Fact]
    public void Or_operator_composes_one_condition()
    {
        string expected = "c = 1";
        var e = Or(t["c"] == 1);

        var sql = Generate(e);
        Assert.Equal(expected, sql);
    }

    [Fact]
    public void And_operator_empties_without_condition()
    {
        string expected = "";
        var e = And();

        var sql = Generate(e);
        Assert.Equal(expected, sql);
    }

    [Fact]
    public void Compose_multiple_conditions()
    {
        string expected = "NOT (c = 5 OR (c = 1 AND a != 3))";

        var e = !(t["c"] == 5 || (t["c"] == 1 && t["a"] != 3));

        Assert.Equal(expected, Generate(e));
    }

    [Fact]
    public void DeMorgan_composite_test()
    {
        string expected = "(NOT a = b AND NOT c = d)";
        var a = t["a"];

        var e = !(a == t["b"]) && !(t["c"] == t["d"]);

        Assert.Equal(expected, Generate(e));
    }

    [Fact]
    public void OR_operators_do_not_shortcut()
    {
        string expected = "(TRUE OR a = b)";
        var e = true || t["a"] == t["b"];
        Assert.Equal(expected, Generate(e));
    }

    [Fact]
    public void AND_operators_do_not_shortcut()
    {
        string expected = "(FALSE AND a = b)";
        var e = false && t["a"] == t["b"];
        Assert.Equal(expected, Generate(e));
    }

    private string Generate(IQuery composite)
    {
        composite.Generate(generator);
        return generator.ToString();
    }
}