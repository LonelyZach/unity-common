using System.Collections.Generic;
using System.Linq;
using System.State;

namespace System.Simulation
{
  public class Simulation<TStateRoot> where TStateRoot : RootStateEntity<TStateRoot>
  {
    private IList<SimulationStep<TStateRoot>> _simulationSteps = new List<SimulationStep<TStateRoot>> { };

    public TStateRoot State { get; private set; }

    public Simulation(TStateRoot state)
    {
      State = state;
    }

    public Simulation<TStateRoot> DeepCopy()
    {
      var state = State.DeepCopy();
      var simulation = new Simulation<TStateRoot>(state);
      simulation._simulationSteps = _simulationSteps.Select(x => x).ToList();
      return simulation;
    }

    public bool Step(int step)
    {
      var simulateNow = State.SetStep(step);
      if (!simulateNow) return false;
      
      foreach (var simulationStep in _simulationSteps)
      {
        simulationStep.Run(State, step);
      }

      return true;
    }

    public void Register(SimulationStep<TStateRoot> simulationStep)
    {
      _simulationSteps.Add(simulationStep);
    }
  }
}
