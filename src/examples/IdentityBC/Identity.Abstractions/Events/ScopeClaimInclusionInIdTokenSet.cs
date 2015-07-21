using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/scope-claim-inclusion-in-id-token", "0.1.0.0")]
    public class ScopeClaimInclusionInIdTokenSet : IDomainEvent
    {
        public string ClaimName { get; set; }
        public bool Always { get; set; }
    }
}