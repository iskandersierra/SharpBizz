using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace SharpBizz.Domain.Attributes
{
    public static class ContentTypeSchemaExtensions
    {
        public static ContentTypeSchemaAttribute GetContentTypeSchema(this Type type)
        {
            Contract.Requires(type != null, "Type cannot be null");

            var attr = type.GetCustomAttribute<ContentTypeSchemaAttribute>();
            if (attr == null) return null;

            var uri = new Uri(attr.Schema, UriKind.RelativeOrAbsolute);
            if (uri.IsAbsoluteUri)
                return attr;

            var prefAttr = type.Assembly.GetCustomAttribute<ContentTypeSchemaPrefixAttribute>();
            if (prefAttr == null)
                throw new InvalidOperationException("Content type schema cannot be a relative uri. You can use ContentTypeSchemaPrefixAttribute at assembly level to mark all relative schemas with a valid absolute uri prefix.");

            uri = new Uri(prefAttr.Prefix, UriKind.Absolute);
            uri = new Uri(uri, attr.Schema);

            attr = new ContentTypeSchemaAttribute(uri.AbsoluteUri, attr.Version, attr.CompatibleDownToVersion);

            return attr;
        }

        public static ContentTypeSchemaAttribute GetContentTypeSchema(this object obj)
        {
            Contract.Requires(obj != null, "Object cannot be null");

            return obj.GetType().GetContentTypeSchema();
        }
    }
}