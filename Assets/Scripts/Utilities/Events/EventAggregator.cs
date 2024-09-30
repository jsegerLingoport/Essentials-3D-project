using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Utilities.Events
{
    public abstract class EventAggregator : SerializedScriptableObject
    {
        [ShowInInspector, OdinSerialize]
        private Dictionary<Type, List<Delegate>> _eventListeners = new();

        public void Subscribe<T>(params Action<T>[] listeners) where T : struct
        {
            foreach (var listener in listeners)
            {
                if (_eventListeners.TryGetValue(typeof(T), out var listenerActions))
                {
                    if (!listenerActions.Contains(listener))
                    {
                        listenerActions.Add(listener);
                    }
                }
                else
                {
                    _eventListeners[typeof(T)] = new List<Delegate> { listener };
                }
            }
        }

        public void Clear()
        {
            _eventListeners = new Dictionary<Type, List<Delegate>>();
        }
        public void Unsubscribe<T>(params Action<T>[] listeners) where T : struct
        {
            foreach (var listener in listeners)
            {
                if (_eventListeners.TryGetValue(typeof(T), out var listenersActions))
                {
                    listenersActions.Remove(listener);
                }
            }
        }

        public void Publish<T>(T eventData) where T : struct
        {
            if (!_eventListeners.TryGetValue(typeof(T), out var listeners)) return;

            // Create a copy of the listeners to avoid modifying the collection during iteration
            var listenersCopy = listeners.ToList(); 

            foreach (var listener in listenersCopy)
            {
                ((Action<T>)listener)?.Invoke(eventData);
            }
        }
    }
}