using System.Collections.Generic;
using SharpBizz.Domain.Attributes;

namespace SharpBizz.Domain
{
    public interface IDomainCommand
    {
    }

    [ContentTypeSchema("commands/execute-commands", "0.1.0.0")]
    public class ExecuteCommands : IDomainCommand
    {
        public List<IDomainCommand> Commands { get; set; }
    }

    //public interface IDomainCommandProcessor
    //{
    //    IEnumerable<IDomainEvent> GetEvents();
    //}

    //public interface ICreationCommandProcessor : IDomainCommandProcessor
    //{
        
    //}

    //public abstract class DomainCommandProcessor : IDomainCommandProcessor
    //{
    //    public abstract IEnumerable<IDomainEvent> GetEvents();
    //}

    //public abstract class CreationCommandProcessor : DomainCommandProcessor, ICreationCommandProcessor
    //{
        
    //}
}
