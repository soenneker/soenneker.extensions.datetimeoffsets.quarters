using System;
using AwesomeAssertions;
using Soenneker.Tests.Unit;

namespace Soenneker.Extensions.DateTimeOffsets.Quarters.Tests;

public sealed class DateTimeOffsetsQuartersExtensionTests : UnitTest
{
    [Test]
    public void Offset_preserving_boundaries_use_calendar_quarters()
    {
        var value = new DateTimeOffset(2024, 5, 20, 14, 30, 0, TimeSpan.FromHours(9));

        value.ToStartOfQuarter().Should().Be(new DateTimeOffset(2024, 4, 1, 0, 0, 0, TimeSpan.FromHours(9)));
        value.ToEndOfQuarter().AddTicks(1).Should().Be(new DateTimeOffset(2024, 7, 1, 0, 0, 0, TimeSpan.FromHours(9)));
        value.ToStartOfPreviousQuarter().Should().Be(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.FromHours(9)));
        value.ToStartOfNextQuarter().Should().Be(new DateTimeOffset(2024, 7, 1, 0, 0, 0, TimeSpan.FromHours(9)));
    }

    [Test]
    public void Tz_boundaries_recalculate_the_offset_at_each_local_quarter()
    {
        TimeZoneInfo eastern = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Eastern Standard Time" : "America/New_York");
        var februaryInstant = new DateTimeOffset(2024, 2, 15, 12, 0, 0, TimeSpan.Zero);

        februaryInstant.ToStartOfTzQuarter(eastern)
                       .Should().Be(new DateTimeOffset(2024, 1, 1, 5, 0, 0, TimeSpan.Zero));
        februaryInstant.ToStartOfNextTzQuarter(eastern)
                       .Should().Be(new DateTimeOffset(2024, 4, 1, 4, 0, 0, TimeSpan.Zero));
        februaryInstant.ToEndOfTzQuarter(eastern).AddTicks(1)
                       .Should().Be(new DateTimeOffset(2024, 4, 1, 4, 0, 0, TimeSpan.Zero));
    }

    [Test]
    public void Tz_boundary_advances_through_a_midnight_gap()
    {
        TimeZoneInfo timeZone = CreateQuarterBoundaryGapTimeZone();
        var marchInstant = new DateTimeOffset(2025, 3, 15, 12, 0, 0, TimeSpan.Zero);

        DateTimeOffset result = marchInstant.ToStartOfNextTzQuarter(timeZone);
        DateTime localResult = TimeZoneInfo.ConvertTimeFromUtc(result.UtcDateTime, timeZone);

        localResult.Should().Be(new DateTime(2025, 4, 1, 1, 0, 0, DateTimeKind.Unspecified));
    }

    private static TimeZoneInfo CreateQuarterBoundaryGapTimeZone()
    {
        TimeZoneInfo.TransitionTime transitionStart = TimeZoneInfo.TransitionTime.CreateFixedDateRule(
            new DateTime(1, 1, 1, 0, 0, 0), 4, 1);
        TimeZoneInfo.TransitionTime transitionEnd = TimeZoneInfo.TransitionTime.CreateFixedDateRule(
            new DateTime(1, 1, 1, 0, 0, 0), 10, 1);
        TimeZoneInfo.AdjustmentRule rule = TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(
            new DateTime(2025, 1, 1), new DateTime(2025, 12, 31), TimeSpan.FromHours(1), transitionStart, transitionEnd);

        return TimeZoneInfo.CreateCustomTimeZone(
            "QuarterBoundaryGap", TimeSpan.Zero, "Quarter boundary gap", "Standard", "Daylight", [rule]);
    }
}
