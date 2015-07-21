using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/scope-claim-removed", "0.1.0.0")]
    public class ScopeClaimRemoved : IDomainEvent
    {
        public string ClaimName { get; set; }
    }
}