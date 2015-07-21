using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Commands
{
    [ContentTypeSchema("commands/add-scope-claim", "0.1.0.0")]
    public class AddScopeClaim : IDomainCommand
    {
        public string ClaimName { get; set; }
    }
}
