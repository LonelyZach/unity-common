using Messaging;

namespace System
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
      RegisterHandler(handler, null);
    }

    public void RegisterHandler<TMessage>(Action<TMessage> handler, Func<TMessage, bool> predicate) where TMessage : Message
    {
      _bus.RegisterHandler(_system, handler, predicate);
    }
    
    public void RegisterHandler<TMessage>(object owner, Action<TMessage> handler, Func<TMessage, bool> predicate) where TMessage : Message
    {
      _bus.RegisterHandler(_system, handler, predicate);
    }
    
    public void Deregister(object owner)
    {
      _bus.Deregister(owner);
    }

    public void Dispatch<TMessage>(TMessage message) where TMessage : Message
    {
      _bus.Dispatch(_system, message);
    }
  }
}
