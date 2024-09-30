using System;

namespace Utilities.Events
{
    public interface IEventAggregatorHandel 
    {
        public EventAggregator EventAggregator { get; }

        public void Subscribe<T>(params Action<T>[] listeners) where T : struct =>  EventAggregator.Subscribe(listeners);

        public void Unsubscribe<T>(params Action<T>[] listeners) where T : struct =>
            EventAggregator.Subscribe(listeners);

        public void Clear() => EventAggregator.Clear();

        public void Publish<T>(T eventData) where T : struct => EventAggregator.Publish(eventData);

    }
}