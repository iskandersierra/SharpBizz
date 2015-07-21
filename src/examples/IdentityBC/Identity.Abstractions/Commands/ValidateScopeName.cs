using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Commands
{
    [ContentTypeSchema("commands/validate-scope-name", "0.1.0.0")]
    public class ValidateScopeName : IDomainCommand
    {
    }
}