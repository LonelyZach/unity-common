namespace com.lonely.common.System.State
{
  public interface IRootStateEntity : IStateEntity
  {
    int? NextSimulateAtStep { get; }
  }
}
