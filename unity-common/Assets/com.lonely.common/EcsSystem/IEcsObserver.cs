using System;
using System.Collections.Generic;

namespace com.lonely.common.EcsSystem
{
  public interface IEcsObserver<TState>
    where TState : EcsRoot<TState>
  {
    public delegate void Register(ObserverRegistry<TState> registry);
    
    public abstract IEnumerable<Register> Observers();

    public static Register On<TObservable>(ObserverRegistry<TState>.Handler<TObservable> handler)
      where TObservable : IEcsObservable
    {
      return registry => registry.Register(handler);
    }
  }
}