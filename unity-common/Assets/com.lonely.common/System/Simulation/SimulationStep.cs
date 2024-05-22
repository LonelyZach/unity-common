using System.State;

namespace System.Simulation
{
  public abstract record SimulationStep<TStateRoot>() where TStateRoot : RootStateEntity<TStateRoot>
  {
    public abstract void Run(TStateRoot state, int step);
  }
}
