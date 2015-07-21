using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Events
{
    [ContentTypeSchema("events/scope-name-validated", "0.1.0.0")]
    public class ScopeNameValidated : IDomainEvent
    {
    }
}