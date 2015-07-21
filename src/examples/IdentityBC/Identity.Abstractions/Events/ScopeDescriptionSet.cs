using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/scope-description-set", "0.1.0.0")]
    public class ScopeDescriptionSet : IDomainEvent
    {
        public string Description { get; set; }
    }
}