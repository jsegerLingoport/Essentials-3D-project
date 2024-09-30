using System;

namespace Utilities.Events
{
    public interface IEventAggregator
    {
        public void Subscribe<T>(params Action<T>[] listeners) where T : struct;

        public void Unsubscribe<T>(params Action<T>[] listeners) where T : struct;

        public void Clear();

        public void Publish<T>(T eventData) where T : struct;

    }
}