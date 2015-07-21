using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/scope-claims-rule-set", "0.1.0.0")]
    public class ScopeClaimsRuleSet : IDomainEvent
    {
        public string ClaimsRule { get; set; }
    }
}