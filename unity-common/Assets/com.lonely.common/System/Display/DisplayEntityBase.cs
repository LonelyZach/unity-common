using System;
using com.lonely.common.System.State;
using UnityEngine;

namespace com.lonely.common.System.Display
{
  public abstract class DisplayEntityBase<TLocationFinder> : MonoBehaviour, IDisplayEntity<TLocationFinder>
    where TLocationFinder : LocationFinderBehaviour
  {
    public abstract Type StateEntityType { get; }

    public bool Orphaned { get; set; }

    public IDisplayEntity<TLocationFinder> Instantiate(TLocationFinder locationFinder)
    {
      var parent = InitParentSelector(locationFinder);
      return Instantiate(this, parent);
    }

    public abstract void InitUnsafe(IStateEntity state, object stateEntity, TLocationFinder locationFinder, float simulationStartTime, SystemBus bus);

    public abstract void SyncUnsafe(IStateEntity state, object stateEntity, TLocationFinder locationFinder, float simulationStartTime);

    protected virtual Transform InitParentSelector(TLocationFinder locationFinder)
    {
      return null;
    }

    public void Destroy()
    {
      Destroy(gameObject);
    }
  }
}
