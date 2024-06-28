using System;
using com.lonely.common.EcsSystem;

namespace Tests.RpgSystemTests.Sample.Components
{
  internal record Id(Guid Value) : Component
  {
  }
}
