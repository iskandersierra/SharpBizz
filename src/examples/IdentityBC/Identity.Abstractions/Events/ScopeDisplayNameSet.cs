using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/scope-display-name-set", "0.1.0.0")]
    public class ScopeDisplayNameSet : IDomainEvent
    {
        public string DisplayName { get; set; }
    }
}