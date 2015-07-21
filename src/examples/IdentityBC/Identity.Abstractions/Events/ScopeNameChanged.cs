using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/scope-name-changed", "0.1.0.0")]
    public class ScopeNameChanged : IDomainEvent
    {
        public string NewName { get; set; }
    }
}