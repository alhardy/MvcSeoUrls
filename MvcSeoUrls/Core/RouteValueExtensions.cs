using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web.Routing;
using MvcSeoUrls.Attributes;
using MvcSeoUrls.Extensions;

namespace MvcSeoUrls.Core
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