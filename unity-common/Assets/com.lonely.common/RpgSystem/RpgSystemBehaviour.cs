using System.Collections.Generic;
using com.lonely.common.RpgSystem.State;
using com.lonely.common.System;

namespace com.lonely.common.RpgSystem
{
  public class RpgSystemBehaviour : SystemBehaviour<RpgState, RpgSystemLocationFinderBehaviour>
  {
    public override RpgState InitState(SystemBus systemBus)
    {
      return new RpgState(0);
    }
  }
}