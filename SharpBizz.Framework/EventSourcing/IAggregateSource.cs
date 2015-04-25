using System;
using System.Threading.Tasks;

namespace SharpBizz.Framework.EventSourcing
{
    public interface IAggregateSource
    {
        IAggregateSetup<TAggregate> SetupAggregate<TAggregate>(string id) where TAggregate: new();
    }

    public interface IAggregateSetup<TAggregate>
    {
        IAggregateEventSetup<TAggregate, TEvent> When<TEvent>(Func<TAggregate, TEvent, TAggregate> transformAggregate);
        IAggregateEventSetup<TAggregate, TEvent> When<TEvent>(Func<TAggregate, TAggregate> transformAggregate);
        IAggregateEventSetup<TAggregate, TEvent> When<TEvent>(Action<TAggregate, TEvent> transformAggregate);
        IAggregateEventSetup<TAggregate, TEvent> When<TEvent>(Action<TAggregate> transformAggregate);
        IAggregateEventSetup<TAggregate, TEvent> When<TEvent>(Func<TAggregate> transformAggregate);
        IAggregateEventSetup<TAggregate, TEvent> When<TEvent>(Action transformAggregate);
        Task<TAggregate> LoadAsync();
    }

    public interface IAggregateEventSetup<TAggregate, TEvent> : IAggregateSetup<TAggregate>
    {
        IAggregateEventSetup<TAggregate, TEvent> If(Func<TAggregate, TEvent, bool> predicate);
        IAggregateEventSetup<TAggregate, TEvent> If(Func<TAggregate, bool> predicate);
        IAggregateEventSetup<TAggregate, TEvent> If(Func<bool> predicate);
    }
}
