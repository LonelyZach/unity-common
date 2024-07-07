using System;

namespace com.lonely.common.System.State
{
  public record Timer
  {
    [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "We want to ensure only root state entities create timers.")]
    public Timer(int startStep, int endStep, string description)
    {
      StartStep = startStep;
      EndSetp = endStep;
      Description = description;
      Key = Guid.NewGuid().ToString();
    }
    
    public Timer(int startStep, int endStep, string description, string key)
    {
      StartStep = startStep;
      EndSetp = endStep;
      Description = description;
      Key = key;
    }

    public int StartStep { get; }
    public int EndSetp { get; }
    public string Description { get; }
    public string Key { get; }
  }
}
