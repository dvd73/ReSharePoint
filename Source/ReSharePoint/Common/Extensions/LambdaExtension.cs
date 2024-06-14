using System;

namespace ReSharePoint.Common.Extensions
{
    public static class LambdaExtension
    {
        public static Func<T, bool> AndAlso<T>(
            this Func<T, bool> predicate1,
            Func<T, bool> predicate2)
        {
            return arg => predicate1(arg) && predicate2(arg);
        }

        public static Func<T, bool> OrElse<T>(
            this Func<T, bool> predicate1,
            Func<T, bool> predicate2)
        {
            return arg => predicate1(arg) || predicate2(arg);
        }
    }
}
