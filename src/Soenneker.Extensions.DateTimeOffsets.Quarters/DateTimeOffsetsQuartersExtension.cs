using Soenneker.Enums.UnitOfTime;
using System;
using System.Diagnostics.Contracts;

namespace Soenneker.Extensions.DateTimeOffsets.Quarters;

/// <summary>
/// Provides extension methods for <see cref="DateTimeOffset"/> that operate on quarter boundaries,
/// including helpers that compute quarter starts/ends in a specified time zone while returning UTC instants.
/// </summary>
public static class DateTimeOffsetsQuartersExtension
{
    /// <summary>
    /// Returns the start of the quarter containing <paramref name="dateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The value to adjust.</param>
    /// <returns>
    /// A <see cref="DateTimeOffset"/> representing the first moment of the quarter containing <paramref name="dateTimeOffset"/>.
    /// </returns>
    /// <remarks>
    /// No time zone conversion is performed and the offset is preserved. Quarter boundaries are Jan/Apr/Jul/Oct 1 at 00:00:00.
    /// </remarks>
    [Pure]
    public static DateTimeOffset ToStartOfQuarter(this DateTimeOffset dateTimeOffset) =>
        dateTimeOffset.Trim(UnitOfTime.Quarter); // delegates to Trim(Quarter)

    /// <summary>
    /// Returns the end of the quarter containing <paramref name="dateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The value to adjust.</param>
    /// <returns>
    /// A <see cref="DateTimeOffset"/> representing the last tick of the quarter containing <paramref name="dateTimeOffset"/>.
    /// </returns>
    /// <remarks>
    /// Computed as one tick before the start of the next quarter. No time zone conversion is performed and the offset is preserved.
    /// </remarks>
    [Pure]
    public static DateTimeOffset ToEndOfQuarter(this DateTimeOffset dateTimeOffset) =>
        dateTimeOffset.TrimEnd(UnitOfTime.Quarter); // delegates to TrimEnd(Quarter)

    /// <summary>
    /// Returns the start of the next quarter relative to <paramref name="dateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The value to adjust.</param>
    /// <returns>A <see cref="DateTimeOffset"/> representing the first moment of the next quarter.</returns>
    /// <remarks>No time zone conversion is performed and the offset is preserved.</remarks>
    [Pure]
    public static DateTimeOffset ToStartOfNextQuarter(this DateTimeOffset dateTimeOffset) =>
        dateTimeOffset.ToStartOfQuarter()
                      .AddMonths(3);

    /// <summary>
    /// Returns the start of the previous quarter relative to <paramref name="dateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The value to adjust.</param>
    /// <returns>A <see cref="DateTimeOffset"/> representing the first moment of the previous quarter.</returns>
    /// <remarks>No time zone conversion is performed and the offset is preserved.</remarks>
    [Pure]
    public static DateTimeOffset ToStartOfPreviousQuarter(this DateTimeOffset dateTimeOffset) =>
        dateTimeOffset.ToStartOfQuarter()
                      .AddMonths(-3);

    /// <summary>
    /// Returns the end of the next quarter relative to <paramref name="dateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The value to adjust.</param>
    /// <returns>A <see cref="DateTimeOffset"/> representing the last tick of the next quarter.</returns>
    /// <remarks>
    /// Computed as one tick before the start of the quarter after next. No time zone conversion is performed and the offset is preserved.
    /// </remarks>
    [Pure]
    public static DateTimeOffset ToEndOfNextQuarter(this DateTimeOffset dateTimeOffset) =>
        dateTimeOffset.ToStartOfQuarter()
                      .AddMonths(6)
                      .AddTicks(-1);

    /// <summary>
    /// Returns the end of the previous quarter relative to <paramref name="dateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The value to adjust.</param>
    /// <returns>A <see cref="DateTimeOffset"/> representing the last tick of the previous quarter.</returns>
    /// <remarks>
    /// Computed as one tick before the start of the current quarter. No time zone conversion is performed and the offset is preserved.
    /// </remarks>
    [Pure]
    public static DateTimeOffset ToEndOfPreviousQuarter(this DateTimeOffset dateTimeOffset) =>
        dateTimeOffset.ToStartOfQuarter()
                      .AddTicks(-1);

    /// <summary>
    /// Computes the start of the quarter in <paramref name="tz"/> that contains the instant <paramref name="utcInstant"/>,
    /// returning the result as a UTC <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="utcInstant">
    /// An instant in time. It is normalized to UTC before conversion and treated as an instant (not a local wall time).
    /// </param>
    /// <param name="tz">The time zone whose local calendar rules determine quarter boundaries.</param>
    /// <returns>
    /// A UTC <see cref="DateTimeOffset"/> representing the start of the quarter in <paramref name="tz"/> that contains <paramref name="utcInstant"/>.
    /// </returns>
    /// <remarks>
    /// This computes the boundary as a local wall time (00:00 on the quarter start date) and maps it to UTC using the time zone's
    /// rules at that wall time (DST-safe).
    /// </remarks>
    [Pure]
    public static DateTimeOffset ToStartOfTzQuarter(this DateTimeOffset utcInstant, TimeZoneInfo tz)
    {
        DateTimeOffset utc = utcInstant.ToUniversalTime();
        DateTimeOffset local = TimeZoneInfo.ConvertTime(utc, tz);

        int startMonth = (local.Month - 1) / 3 * 3 + 1;

        // Quarter start as local wall-clock time.
        DateTime localStart = new(local.Year, startMonth, 1, 0, 0, 0, DateTimeKind.Unspecified);

        DateTime utcStart = TimeZoneInfo.ConvertTimeToUtc(localStart, tz);
        return new DateTimeOffset(utcStart, TimeSpan.Zero);
    }

    /// <summary>
    /// Computes the end of the quarter in <paramref name="tz"/> that contains the instant <paramref name="utcInstant"/>,
    /// returning the result as a UTC <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="utcInstant">
    /// An instant in time. It is normalized to UTC before conversion and treated as an instant (not a local wall time).
    /// </param>
    /// <param name="tz">The time zone whose local calendar rules determine quarter boundaries.</param>
    /// <returns>A UTC <see cref="DateTimeOffset"/> representing the last tick of the quarter in <paramref name="tz"/>.</returns>
    /// <remarks>
    /// Computed as one tick before the start of the next quarter in <paramref name="tz"/> (DST-safe).
    /// </remarks>
    [Pure]
    public static DateTimeOffset ToEndOfTzQuarter(this DateTimeOffset utcInstant, TimeZoneInfo tz) =>
        utcInstant.ToStartOfTzQuarter(tz)
                  .AddMonths(3)
                  .AddTicks(-1);

    /// <summary>
    /// Computes the start of the next quarter in <paramref name="tz"/> relative to the instant <paramref name="utcInstant"/>,
    /// returning the result as a UTC <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="utcInstant">
    /// An instant in time. It is normalized to UTC before conversion and treated as an instant (not a local wall time).
    /// </param>
    /// <param name="tz">The time zone whose local calendar rules determine quarter boundaries.</param>
    /// <returns>A UTC <see cref="DateTimeOffset"/> representing the start of the next quarter in <paramref name="tz"/>.</returns>
    [Pure]
    public static DateTimeOffset ToStartOfNextTzQuarter(this DateTimeOffset utcInstant, TimeZoneInfo tz) =>
        utcInstant.ToStartOfTzQuarter(tz)
                  .AddMonths(3);

    /// <summary>
    /// Computes the start of the previous quarter in <paramref name="tz"/> relative to the instant <paramref name="utcInstant"/>,
    /// returning the result as a UTC <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="utcInstant">
    /// An instant in time. It is normalized to UTC before conversion and treated as an instant (not a local wall time).
    /// </param>
    /// <param name="tz">The time zone whose local calendar rules determine quarter boundaries.</param>
    /// <returns>A UTC <see cref="DateTimeOffset"/> representing the start of the previous quarter in <paramref name="tz"/>.</returns>
    [Pure]
    public static DateTimeOffset ToStartOfPreviousTzQuarter(this DateTimeOffset utcInstant, TimeZoneInfo tz) =>
        utcInstant.ToStartOfTzQuarter(tz)
                  .AddMonths(-3);

    /// <summary>
    /// Computes the end of the next quarter in <paramref name="tz"/> relative to the instant <paramref name="utcInstant"/>,
    /// returning the result as a UTC <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="utcInstant">
    /// An instant in time. It is normalized to UTC before conversion and treated as an instant (not a local wall time).
    /// </param>
    /// <param name="tz">The time zone whose local calendar rules determine quarter boundaries.</param>
    /// <returns>A UTC <see cref="DateTimeOffset"/> representing the last tick of the next quarter in <paramref name="tz"/>.</returns>
    /// <remarks>
    /// Computed as one tick before the start of the quarter after next in <paramref name="tz"/> (DST-safe).
    /// </remarks>
    [Pure]
    public static DateTimeOffset ToEndOfNextTzQuarter(this DateTimeOffset utcInstant, TimeZoneInfo tz) =>
        utcInstant.ToStartOfTzQuarter(tz)
                  .AddMonths(6)
                  .AddTicks(-1);

    /// <summary>
    /// Computes the end of the previous quarter in <paramref name="tz"/> relative to the instant <paramref name="utcInstant"/>,
    /// returning the result as a UTC <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="utcInstant">
    /// An instant in time. It is normalized to UTC before conversion and treated as an instant (not a local wall time).
    /// </param>
    /// <param name="tz">The time zone whose local calendar rules determine quarter boundaries.</param>
    /// <returns>A UTC <see cref="DateTimeOffset"/> representing the last tick of the previous quarter in <paramref name="tz"/>.</returns>
    /// <remarks>
    /// Computed as one tick before the start of the current quarter in <paramref name="tz"/> (DST-safe).
    /// </remarks>
    [Pure]
    public static DateTimeOffset ToEndOfPreviousTzQuarter(this DateTimeOffset utcInstant, TimeZoneInfo tz) =>
        utcInstant.ToStartOfTzQuarter(tz)
                  .AddTicks(-1);
}