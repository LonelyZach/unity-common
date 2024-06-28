using System.Collections.Generic;

namespace com.lonely.common.System.State
{
  public interface IStateEntity
  {
    public string Key { get; }

    public abstract IEnumerable<IStateEntity> GetChildren();

    public IEnumerable<object> GetDisplays();
  }
}
