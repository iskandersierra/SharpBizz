using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Commands
{
    [ContentTypeSchema("commands/create-resource-scope", "0.1.0.0")]
    public class CreateResourceScope : IDomainCommand
    {
        public string Name { get; set; }
    }
}