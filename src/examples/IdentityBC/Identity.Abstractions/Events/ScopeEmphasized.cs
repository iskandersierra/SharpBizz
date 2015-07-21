using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/scope-emphasized", "0.1.0.0")]
    public class ScopeEmphasized : IDomainEvent
    {
    }
}