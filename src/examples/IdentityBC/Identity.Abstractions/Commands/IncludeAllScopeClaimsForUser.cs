using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Commands
{
    [ContentTypeSchema("commands/include-all-scope-claims-for-user", "0.1.0.0")]
    public class IncludeAllScopeClaimsForUser : IDomainCommand
    {
        public bool Value { get; set; }
    }
}