using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/scope-hidden-in-discovery-document", "0.1.0.0")]
    public class ScopeHiddenInDiscoveryDocument : IDomainEvent
    {
    }
}