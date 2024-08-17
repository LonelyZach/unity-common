using System;
using com.lonely.common.Messaging;

namespace com.lonely.common.System
{
  public class SystemBus
  {
    private readonly object _system;
    private readonly Bus _bus = new Bus();
    
    public SystemBus(object system)
    {
      _system = system;
    }
    
    public void RegisterHandler<TMessage>(Action<TMessage> handler) where TMessage : Message
    {
      if (_system == null)
      {
        return;
      }
      
      RegisterHandler(handler, null);
    }

    public void RegisterHandler<TMessage>(Action<TMessage> handler, Func<TMessage, bool> predicate) where TMessage : Message
    {
      if (_system == null)
      {
        return;
      }

      _bus.RegisterHandler(_system, handler, predicate);
    }
    
    public void RegisterHandler<TMessage>(object owner, Action<TMessage> handler, Func<TMessage, bool> predicate) where TMessage : Message
    {
      if (_system == null)
      {
        return;
      }

      _bus.RegisterHandler(_system, handler, predicate);
    }
    
    public void Deregister(object owner)
    {
      if (_system == null)
      {
        return;
      }

      _bus.Deregister(owner);
    }

    public void Dispatch<TMessage>(TMessage message) where TMessage : Message
    {
      if (_system == null)
      {
        return;
      }

      _bus.Dispatch(_system, message);
    }
  }
}
