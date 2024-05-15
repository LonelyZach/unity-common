namespace System.State
{
  public interface IRootStateEntity : IStateEntity
  {
    int? NextSimulateAtStep { get; }
  }
}
