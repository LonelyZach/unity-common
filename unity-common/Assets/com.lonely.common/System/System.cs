using System;
using com.lonely.common.Messaging;
using com.lonely.common.System.Simulation;
using com.lonely.common.System.State;

namespace com.lonely.common.System
{
  public class System<TState> : ISystem
    where TState : RootStateEntity
  {
    public readonly Simulation<TState> Simulation;
    private readonly SystemBus _bus;
    
    public System(TState state)
    {
      Simulation = new Simulation<TState>(state);
      _bus = new SystemBus(this);
    }

    public System(Simulation<TState> simulation)
    {
      Simulation = simulation;
      _bus = new SystemBus(this);
    }

    public System<TState> DeepCopy()
    {
      return new System<TState>(Simulation.DeepCopy());
    }

    public void Destroy()
    {
      GlobalBus.Deregister(this);
    }

    public void RegisterHandler<TMessage>(Action<TMessage> handler) where TMessage : Message
    {
      RegisterHandler(handler, null);
    }

    public void RegisterHandler<TMessage>(Action<TMessage> handler, Func<TMessage, bool> predicate) where TMessage : Message
    {
      GlobalBus.RegisterHandler(this, handler, predicate);
    }

    public void Dispatch<TMessage>(TMessage message) where TMessage : Message
    {
      GlobalBus.Dispatch(this, message);
    }

    public bool Step(int step)
    {
      return Simulation.Step(step);
    }

    public TState GetState()
    {
      return Simulation.State;
    }
    
    public SystemBus GetBus()
    {
      return _bus;
    }
  }
}
