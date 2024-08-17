using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;

namespace com.lonely.common.EcsSystem
{
  public class ObserverRegistry<TState>
    where TState : EcsRoot<TState>
  {
    public delegate void Handler<TObservable>(TState state, TObservable observable) where TObservable : IEcsObservable;
    
    private readonly TypedListDictionary<object> _handlers  = new ();

    public void Register<TObservable>(Handler<TObservable> handler) where TObservable : IEcsObservable
    {
      _handlers.Add(typeof(TObservable), handler);
    }

    public void Notify<TObservable>(TState state, TObservable observable) where TObservable : IEcsObservable
    {
      var observableType = typeof(TObservable);
      var handlers = _handlers.Get(observableType);
      foreach (var handler in handlers)
      {
        ((Handler<TObservable>)handler)(state, observable);
      }
    }
  }
}