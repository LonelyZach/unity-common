using com.lonely.common.System.State;

namespace com.lonely.common.System.Simulation
{
  public abstract record SimulationStep<TStateRoot>() where TStateRoot : RootStateEntity
  {
    public abstract void Run(TStateRoot state, int step);
  }
}
