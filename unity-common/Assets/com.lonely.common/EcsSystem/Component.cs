namespace com.lonely.common.EcsSystem
{
  public abstract record Component
  {
    public virtual int? SimulateAt => null;
  }
}