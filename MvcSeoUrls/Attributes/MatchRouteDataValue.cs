using System;

namespace MvcSeoUrls.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class MatchRouteDataValue : Attribute
    {
        public MatchRouteDataValue() { }
    }
}