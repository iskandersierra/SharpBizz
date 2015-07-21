using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Commands
{
    [ContentTypeSchema("commands/set-scope-optional", "0.1.0.0")]
    public class SetScopeOptional : IDomainCommand
    {
    }
}