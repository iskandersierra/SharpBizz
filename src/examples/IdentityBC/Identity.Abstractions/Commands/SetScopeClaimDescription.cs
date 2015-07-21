using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Commands
{
    [ContentTypeSchema("commands/set-scope-claim-description", "0.1.0.0")]
    public class SetScopeClaimDescription : IDomainCommand
    {
        public string ClaimName { get; set; }
        public string Description { get; set; }
    }
}