using System;
using System.Diagnostics.Contracts;
using Soenneker.Enums.UnitOfTime;

namespace Soenneker.Extensions.DateTimeOffsets.Quarters;

/// <summary>
/// Provides extension methods for <see cref="DateTimeOffset"/> that operate on quarter boundaries,
/// including helpers that compute quarter starts/ends in a specified time zone while returning UTC instants.
/// </summary>
public static class DateTimeOffsetsQuartersExtension
{
    /// <summary>
    /// Returns the start of the quarter containing <paramref name="dateTimeOffset"/> using the extension library's quarter definition.
    /// </summary>
    /// <param name="dateTimeOffset">The value to adjust.</param>
    /// <returns>A <see cref="DateTimeOffset"/> representing the first moment of the quarter containing <paramref name="dateTimeOffset"/>.</returns>
    /// <remarks>
    /// This delegates to <c>ToStartOf(UnitOfTime.Quarter)</c>. Ensure your <c>ToStartOf</c>/<c>Trim</c> implementation for
    /// <see cref="UnitOfTime.Quarter"/> matches the desired rule (e.g., quarter begins on Jan/Apr/Jul/Oct 1 at 00:00:00).
    /// </remarks>
    [Pure]
    public static DateTimeOffset ToStartOfQuarter(this DateTimeOffset dateTimeOffset) =>
        dateTimeOffset.ToStartOf(UnitOfTime.Quarter);

    /// <summary>
    /// Returns the start of the next quarter relative to <paramref name="dateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The value to adjust.</param>
    /// <returns>A <see cref="DateTimeOffset"/> representing the first moment of the next quarter.</returns>
    /// <remarks>No time zone conversion is performed.</remarks>
    [Pure]
    public static DateTimeOffset ToStartOfNextQuarter(this DateTimeOffset dateTimeOffset) =>
        dateTimeOffset.ToStartOfQuarter()
                      .AddMonths(3);

    /// <summary>
    /// Returns the start of the previous quarter relative to <paramref name="dateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The value to adjust.</param>
    /// <returns>A <see cref="DateTimeOffset"/> representing the first moment of the previous quarter.</returns>
    /// <remarks>No time zone conversion is performed.</remarks>
    [Pure]
    public static DateTimeOffset ToStartOfPreviousQuarter(this DateTimeOffset dateTimeOffset) =>
        dateTimeOffset.ToStartOfQuarter()
                      .AddMonths(-3);

    /// <summary>
    /// Returns the end of the quarter containing <paramref name="dateTimeOffset"/> using the extension library's quarter definition.
    /// </summary>
    /// <param name="dateTimeOffset">The value to adjust.</param>
    /// <returns>A <see cref="DateTimeOffset"/> representing the last tick of the quarter containing <paramref name="dateTimeOffset"/>.</returns>
    /// <remarks>
    /// This delegates to <c>ToEndOf(UnitOfTime.Quarter)</c>, which is typically defined as one tick before the start of the next quarter.
    /// </remarks>
    [Pure]
    public static DateTimeOffset ToEndOfQuarter(this DateTimeOffset dateTimeOffset) =>
        dateTimeOffset.ToEndOf(UnitOfTime.Quarter);

    /// <summary>
    /// Returns the end of the next quarter relative to <paramref name="dateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The value to adjust.</param>
    /// <returns>A <see cref="DateTimeOffset"/> representing the last tick of the next quarter.</returns>
    /// <remarks>This is computed as the end of the current quarter plus 3 months.</remarks>
    [Pure]
    public static DateTimeOffset ToEndOfNextQuarter(this DateTimeOffset dateTimeOffset) =>
        dateTimeOffset.ToEndOfQuarter()
                      .AddMonths(3);

    /// <summary>
    /// Returns the end of the previous quarter relative to <paramref name="dateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The value to adjust.</param>
    /// <returns>A <see cref="DateTimeOffset"/> representing the last tick of the previous quarter.</returns>
    /// <remarks>This is computed as the end of the current quarter minus 3 months.</remarks>
    [Pure]
    public static DateTimeOffset ToEndOfPreviousQuarter(this DateTimeOffset dateTimeOffset) =>
        dateTimeOffset.ToEndOfQuarter()
                      .AddMonths(-3);

    /// <summary>
    /// Computes the start of the quarter in <paramref name="tz"/> that contains the instant <paramref name="utcInstant"/>,
    /// returning the result as a UTC <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="utcInstant">An instant in time. It is treated as a UTC instant (any offset is normalized to UTC).</param>
    /// <param name="tz">The time zone whose local calendar rules determine quarter boundaries.</param>
    /// <returns>
    /// A UTC <see cref="DateTimeOffset"/> representing the start of the target time zone's quarter containing <paramref name="utcInstant"/>.
    /// </returns>
    /// <remarks>
    /// This converts <paramref name="utcInstant"/> into <paramref name="tz"/>, trims to quarter start in that zone, then returns the same instant in UTC.
    /// </remarks>
    [Pure]
    public static DateTimeOffset ToStartOfTzQuarter(this DateTimeOffset utcInstant, TimeZoneInfo tz) =>
        utcInstant.ToUniversalTime()
                  .ToTz(tz)
                  .ToStartOfQuarter()
                  .ToUtc();

    /// <summary>
    /// Computes the start of the next quarter in <paramref name="tz"/> relative to the instant <paramref name="utcInstant"/>,
    /// returning the result as a UTC <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="utcInstant">An instant in time, treated as UTC.</param>
    /// <param name="tz">The time zone whose local calendar rules determine quarter boundaries.</param>
    /// <returns>A UTC <see cref="DateTimeOffset"/> representing the start of the next quarter in <paramref name="tz"/>.</returns>
    [Pure]
    public static DateTimeOffset ToStartOfNextTzQuarter(this DateTimeOffset utcInstant, TimeZoneInfo tz) =>
        utcInstant.ToUniversalTime()
                  .ToTz(tz)
                  .ToStartOfNextQuarter()
                  .ToUtc();

    /// <summary>
    /// Computes the start of the previous quarter in <paramref name="tz"/> relative to the instant <paramref name="utcInstant"/>,
    /// returning the result as a UTC <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="utcInstant">An instant in time, treated as UTC.</param>
    /// <param name="tz">The time zone whose local calendar rules determine quarter boundaries.</param>
    /// <returns>A UTC <see cref="DateTimeOffset"/> representing the start of the previous quarter in <paramref name="tz"/>.</returns>
    [Pure]
    public static DateTimeOffset ToStartOfPreviousTzQuarter(this DateTimeOffset utcInstant, TimeZoneInfo tz) =>
        utcInstant.ToUniversalTime()
                  .ToTz(tz)
                  .ToStartOfPreviousQuarter()
                  .ToUtc();

    /// <summary>
    /// Computes the end of the quarter in <paramref name="tz"/> that contains the instant <paramref name="utcInstant"/>,
    /// returning the result as a UTC <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="utcInstant">An instant in time, treated as UTC.</param>
    /// <param name="tz">The time zone whose local calendar rules determine quarter boundaries.</param>
    /// <returns>A UTC <see cref="DateTimeOffset"/> representing the last tick of the quarter in <paramref name="tz"/>.</returns>
    [Pure]
    public static DateTimeOffset ToEndOfTzQuarter(this DateTimeOffset utcInstant, TimeZoneInfo tz) =>
        utcInstant.ToUniversalTime()
                  .ToTz(tz)
                  .ToEndOfQuarter()
                  .ToUtc();

    /// <summary>
    /// Computes the end of the next quarter in <paramref name="tz"/> relative to the instant <paramref name="utcInstant"/>,
    /// returning the result as a UTC <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="utcInstant">An instant in time, treated as UTC.</param>
    /// <param name="tz">The time zone whose local calendar rules determine quarter boundaries.</param>
    /// <returns>A UTC <see cref="DateTimeOffset"/> representing the last tick of the next quarter in <paramref name="tz"/>.</returns>
    [Pure]
    public static DateTimeOffset ToEndOfNextTzQuarter(this DateTimeOffset utcInstant, TimeZoneInfo tz) =>
        utcInstant.ToUniversalTime()
                  .ToTz(tz)
                  .ToEndOfNextQuarter()
                  .ToUtc();

    /// <summary>
    /// Computes the end of the previous quarter in <paramref name="tz"/> relative to the instant <paramref name="utcInstant"/>,
    /// returning the result as a UTC <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="utcInstant">An instant in time, treated as UTC.</param>
    /// <param name="tz">The time zone whose local calendar rules determine quarter boundaries.</param>
    /// <returns>A UTC <see cref="DateTimeOffset"/> representing the last tick of the previous quarter in <paramref name="tz"/>.</returns>
    [Pure]
    public static DateTimeOffset ToEndOfPreviousTzQuarter(this DateTimeOffset utcInstant, TimeZoneInfo tz) =>
        utcInstant.ToUniversalTime()
                  .ToTz(tz)
                  .ToEndOfPreviousQuarter()
                  .ToUtc();
}