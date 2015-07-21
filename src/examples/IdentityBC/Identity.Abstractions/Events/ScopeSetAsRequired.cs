using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/scope-set-as-required", "0.1.0.0")]
    public class ScopeSetAsRequired : IDomainEvent
    {
    }
}