using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web.Routing;
using MvcSeoUrls.Attributes;

namespace MvcSeoUrls.Extensions
{
    public static class RouteValueExtensions
    {
        public static RouteValueDictionary ToRouteData<T>(this T @object)
        {
            var specificRouteType = @object.GetType();

            var attribType = typeof(RouteAliasAttribute);

            var routeProperties = specificRouteType.GetProperties().Where(x => x.GetCustomAttributes(attribType, false).Any());

            var routeData = new RouteValueDictionary();

            var propertyInfos = routeProperties as PropertyInfo[] ?? routeProperties.ToArray();

            AddPropertiesToRouteData(@object, propertyInfos, routeData, attribType);

            var properties = specificRouteType.GetProperties().Where(x => !propertyInfos.Contains(x));

            AddPropertiesToRouteData(@object, properties, routeData);

            return routeData;
        }

        public static bool EnsureRouteDataValueMatches<T>(this T @object, RouteValueDictionary values)
        {
            var atLeastOneRouteDataValueHasNotMatched = false;

            if (values == null) return false;

            var routeDataType = @object.GetType();

            var attribType = typeof(MatchRouteDataValue);

            var propertiesToCompare = routeDataType.GetProperties().Where(x => x.GetCustomAttributes(attribType, false).Any());

            foreach (var propertyInfo in propertiesToCompare)
            {
                var value = propertyInfo.GetValue(@object, null);
                var stringValue = !string.IsNullOrWhiteSpace(value.ToString()) ? value.ToString().EscapeName() : string.Empty;
                if (values.ContainsKey(propertyInfo.Name) && string.Compare(values[propertyInfo.Name].ToString(), stringValue.EscapeName(), StringComparison.InvariantCultureIgnoreCase) != 0)
                {
                    atLeastOneRouteDataValueHasNotMatched = true;
                    values[propertyInfo.Name] = stringValue.EscapeName();
                }
            }

            return atLeastOneRouteDataValueHasNotMatched;
        }
      
        private static void AddPropertiesToRouteData<T>(T @object, IEnumerable<PropertyInfo> properties, IDictionary<string, object> routeData)
        {
            foreach (var property in properties)
            {
                var value = property.GetValue(@object, null);
                var stringValue = string.Empty;

                if (value != null && value.GetType().IsValueType())
                {
                    stringValue = value.ToString();
                    routeData.Add(property.Name, stringValue.EscapeName());
                }
                else if (value != null && value.GetType().IsGenericList())
                {
                    var type = property.PropertyType.GetGenericArguments()[0];
                    var listType = typeof(Collection<>).MakeGenericType(type);
                    var instance = Activator.CreateInstance(listType, value);
                    stringValue = string.Join(",", ((ICollection)instance).Cast<object>().ToArray());
                    routeData.Add(property.Name, stringValue);
                }
            }
        }

        private static void AddPropertiesToRouteData<T>(T @object, IEnumerable<PropertyInfo> properties, IDictionary<string, object> routeData, Type attribType)
        {          
            foreach (var property in properties)
            {
                var aliasedProperties = property.GetCustomAttributes(attribType, false).First() as RouteAliasAttribute;

                if (aliasedProperties == null) break;

                var value = property.GetValue(@object, null);

                if (value != null && value.GetType().IsValueType())
                {
                    var stringValue = value.ToString();
                    routeData.Add(aliasedProperties.Alias, stringValue.EscapeName());
                }
                else if (value != null && value.GetType().IsGenericList())
                {
                    var type = property.PropertyType.GetGenericArguments()[0];
                    var listType = typeof (Collection<>).MakeGenericType(type);
                    var instance = Activator.CreateInstance(listType, value);
                    var stringValue = string.Join(",", ((ICollection) instance).Cast<object>().ToArray());
                    routeData.Add(aliasedProperties.Alias, stringValue);
                }
            }
        }
    }
}