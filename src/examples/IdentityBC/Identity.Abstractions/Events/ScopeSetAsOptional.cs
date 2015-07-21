using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/scope-set-as-optional", "0.1.0.0")]
    public class ScopeSetAsOptional : IDomainEvent
    {
    }
}