using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SubMapper
{
    public static partial class Utils
    {
        private static PropertyInfo GetPropertyInfo<TI, TValue>(
            this Expression<Func<TI, TValue>> getIExpr)
        {
            var memberExpression = getIExpr.Body as MemberExpression;

            // Is of type i => i.Property
            if (memberExpression != null)
                return memberExpression.Member as PropertyInfo;

            // Is of type i => i
            else return null;
        }

        public static MapPropertyInfo<TI, TValue> GetMapPropertyInfo<TI, TValue>(
            this Expression<Func<TI, TValue>> getIExpr)
            => new MapPropertyInfo<TI, TValue>(getIExpr);

        public class MapPropertyInfo<TI, TValue>
        {
            public Func<object, object> Getter { get; private set; }
            public Action<object, object> Setter { get; private set; }
            public string PropertyName { get; private set; }

            public MapPropertyInfo(Expression<Func<TI, TValue>> getIExpr)
            {
                var iPropertyInfo = GetPropertyInfo(getIExpr);

                // TODO: refactor this logic, separated in two places
                if (iPropertyInfo != null)
                {
                    Getter = i => iPropertyInfo.GetValue(i);
                    Setter = (i, v) => iPropertyInfo.SetValue(i, v);
                    PropertyName = iPropertyInfo.Name;
                }
                else
                {
                    Getter = i => i;
                    Setter = null;
                    PropertyName = "TODO";
                }
            }
        }
    }
}
