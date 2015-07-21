using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Commands
{
    [ContentTypeSchema("commands/change-scope-name", "0.1.0.0")]
    public class ChangeScopeName : IDomainCommand
    {
        public string NewName { get; set; }
    }
}