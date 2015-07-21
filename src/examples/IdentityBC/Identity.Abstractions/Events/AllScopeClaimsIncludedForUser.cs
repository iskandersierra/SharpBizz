using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/all-scope-claims-included-for-user", "0.1.0.0")]
    public class AllScopeClaimsIncludedForUser : IDomainEvent
    {
        public bool Value { get; set; }
    }
}