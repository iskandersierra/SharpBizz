using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/scope-de-emphasized", "0.1.0.0")]
    public class ScopeDeEmphasized : IDomainEvent
    {
    }
}