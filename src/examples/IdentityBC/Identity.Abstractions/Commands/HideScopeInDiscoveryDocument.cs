using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Commands
{
    [ContentTypeSchema("commands/hide-scope-in-discovery-document", "0.1.0.0")]
    public class HideScopeInDiscoveryDocument : IDomainCommand
    {
    }
}