[![](https://img.shields.io/nuget/v/soenneker.extensions.datetimeoffsets.quarters.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.datetimeoffsets.quarters/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.datetimeoffsets.quarters/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.datetimeoffsets.quarters/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.extensions.datetimeoffsets.quarters.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.datetimeoffsets.quarters/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.datetimeoffsets.quarters/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.datetimeoffsets.quarters/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.DateTimeOffsets.Quarters
A collection of helpful DateTimeOffset quarter extension methods.

## Installation

```bash
dotnet add package Soenneker.Extensions.DateTimeOffsets.Quarters
```

## Quick start

```csharp
using Soenneker.Extensions.DateTimeOffsets.Quarters;

DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
var result = dateTimeOffset.ToStartOfQuarter();
```

## Common operations

- `ToStartOfQuarter()` - Returns the start of the quarter containing `dateTimeOffset`. No time zone conversion is performed and the offset is preserved.
- `ToEndOfQuarter()` - Returns the end of the quarter containing `dateTimeOffset`.
- `ToStartOfNextQuarter()` - Returns the start of the next quarter relative to `dateTimeOffset`. No time zone conversion is performed and the offset is preserved.
- `ToStartOfPreviousQuarter()` - Returns the start of the previous quarter relative to `dateTimeOffset`. No time zone conversion is performed and the offset is preserved.
- `ToEndOfNextQuarter()` - Returns the end of the next quarter relative to `dateTimeOffset`.
- `ToEndOfPreviousQuarter()` - Returns the end of the previous quarter relative to `dateTimeOffset`.
- `ToStartOfTzQuarter()` - Computes the start of the quarter in `tz` that contains the instant `utcInstant`, returning the result as a UTC `DateTimeOffset`. This computes the boundary as a local wall time (00:00 on the quarter start date) and maps it to UTC using the time zone's rules at that wall time (DST-safe).
- `ToEndOfTzQuarter()` - Computes the end of the quarter in `tz` that contains the instant `utcInstant`, returning the result as a UTC `DateTimeOffset`.
- `ToStartOfNextTzQuarter()` - Computes the start of the next quarter in `tz` relative to the instant `utcInstant`, returning the result as a UTC `DateTimeOffset`.
- `ToStartOfPreviousTzQuarter()` - Computes the start of the previous quarter in `tz` relative to the instant `utcInstant`, returning the result as a UTC `DateTimeOffset`.
- `ToEndOfNextTzQuarter()` - Computes the end of the next quarter in `tz` relative to the instant `utcInstant`, returning the result as a UTC `DateTimeOffset`.
- `ToEndOfPreviousTzQuarter()` - Computes the end of the previous quarter in `tz` relative to the instant `utcInstant`, returning the result as a UTC `DateTimeOffset`.
