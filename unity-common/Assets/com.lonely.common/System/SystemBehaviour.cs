using System;
using System.Collections.Generic;
using com.lonely.common.Messaging;
using com.lonely.common.System.Display;
using com.lonely.common.System.Simulation;
using com.lonely.common.System.State;
using UnityEngine;

namespace com.lonely.common.System
{
  public abstract class SystemBehaviour<TState, TLocationFinder> : MonoBehaviour, ISystemBehaviour
    where TState : RootStateEntity
    where TLocationFinder : LocationFinderBehaviour
  {
    public DisplayEntityBase<TLocationFinder>[] DisplayEntityPrefabs;

    public TLocationFinder LocationFinder;

    protected bool IsStarted { get; private set; }

    public System<TState> System;
    private DisplayEntityTracker<TState, TLocationFinder> _displayEntityTracker;

    private readonly IList<Action<System<TState>>> _messageHandlerRegistrations = new List<Action<System<TState>>>();

    private float _systemStartTime;

    public void Start()
    {
      if (IsStarted)
      {
        return;
      }

      var systemBus = new SystemBus(this);
      var state = InitState(systemBus);
      state.SimulateNow("Init");
      System = new System<TState>(state, systemBus);
      foreach (var step in InitSimulation())
      {
        System.Simulation.Register(step);
      }

      _displayEntityTracker = new DisplayEntityTracker<TState, TLocationFinder>(DisplayEntityPrefabs, LocationFinder);
      foreach (var registration in _messageHandlerRegistrations)
      {
        registration.Invoke(System);
      }

      _systemStartTime = Time.time;
      IsStarted = true;
    }

    public void OnDestroy()
    {
      System?.Destroy();
    }

    public void Update()
    {
      var elapsedTime = Time.time - _systemStartTime;
      var targetStep = (int)(elapsedTime * Config.StepsEachSecond);

      var simulated = System.Step(targetStep);
      if (simulated)
      {
        _displayEntityTracker.Render(System.GetState(), System.GetBus(), _systemStartTime);
      }

      AfterUpdate();
    }

    public virtual void AfterUpdate()
    {

    }

    public void Handler<TMessage>(Action<TState, TMessage> handler) where TMessage : Message
    {
      Handler(handler, null);
    }

    public void Handler<TMessage>(Action<TState, TMessage> handler, Func<TState, TMessage, bool> predicate) where TMessage : Message
    {
      var wrappedHandler = new Action<TMessage>(x => {
        handler(System.GetState(), x);
        System.GetState().SimulateNow("Message handled.");
      });
      var wrappedPredicate = predicate == null ? null : new Func<TMessage, bool>(x => predicate(System.GetState(), x));
      var registration = new Action<System<TState>>(x => GlobalBus.RegisterHandler(System, wrappedHandler, wrappedPredicate));
      _messageHandlerRegistrations.Add(registration);
    }

    public void Dispatch<TMessage>(TMessage message) where TMessage : Message
    {
      System.Dispatch(message);
    }

    public abstract TState InitState(SystemBus systemBus);

    public virtual IList<SimulationStep<TState>> InitSimulation()
    {
      return new List<SimulationStep<TState>>();
    }
  }
}
