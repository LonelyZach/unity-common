using com.lonely.common.System.State;

namespace com.lonely.common.System.Display
{
  public interface IDisplayEntityListener<TState, TStateEntity, TLocationFinder>
    where TState : RootStateEntity
    where TLocationFinder : LocationFinderBehaviour
  {
    public void Init(TState state, TStateEntity stateEntity, TLocationFinder locationFinder, float simulationStartTime);
    public void Sync(TState state, TStateEntity stateEntity, TLocationFinder locationFinder, float simulationStartTime);
  }
}
