using com.lonely.common.EcsSystem;
using Tests.RpgSystemTests.Sample.Statuses;

namespace Tests.RpgSystemTests.Sample.Components
{
  internal record Stack(Status Status) : Component
  {
    public int Magnitude { get; set; }
  }
}
