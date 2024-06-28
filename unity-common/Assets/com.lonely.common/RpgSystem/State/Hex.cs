using System.Collections.Generic;
using System.Linq;
using com.lonely.common.System.State;

namespace com.lonely.common.RpgSystem.State
{
  public class Hex : StateEntity
  {
    public Hex(int x, int y)
    {
      X = x;
      Y = y;
    }

    public int X { get; }
    public int Y { get; }
    
    public override string Key => $"Hex_{X}_{Y}";
    
    public override StateEntity DeepCopy()
    {
      return new Hex(X, Y);
    }

    public override IEnumerable<IStateEntity> GetChildren()
    {
      return Enumerable.Empty<IStateEntity>();
    }
  }
}