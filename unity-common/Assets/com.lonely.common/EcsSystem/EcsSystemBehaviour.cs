using com.lonely.common.System;
using com.lonely.common.System.State;

namespace com.lonely.common.EcsSystem
{
  public abstract class EcsSystemBehaviour<TState, TLocationFinder> : SystemBehaviour<TState, TLocationFinder>
    where TState : RootStateEntity
    where TLocationFinder : LocationFinderBehaviour
  {
  }
}