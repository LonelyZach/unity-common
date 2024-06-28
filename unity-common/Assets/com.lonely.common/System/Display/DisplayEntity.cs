using System;
using System.Collections.Generic;
using com.lonely.common.Logging;
using com.lonely.common.System.State;

namespace com.lonely.common.System.Display
{
  public abstract class DisplayEntity<TState, TStateEntity, TLocationFinder> : DisplayEntityBase<TLocationFinder>
    where TState : RootStateEntity
    where TLocationFinder : LocationFinderBehaviour
  {
    private readonly List<IDisplayEntityListener<TState, TStateEntity, TLocationFinder>> _listeners = new List<IDisplayEntityListener<TState, TStateEntity, TLocationFinder>>();

    public override Type StateEntityType => typeof(TStateEntity);

    protected SystemBus Bus { get; private set; }
    protected TState State { get; private set; }
    protected TStateEntity StateEntity { get; private set; }

    public override void InitUnsafe(IStateEntity state, object stateEntity, TLocationFinder locationFinder, float simulationStartTime, SystemBus bus)
    {
      Bus = bus;
      _listeners.AddRange(gameObject.GetComponents<IDisplayEntityListener<TState, TStateEntity, TLocationFinder>>());
      
      if (!(stateEntity is TStateEntity))
      {
        Log.Error($"Incorrect display handler for state entity of type {stateEntity.GetType()}");
        return;
      }

      State = (TState)state;
      StateEntity = (TStateEntity)stateEntity;

      Init(State, StateEntity, locationFinder, simulationStartTime);
      foreach (var listener in _listeners)
      {
        listener.Init(State, StateEntity, locationFinder, simulationStartTime);
      }
    }

    protected abstract void Init(TState state, TStateEntity stateEntity, TLocationFinder locationFinder, float simulationStartTime);

    public override void SyncUnsafe(IStateEntity state, object stateEntity, TLocationFinder locationFinder, float simulationStartTime)
    {
      if (!(stateEntity is TStateEntity))
      {
        Log.Error($"Incorrect display handler for state entity of type {stateEntity.GetType()}");
        return;
      }

      State = (TState)state;
      StateEntity = (TStateEntity)stateEntity;

      Sync(State, StateEntity, locationFinder, simulationStartTime);
      foreach (var listener in _listeners)
      {
        listener.Sync(State, StateEntity, locationFinder, simulationStartTime);
      }
    }

    protected abstract void Sync(TState state, TStateEntity stateEntity, TLocationFinder locationFinder, float simulationStartTime);
  }
}
