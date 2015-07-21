using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Commands
{
    [ContentTypeSchema("commands/set-scope-claims-rule", "0.1.0.0")]
    public class SetScopeClaimsRule : IDomainCommand
    {
        public string ClaimsRule { get; set; }
    }
}