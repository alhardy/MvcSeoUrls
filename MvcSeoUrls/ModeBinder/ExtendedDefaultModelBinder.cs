using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MvcSeoUrls.Attributes;

namespace MvcSeoUrls.ModeBinder
{
    public class ExtendedDefaultModelBinder : DefaultModelBinder
    {
        private static readonly MethodInfo ToArrayMethod = typeof(Enumerable).GetMethod("ToArray");

        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            if (propertyDescriptor.PropertyType.GetInterface(typeof(IEnumerable).Name) != null)
            {
                var actualValue = bindingContext.ValueProvider.GetValue(propertyDescriptor.Name);

                if (actualValue != null && !string.IsNullOrWhiteSpace(actualValue.AttemptedValue) && actualValue.AttemptedValue.Contains(","))
                {
                    var valueType = propertyDescriptor.PropertyType.GetElementType() ?? propertyDescriptor.PropertyType.GetGenericArguments().FirstOrDefault();

                    if (valueType != null && valueType.GetInterface(typeof(IConvertible).Name) != null)
                    {
                        var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(valueType));

                        foreach (var splitValue in actualValue.AttemptedValue.Split(new[] { ',' }))
                        {
                            list.Add(Convert.ChangeType(splitValue, valueType));
                        }

                        if (propertyDescriptor.PropertyType.IsArray)
                        {
                            return ToArrayMethod.MakeGenericMethod(valueType).Invoke(this, new object[] { list });
                        }

                        return list;
                    }
                }
            }

            return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }

        protected override PropertyDescriptorCollection GetModelProperties(ControllerContext controllerContext,ModelBindingContext bindingContext)
        {
            var properties = base.GetModelProperties(controllerContext, bindingContext);

            var additional = new List<PropertyDescriptor>();            

            foreach (var p in GetTypeDescriptor(controllerContext, bindingContext).GetProperties().Cast<PropertyDescriptor>())
            {                
                foreach (var attr in p.Attributes.OfType<RouteAliasAttribute>())
                {
                    additional.Add(new RouteAliasPropertyDescriptor(attr.Alias, p));

                    if (bindingContext.PropertyMetadata.ContainsKey(p.Name))
                        bindingContext.PropertyMetadata.Add(attr.Alias, bindingContext.PropertyMetadata[p.Name]);                   
                }
            }

            return new PropertyDescriptorCollection(properties.Cast<PropertyDescriptor>().Concat(additional).ToArray());
        }
    }
}