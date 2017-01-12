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

        public static MapPropertyInfo GetMapPropertyInfo<TI, TValue>(
            this Expression<Func<TI, TValue>> getIExpr)
            => new MapPropertyInfo(getIExpr.GetPropertyInfo());

        public static MapPropertyInfo GetMapPropertyInfo(
            this Type targetType, string propertyName)
            => new MapPropertyInfo(targetType.GetProperty(propertyName));

        public class MapPropertyInfo
        {
            public Func<object, object> Getter { get; private set; }
            public Action<object, object> Setter { get; private set; }
            public string PropertyName { get; private set; }
            public PropertyInfo PropertyInfo { get; set; }

            public MapPropertyInfo(PropertyInfo propertyInfo)
            {
                // TODO: refactor this logic, separated in two places
                if (propertyInfo != null)
                {
                    Getter = i => propertyInfo.GetValue(i);
                    Setter = (i, v) => propertyInfo.SetValue(i, v);
                    PropertyName = propertyInfo.Name;
                    PropertyInfo = propertyInfo;
                }
                else
                {
                    Getter = i => i;
                    Setter = null;
                    PropertyName = "TODO";
                    PropertyInfo = null;
                }
            }
        }
    }
}
