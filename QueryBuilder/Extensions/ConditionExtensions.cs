using QueryBuilder.Interfaces;

namespace QueryBuilder.Extensions;

public static class ConditionExtensions
{
    public static Condition IsActive(this IConditionBuilder builder) =>
        builder["DataState"] == 2;
}
