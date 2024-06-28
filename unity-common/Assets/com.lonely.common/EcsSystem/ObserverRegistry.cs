using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;

namespace com.lonely.common.EcsSystem
{
  public class ObserverRegistry<TState>
    where TState : EcsRoot<TState>
  {
    public delegate void Handler<TObservable>(TState state, TObservable observable);
    
    private readonly TypedListDictionary<object> _handlers  = new TypedListDictionary<object>();

    public void Register<TObservable>(Handler<TObservable> handler)
    {
      _handlers.Add(handler);
    }

    public void Notify<TObservable>(TState state, TObservable observable)
    {
      var handlers = _handlers.Get<Handler<TObservable>>();
      foreach (var handler in handlers)
      {
        handler(state, observable);
      }
    }
  }
}