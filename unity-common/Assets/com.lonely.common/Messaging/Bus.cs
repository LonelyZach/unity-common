using System;
using System.Collections.Generic;

namespace com.lonely.common.Messaging
{
  public class Bus
  {
    private static IDictionary<object, MessageHandlers> _systems = new Dictionary<object, MessageHandlers>();

    public void RegisterHandler<TMessage>(object system, Action<TMessage> handler)
    {
      RegisterHandler(system, handler, null);
    }

    public void RegisterHandler<TMessage>(object system, Action<TMessage> handler, Func<TMessage, bool> predicate)
    {
      var messageType = typeof(TMessage);

      void WrappedHandler(object x) => handler((TMessage)x);
      bool WrappedPredicate(object x) => predicate((TMessage)x);
      RegisterHandler(system, messageType, WrappedHandler, predicate == null ? null : WrappedPredicate);
    }

    private void RegisterHandler(object system, Type messageType, Action<object> handler, Func<object, bool> predicate)
    {
      if (!_systems.TryGetValue(system, out var handlers))
      {
        handlers = new MessageHandlers();
        _systems.Add(system, handlers);
      }

      if (!handlers.HandlersByMessageType.ContainsKey(messageType))
      {
        handlers.HandlersByMessageType.Add(messageType, new List<MessageHandler>());
      }
      
      handlers.HandlersByMessageType[messageType].Add(new MessageHandler(handler, predicate));
    }

    public void Deregister(object system)
    {
      _systems.Remove(system);
    }
    
    public void Dispatch(object system, object message)
    {
      var messageType = message.GetType();
      foreach (var registeredSystem in _systems)
      {
        if (!registeredSystem.Value.HandlersByMessageType.TryGetValue(messageType, out var handlers))
        {
          continue;
        }

        foreach (var handler in handlers)
        {
          var action = handler.Handler;
          var predicate = handler.Predicate;
          if (predicate != null)
          { 
            if (!predicate.Invoke(message))
            {
              continue;
            }
          }

          action.Invoke(message);
        }
      }
    }

    public class MessageHandlers
    {
      public MessageHandlers()
      {
        HandlersByMessageType = new Dictionary<Type, IList<MessageHandler>>();
      }

      public IDictionary<Type, IList<MessageHandler>> HandlersByMessageType;
    }

    public class MessageHandler
    {
      public MessageHandler(Action<object> handler, Func<object, bool> predicate)
      {
        Handler = handler;
        Predicate = predicate;
      }

      public Action<object> Handler;
      public Func<object, bool> Predicate;
    }
  }
}