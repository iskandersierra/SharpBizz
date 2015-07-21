using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Commands
{
    [ContentTypeSchema("commands/set-scope-required", "0.1.0.0")]
    public class SetScopeRequired : IDomainCommand
    {
    }
}