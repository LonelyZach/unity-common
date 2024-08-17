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
    
    public static Register On<TObservable>(Func<TObservable, bool> predicate, ObserverRegistry<TState>.Handler<TObservable> handler)
      where TObservable : IEcsObservable
    {
      void WrappedHandler(TState state, TObservable observable)
      {
        if (predicate(observable))
        {
          handler(state, observable);
        }
      }
      
      return registry => registry.Register((ObserverRegistry<TState>.Handler<TObservable>)WrappedHandler);
    }
  }
}