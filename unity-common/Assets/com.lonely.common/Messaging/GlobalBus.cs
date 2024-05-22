using System;
using System.Collections.Generic;
using System.IO;

namespace Messaging
{
  public static class GlobalBus
  {
    private static Bus _bus = new Bus();

    public static void RegisterHandler<TMessage>(object system, Action<TMessage> handler)
    {
      _bus.RegisterHandler(system, handler);
    }

    public static void RegisterHandler<TMessage>(object system, Action<TMessage> handler, Func<TMessage, bool> predicate)
    {
      _bus.RegisterHandler(system, handler, predicate);
    }

    public static void Dispatch<TMessage>(object system, TMessage message)
    {
      _bus.Dispatch(system, message);
    }
    
    public static void Deregister(object system)
    {
      _bus.Deregister(system);
    }
  }
}