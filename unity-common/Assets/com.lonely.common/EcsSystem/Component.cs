using System;

namespace com.lonely.common.EcsSystem
{
  public abstract record Component
  {
    public virtual int? SimulateAt => null;

    public Guid Id { get; init; } = Guid.NewGuid();

    public virtual Component DeepCopy()
    {
      return this with { };
    }
  }

  public abstract record DataComponent<TData>(TData Data) : Component
    where TData : ComponentData
  {
    public override Component DeepCopy()
    {
      return this with { Data = (TData)Data.DeepCopy() };
    }
  }

  public abstract record ComponentData
  {
    public virtual ComponentData DeepCopy()
    {
      return this with { };
    }
  }
}