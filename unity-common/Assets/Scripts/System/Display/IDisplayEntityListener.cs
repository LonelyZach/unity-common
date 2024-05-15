using System.State;
using UnityEngine;

namespace System.Display
{
  public interface IDisplayEntityListener<TState, TStateEntity, TLocationFinder>
    where TState : RootStateEntity<TState>
    where TLocationFinder : LocationFinderBehaviour
  {
    public void Init(TState state, object stateEntity, TLocationFinder locationFinder, float simulationStartTime);
    public void Sync(TState state, object stateEntity, TLocationFinder locationFinder, float simulationStartTime);
  }
}
