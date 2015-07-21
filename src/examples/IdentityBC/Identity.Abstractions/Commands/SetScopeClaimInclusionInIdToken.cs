using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Commands
{
    [ContentTypeSchema("commands/set-scope-claim-inclusion-in-id-token", "0.1.0.0")]
    public class SetScopeClaimInclusionInIdToken : IDomainCommand
    {
        public string ClaimName { get; set; }
        public bool Always { get; set; }
    }
}