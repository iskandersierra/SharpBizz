using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Commands
{
    [ContentTypeSchema("commands/set-scope-display-name", "0.1.0.0")]
    public class SetScopeDisplayName : IDomainCommand
    {
        public string DisplayName { get; set; }
    }
}