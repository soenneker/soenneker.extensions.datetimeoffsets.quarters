[![](https://img.shields.io/nuget/v/soenneker.extensions.datetimeoffsets.quarters.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.datetimeoffsets.quarters/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.datetimeoffsets.quarters/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.datetimeoffsets.quarters/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.extensions.datetimeoffsets.quarters.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.datetimeoffsets.quarters/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.datetimeoffsets.quarters/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.datetimeoffsets.quarters/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.DateTimeOffsets.Quarters

Calendar-quarter boundary extensions for `DateTimeOffset`, with offset-preserving and time-zone-aware variants.

Quarters begin on January 1, April 1, July 1, and October 1.

## Installation

```bash
dotnet add package Soenneker.Extensions.DateTimeOffsets.Quarters
```

## Offset-preserving boundaries

These methods use the calendar fields and offset already carried by the value. They do not apply time-zone rules.

```csharp
using Soenneker.Extensions.DateTimeOffsets.Quarters;

var value = new DateTimeOffset(2024, 5, 20, 14, 30, 0, TimeSpan.FromHours(-5));

DateTimeOffset start = value.ToStartOfQuarter();
// 2024-04-01 00:00:00 -05:00

DateTimeOffset next = value.ToStartOfNextQuarter();
// 2024-07-01 00:00:00 -05:00
```

The available methods are `ToStartOfQuarter()`, `ToEndOfQuarter()`, `ToStartOfPreviousQuarter()`, `ToEndOfPreviousQuarter()`, `ToStartOfNextQuarter()`, and `ToEndOfNextQuarter()`.

## Time-zone quarter boundaries

Use the `Tz` methods when the input represents an instant and a time zone determines which local quarter contains it. Results are UTC `DateTimeOffset` values with a zero offset.

```csharp
TimeZoneInfo eastern = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
var instant = new DateTimeOffset(2024, 2, 15, 12, 0, 0, TimeSpan.Zero);

DateTimeOffset currentStartUtc = instant.ToStartOfTzQuarter(eastern);
// 2024-01-01 05:00:00 +00:00

DateTimeOffset nextStartUtc = instant.ToStartOfNextTzQuarter(eastern);
// 2024-04-01 04:00:00 +00:00
```

The time-zone variants are `ToStartOfTzQuarter()`, `ToEndOfTzQuarter()`, `ToStartOfPreviousTzQuarter()`, `ToEndOfPreviousTzQuarter()`, `ToStartOfNextTzQuarter()`, and `ToEndOfNextTzQuarter()`.

End methods are inclusive and return one tick before the following quarter begins. Each boundary is calculated in the supplied time zone, so offset changes between quarters are respected. If a transition skips local midnight on a quarter boundary, the start resolves to the first valid local time.
