using System;

namespace MvcSeoUrls.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class RouteAliasAttribute : Attribute
    {
        public RouteAliasAttribute(string alias)
        {
            Alias = alias;
        }
        public string Alias { get; private set; }
    }
}