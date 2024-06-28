using com.lonely.common.System.State;

namespace com.lonely.common.System.Display
{
  public interface IDisplayEntity<TLocationFinder>
    where TLocationFinder : LocationFinderBehaviour
  {
    bool Orphaned { get; set; }

    IDisplayEntity<TLocationFinder> Instantiate(TLocationFinder locationFinder);

    void InitUnsafe(IStateEntity state, object stateEntity, TLocationFinder locationFinder, float simulationStartTime, SystemBus bus);

    void SyncUnsafe(IStateEntity state, object stateEntity, TLocationFinder locationFinder, float simulationStartTime);

    void Destroy();
  }
}
