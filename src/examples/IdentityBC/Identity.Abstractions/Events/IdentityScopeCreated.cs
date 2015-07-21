using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/identity-scope-created", "0.1.0.0")]
    public class IdentityScopeCreated : IDomainEvent
    {
        public string Name { get; set; }
    }
}
