using System;
using System.ComponentModel;

namespace MvcSeoUrls.Attributes
{
    internal sealed class RouteAliasPropertyDescriptor : PropertyDescriptor
    {
        public PropertyDescriptor Inner { get; private set; }

        public RouteAliasPropertyDescriptor(string alias, PropertyDescriptor inner)
            : base(alias, null)
        {
            Inner = inner;
        }

        public override bool CanResetValue(object component)
        {
            return Inner.CanResetValue(component);
        }

        public override Type ComponentType
        {
            get { return Inner.ComponentType; }
        }

        public override object GetValue(object component)
        {
            return Inner.GetValue(component);
        }

        public override bool IsReadOnly
        {
            get { return Inner.IsReadOnly; }
        }

        public override Type PropertyType
        {
            get { return Inner.PropertyType; }
        }

        public override void ResetValue(object component)
        {
            Inner.ResetValue(component);
        }

        public override void SetValue(object component, object value)
        {
            Inner.SetValue(component, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return Inner.ShouldSerializeValue(component);
        }
    }
}