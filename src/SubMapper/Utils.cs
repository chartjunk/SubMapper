using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SubMapper
{
    public static partial class Utils
    {
        public static PropertyInfo GetPropertyInfo<TI, TValue>(
            this Expression<Func<TI, TValue>> getIExpr)
        {
            var memberExpression = getIExpr.Body as MemberExpression;

            // Is of type i => i.Property
            if (memberExpression != null)
                return memberExpression.Member as PropertyInfo;

            // Is of type i => i
            else return null;
        }
    }
}
