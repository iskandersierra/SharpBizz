using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/resource-scope-created", "0.1.0.0")]
    public class ResourceScopeCreated : IDomainEvent
    {
        public string Name { get; set; }
    }
}