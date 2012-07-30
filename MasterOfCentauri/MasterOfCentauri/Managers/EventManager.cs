using System;
using System.Collections.Generic;

namespace MasterOfCentauri.Managers
{
    public class EventManager
    {
        private readonly Dictionary<EventType, List<object>> _events;
        private readonly Dictionary<Type, List<object>> _listeners;

        public EventManager()
        {
            _events = new Dictionary<EventType, List<object>>
                          {
                              {EventType.Update, new List<object>()},
                              {EventType.Frame, new List<object>()},
                              {EventType.Turn, new List<object>()}
                          };

            _listeners = new Dictionary<Type, List<object>>();
        }

        public void Queue(EventType type, object eventObject)
        {
            lock(_events[type])
            {
                _events[type].Add(eventObject);
            }
        }

        public void Subscribe<T>(IEventListener<T> listener)
        {
            var type = typeof (T);
            lock (_listeners)
            {
                if (!_listeners.ContainsKey(type))
                    _listeners.Add(type, new List<object>());
            }

            lock (_listeners[type])
                _listeners[type].Add(listener);

        }

        public void Execute(EventType type)
        {
            IEnumerable<object> eventsToExecute = FindEventsToExecuteAndFlushList(type);
            ExecuteEvents(eventsToExecute);
        }

        private void ExecuteEvents(IEnumerable<object> eventsToExecute)
        {
            foreach (var e in eventsToExecute)
                FindAndExecuteListeners(e);
        }

        private void FindAndExecuteListeners(object e)
        {
            var listeners = FindListeners(e);
            ExecuteListeners(listeners, e);
        }

        private static void ExecuteListeners(IEnumerable<dynamic> listeners, dynamic e)
        {
            foreach (dynamic l in listeners)
                l.EventRaised(e);
        }

        private IEnumerable<dynamic> FindListeners(object e)
        {
            var listOfListenersToCurrentType = new List<object>();

            if(_listeners.ContainsKey(e.GetType()))
            {
                lock (_listeners)
                    listOfListenersToCurrentType = _listeners[e.GetType()];
            }

            object[] listeners;
            lock (listOfListenersToCurrentType)
                listeners = listOfListenersToCurrentType.ToArray();


            return listeners;
        }

        private IEnumerable<object> FindEventsToExecuteAndFlushList(EventType type)
        {
            object[] eventsToExecute;
            lock (_events[type])
            {
                eventsToExecute = _events[type].ToArray();
                _events[type].Clear();
            }
            return eventsToExecute;
        }
    }
}