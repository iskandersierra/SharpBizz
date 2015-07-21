using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBizz.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed class ContentTypeSchemaPrefixAttribute : Attribute
    {
        public ContentTypeSchemaPrefixAttribute(string prefix)
        {
            Contract.Requires(prefix != null, "Content type schema prefix cannot be null");
            Contract.Requires(Uri.IsWellFormedUriString(prefix, UriKind.Absolute), "Content type schema prefix must be an absolute uri");

            Prefix = prefix;
        }

        public string Prefix { get; private set; }
    }
}
