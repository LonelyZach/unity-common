using Tests.RpgSystemTests.Sample.Components;
using Tests.RpgSystemTests.Sample.Entities;

namespace Tests.RpgSystemTests.Sample.Statuses
{
  internal record StatusContext(SampleState State, Character Character, Stack Stack)
  {
  }
}
