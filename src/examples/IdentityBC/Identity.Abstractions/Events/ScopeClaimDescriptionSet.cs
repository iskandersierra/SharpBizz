using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/scope-claim-description-set", "0.1.0.0")]
    public class ScopeClaimDescriptionSet : IDomainEvent
    {
        public string ClaimName { get; set; }
        public string Description { get; set; }
    }
}