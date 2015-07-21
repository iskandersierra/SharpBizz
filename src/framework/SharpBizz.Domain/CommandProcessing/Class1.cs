using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBizz.Domain.CommandProcessing
{
    public interface ICommandProcessorFactory
    {
        ICommandProcessor GetCommandProcessor(CommandProcessorContext context);
    }

    public interface ICommandProcessor
    {
        
    }

    public abstract class ProcessingContextBase
    {
        private readonly Dictionary<string, object> _processingContext;


        public ProcessingContextBase(Dictionary<string, object> processingContext)
        {
            Contract.Requires(processingContext != null, "Processing context cannot be null");

            _processingContext = processingContext;
        }

        public Dictionary<string, object> ProcessingContext
        {
            get { return _processingContext; }
        }

        protected object GetValue(string keyName)
        {

        }

        protected T GetValue<T>(string keyName)
        {

        }

        protected void SetValue(string keyName, object value)
        {
        }

        protected void SetValue<T>(string keyName, T value)
        {
        }
    }

    public class CommandProcessorContext : ProcessingContextBase
    {
        public CommandProcessorContext(Dictionary<string, object> processingContext) 
            : base(processingContext)
        {
            Contract.Requires(processingContext != null, "Processing context cannot be null");
        }

        public string AggregateId
        {
            get
            {
                return GetValue<string>(HeaderKeys.AggregateId);
            }
            set
            {
                SetValue(HeaderKeys.AggregateId, value);
            }
        }
    }

    public static class HeaderKeys
    {
        public const string AggregateId = "AggregateId";
        
    }
}
