using com.lonely.common.EcsSystem;
using Tests.RpgSystemTests.Sample.Components;
using Tests.RpgSystemTests.Sample.Entities;

namespace Tests.RpgSystemTests.Sample.Statuses
{
  internal abstract class Status
  {
    public abstract string Name { get; }
  }
}
