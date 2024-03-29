namespace FakeItEasy.IntegrationTests
{
    using System;

    public static class StringArgumentConstraints
    {
        public static string StartsWith(this IArgumentConstraintManager<string> scope, string beginning)
        {
            return scope.Matches(x => x.StartsWith(beginning, StringComparison.CurrentCulture), $@"Starts with ""{beginning}""");
        }

        public static string Contains(this IArgumentConstraintManager<string> scope, string value)
        {
#if LACKS_STRING_CONTAINS_COMPARISONTYPE
            return scope.Matches(x => x.Contains(value), $@"Contains ""{value}""");
#else
            return scope.Matches(x => x.Contains(value, StringComparison.CurrentCulture), $@"Contains ""{value}""");
#endif
        }
    }
}
