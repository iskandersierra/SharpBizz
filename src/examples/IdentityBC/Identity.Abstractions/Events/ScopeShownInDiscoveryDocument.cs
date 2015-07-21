using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/scope-shown-in-discovery-document", "0.1.0.0")]
    public class ScopeShownInDiscoveryDocument : IDomainEvent
    {
    }
}