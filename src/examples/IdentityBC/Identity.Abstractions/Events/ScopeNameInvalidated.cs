using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/scope-name-invalidated", "0.1.0.0")]
    public class ScopeNameInvalidated : IDomainEvent
    {
        public string Reason { get; set; }
    }
}