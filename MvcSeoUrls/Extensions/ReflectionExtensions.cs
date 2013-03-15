using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcSeoUrls.Extensions
{
    public static class ReflectionExtensions
    {
        public static bool IsValueType(this Type type)
        {
            return (type.IsValueType && type.IsPrimitive) || type == typeof(string);
        }

        public static bool IsGenericList(this Type type)
        {
            return type.GetInterfaces().Any(
                interfaceType =>
                interfaceType.IsGenericType &&
                (interfaceType.GetGenericTypeDefinition() == typeof (IList<>) ||
                 (interfaceType.GetGenericTypeDefinition() == typeof (IEnumerable<>) ||
                  (interfaceType.GetGenericTypeDefinition() == typeof (ICollection<>)))));
        }
    }
}