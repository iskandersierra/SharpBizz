using System;
using System.Diagnostics.Contracts;

namespace SharpBizz.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ContentTypeSchemaAttribute : Attribute
    {
        public ContentTypeSchemaAttribute(string schema)
            : this(schema, "0.1.0.0")
        {
            Contract.Requires(schema != null, "Content type schema prefix cannot be null");
            Contract.Requires(Uri.IsWellFormedUriString(schema, UriKind.RelativeOrAbsolute), "Content type schema prefix must be a valid uri");
        }

        public ContentTypeSchemaAttribute(string schema, string version)
            : this(schema, version, version)
        {
            Contract.Requires(schema != null, "Content type schema prefix cannot be null");
            Contract.Requires(version != null, "Content type schema version cannot be null");

            Contract.Requires(Uri.IsWellFormedUriString(schema, UriKind.RelativeOrAbsolute), "Content type schema prefix must be a valid uri");

            Version parsedVersion;
            Contract.Requires(System.Version.TryParse(version, out parsedVersion), "Invalid version format");
        }

        public ContentTypeSchemaAttribute(string schema, string version, string compatibleDownToVersion)
        {
            Contract.Requires(schema != null, "Content type schema prefix cannot be null");
            Contract.Requires(version != null, "Content type schema version cannot be null");
            Contract.Requires(compatibleDownToVersion != null, "Content type schema version compatibility cannot be null");

            Contract.Requires(Uri.IsWellFormedUriString(schema, UriKind.RelativeOrAbsolute), "Content type schema prefix must be a valid uri");

            Version parsedVersion;
            Contract.Requires(System.Version.TryParse(version, out parsedVersion), "Invalid version format");

            Version parsedCompatibleVersion;
            Contract.Requires(System.Version.TryParse(compatibleDownToVersion, out parsedCompatibleVersion), "Invalid compatible version format");

            Contract.Requires(parsedVersion >= parsedCompatibleVersion, "Compatibility schema version must be less than or equal to current schema version");

            Schema = schema;
            Version = version;
            CompatibleDownToVersion = compatibleDownToVersion;
        }

        public string Schema { get; private set; }

        public string Version { get; private set; }

        public string CompatibleDownToVersion { get; private set; }
    }
}