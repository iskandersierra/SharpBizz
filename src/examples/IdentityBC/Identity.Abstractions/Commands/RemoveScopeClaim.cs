using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Commands
{
    [ContentTypeSchema("commands/remove-scope-claim", "0.1.0.0")]
    public class RemoveScopeClaim : IDomainCommand
    {
        public string ClaimName { get; set; }
    }
}