[![Build & Test](https://github.com/elminalirzayev/Easy.Tools.Finance.TCMB/actions/workflows/build.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Tools.Finance.TCMB/actions/workflows/build.yml)
[![Build & Release](https://github.com/elminalirzayev/Easy.Tools.Finance.TCMB/actions/workflows/release.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Tools.Finance.TCMB/actions/workflows/release.yml)
[![Build & Nuget Publish](https://github.com/elminalirzayev/Easy.Tools.Finance.TCMB/actions/workflows/nuget.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Tools.Finance.TCMB/actions/workflows/nuget.yml)
[![Release](https://img.shields.io/github/v/release/elminalirzayev/Easy.Tools.Finance.TCMB)](https://github.com/elminalirzayev/Easy.Tools.Finance.TCMB/releases)
[![License](https://img.shields.io/github/license/elminalirzayev/Easy.Tools.Finance.TCMB)](https://github.com/elminalirzayev/Easy.Tools.Finance.TCMB/blob/master/LICENSE.txt)
[![NuGet](https://img.shields.io/nuget/v/Easy.Tools.Finance.TCMB.svg)](https://www.nuget.org/packages/Easy.Tools.Finance.TCMB)

#Easy.Tools.Finance.TCMB

**Easy.Tools.Finance.TCMB** is a high-performance, enterprise-ready .NET library designed to fetch official exchange rates from the **Central Bank of the Republic of Turkey (TCMB/CBRT)**.

It supports fetching both **daily** and **historical** rates, handles XML parsing efficiently, and includes built-in resilience mechanisms.


##  Features

- ** High Performance:** Uses **Static XML Serializer** caching to minimize memory pressure and prevent memory leaks.
- ** Historical Data:** Built-in support for fetching rates from past dates (automatically handles TCMB's archive URL structure).
- ** Resilience:** Built-in **Retry Policy** with exponential backoff for handling network glitches.
- ** Culture Safe:** Parsing logic is strictly **Invariant Culture**, ensuring stability regardless of the server's regional settings.
- ** Easy Integration:** Single-line integration with `IServiceCollection` (Dependency Injection).
- ** Async & Cancellable:** Full support for `async/await` and `CancellationToken`.


## Installation

Install via NuGet Package Manager:

```powershell
Install-Package Easy.Tools.Finance.TCMB
```

Or via .NET CLI:

```bash
dotnet add package Easy.Tools.Finance.TCMB
```

## Usage

### 1. Service Registration (Program.cs)

Register the client in your `Program.cs`.

```csharp
using Easy.Tools.Finance.TCMB;

var builder = WebApplication.CreateBuilder(args);

// 1. Standard Registration
builder.Services.AddTcmbClient();

// 2. OR: Advanced Configuration
builder.Services.AddTcmbClient(options => 
{
    options.RetryCount = 3;             // Retry 3 times
    options.RetryDelaySeconds = 2;      // Wait 2 seconds
    // options.BaseUrl = "...";         // Optional: Use a proxy URL
});

var app = builder.Build();
```

### 2. Fetching Rates (Controller Example)

Inject `ITcmbClient` into your controllers.

```csharp
using Easy.Tools.Finance.TCMB;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CurrencyController : ControllerBase
{
    private readonly ITcmbClient _tcmbClient;

    public CurrencyController(ITcmbClient tcmbClient)
    {
        _tcmbClient = tcmbClient;
    }

    [HttpGet("today")]
    public async Task<IActionResult> GetTodayRates(CancellationToken cancellationToken)
    {
        // Fetch today's rates
        var rates = await _tcmbClient.GetTodayRatesAsync(cancellationToken);

        // Get USD and EUR
        var usd = rates.FirstOrDefault(x => x.Code == "USD");
        var eur = rates.FirstOrDefault(x => x.Code == "EUR");

        if (usd != null)
            Console.WriteLine($"USD Selling: {usd.ForexSelling}");

        return Ok(rates);
    }

    [HttpGet("history/{date}")]
    public async Task<IActionResult> GetHistoryRates(DateTime date, CancellationToken cancellationToken)
    {
        // Fetch historical rates (e.g., 2023-05-15)
        // The library automatically formats the URL to: .../202305/15052023.xml
        var rates = await _tcmbClient.GetRatesByDateAsync(date, cancellationToken);

        return Ok(rates);
    }
}
```


## Models

The package returns a list of `TcmbCurrency` objects.

| Property | Type | Description |
| --- | --- | --- |
| `Code` | `string` | Currency code (e.g., `USD`, `EUR`). |
| `CurrencyName` | `string` | English name (e.g., `US DOLLAR`). |
| `Name` | `string` | Turkish name (e.g., `ABD DOLARI`). |
| `Unit` | `int` | Unit amount (e.g. `1` or `100`). |
| `ForexBuying` | `decimal` | Forex (Döviz) buying rate. |
| `ForexSelling` | `decimal` | Forex (Döviz) selling rate. |
| `BanknoteBuying` | `decimal` | Banknote (Efektif) buying rate. |
| `BanknoteSelling` | `decimal` | Banknote (Efektif) selling rate. |


---

## Contributing

Contributions and suggestions are welcome. Please open an issue or submit a pull request.

---

## License

This project is licensed under the MIT License.

---

© 2025 Elmin Alirzayev / Easy Code Tools