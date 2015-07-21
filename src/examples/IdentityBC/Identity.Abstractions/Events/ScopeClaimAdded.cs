using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/scope-claim-added", "0.1.0.0")]
    public class ScopeClaimAdded : IDomainEvent
    {
        public string ClaimName { get; set; }
    }
}