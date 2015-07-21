using SharpBizz.Domain;
using SharpBizz.Domain.Attributes;

namespace Identity.Abstractions.Commands
{
    [ContentTypeSchema("commands/enable-scope", "0.1.0.0")]
    public class EnableScope : IDomainCommand
    {
    }
}