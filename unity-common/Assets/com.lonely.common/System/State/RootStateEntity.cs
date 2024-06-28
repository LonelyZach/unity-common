using System;
using System.Collections.Generic;
using System.Linq;

namespace com.lonely.common.System.State
{
  public abstract class RootStateEntity : StateEntity, IRootStateEntity
  {
    protected RootStateEntity(int step)
    {
      Step = step;
    }

    public int Step { get; private set; }

    public int? NextSimulateAtStep => SimulateAtSteps.Any() ? SimulateAtSteps.First().Key : null;

    public SortedList<int, string> SimulateAtSteps { get; private set; } = new SortedList<int, string>();

    public bool SetStep(int step)
    {
      var simulateNow = false;
      Step = step;
      while (NextSimulateAtStep <= Step)
      {
        SimulateAtSteps.RemoveAt(0);
        simulateNow = true;
      }

      return simulateNow;
    }

    public Timer BuildTimer(int startStep, int endStep, string description)
    {
      return BuildTimer(startStep, endStep, description, Guid.NewGuid().ToString());
    }
    
    public Timer BuildTimer(int startStep, int endStep, string description, string key)
    {
      if (!SimulateAtSteps.ContainsKey(endStep))
      {
        SimulateAtSteps.Add(endStep, description);
      }

      return new Timer(this, startStep, endStep, description, key);
    }
    
    public void SimulateAt(int step, string description)
    {
      if (!SimulateAtSteps.ContainsKey(step))
      {
        SimulateAtSteps.Add(step, description);
      }
    }
    
    public void SimulateIn(int deltaSteps, string description)
    {
      var step = Step + deltaSteps;
      SimulateAt(step, description);
    }
    
    public void SimulateNow(string description)
    {
      SimulateAt(Step, description);
    }
  }
}
