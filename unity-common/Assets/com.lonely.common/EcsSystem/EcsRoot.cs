using System;
using System.Collections.Generic;
using System.Linq;
using com.lonely.common.System.State;

namespace com.lonely.common.EcsSystem
{
  public abstract class EcsRoot<TState> : RootStateEntity
    where TState : EcsRoot<TState>
  {
    public EcsRoot(int step, TypedListDictionary<Component> components, TypedListDictionary<Entity> entities, TypedListDictionary<Cmd> cmds, TypedListDictionary<Evt> evts, ObserverRegistry<TState> observers)
    : base(step)
    {
      Components = components ?? new TypedListDictionary<Component>();
      Entities = entities ?? new TypedListDictionary<Entity>();
      Cmds = cmds ?? new TypedListDictionary<Cmd>();
      Evts = evts ?? new TypedListDictionary<Evt>();
      
      Observers = observers ?? BuildRegistry();
    }

    public readonly ObserverRegistry<TState> Observers;
    
    public readonly TypedListDictionary<Component> Components;
    public readonly TypedListDictionary<Entity> Entities;
    
    public readonly TypedListDictionary<Cmd> Cmds;
    public readonly TypedListDictionary<Evt> Evts;
    
    public override IEnumerable<IStateEntity> GetChildren()
    {
      return Enumerable.Empty<IStateEntity>();
    }

    public void AddComponentTo(Entity entity, Component component)
    {
      entity.Components.Add(component);
      var simulateAt = component.SimulateAt;
      if (simulateAt.HasValue)
      {
        BuildTimer(Step, simulateAt.Value, component.GetType().FullName);
      }
    }

    // TODO: Don't do this at runtime.
    public static ObserverRegistry<TState> BuildRegistry()
    {
      var observerType = typeof(IEcsObserver<TState>);
      var stateType = typeof(TState);
      var observerTypes = stateType.Assembly.GetTypes().Where(x => !x.IsAbstract && !x.IsInterface && observerType.IsAssignableFrom(x));
      var observers = observerTypes.Select(Activator.CreateInstance);
      var registry = new ObserverRegistry<TState>();
      foreach (var observer in observers)
      {
        var typedObserver = (IEcsObserver<TState>)observer;
        foreach (var register in typedObserver.Observers())
        {
          register(registry);
        }
      }

      return registry;
    }
  }
}