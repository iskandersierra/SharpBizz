using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Commands
{
    [ContentTypeSchema("commands/invalidate-scope-name", "0.1.0.0")]
    public class InvalidateScopeName : IDomainCommand
    {
        public string Reason { get; set; }
    }
}