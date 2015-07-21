using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Commands
{
    [ContentTypeSchema("commands/create-identity-scope", "0.1.0.0")]
    public class CreateIdentityScope : IDomainCommand
    {
        public string Name { get; set; }
    }
}
