using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Commands
{
    [ContentTypeSchema("commands/set-scope-description", "0.1.0.0")]
    public class SetScopeDescription : IDomainCommand
    {
        public string Description { get; set; }
    }
}