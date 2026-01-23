[![Build & Test](https://github.com/elminalirzayev/Easy.Tools.Finance.TCMB/actions/workflows/build.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Tools.Finance.TCMB/actions/workflows/build.yml)
[![Build & Release](https://github.com/elminalirzayev/Easy.Tools.Finance.TCMB/actions/workflows/release.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Tools.Finance.TCMB/actions/workflows/release.yml)
[![Build & Nuget Publish](https://github.com/elminalirzayev/Easy.Tools.Finance.TCMB/actions/workflows/nuget.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Tools.Finance.TCMB/actions/workflows/nuget.yml)
[![Release](https://img.shields.io/github/v/release/elminalirzayev/Easy.Tools.Finance.TCMB)](https://github.com/elminalirzayev/Easy.Tools.Finance.TCMB/releases)
[![License](https://img.shields.io/github/license/elminalirzayev/Easy.Tools.Finance.TCMB)](https://github.com/elminalirzayev/Easy.Tools.Finance.TCMB/blob/master/LICENSE.txt)
[![NuGet](https://img.shields.io/nuget/v/Easy.Tools.Finance.TCMB.svg)](https://www.nuget.org/packages/Easy.Tools.Finance.TCMB)

# Easy.Tools.Finance.TCMB

* **Easy.Tools.Finance.TCMB, T.C. Merkez Bankası (TCMB) güncel döviz kurlarını çekmek için geliştirilmiş; kullanımı kolay, modern ve dayanıklı (resilient) bir .NET kütüphanesidir.
* **Easy.Tools.Finance.TCMB is a lightweight, modern, and resilient .NET library designed to fetch daily exchange rates from the Central Bank of the Republic of Turkey (TCMB).

---

## Installation

Install via NuGet:

```
dotnet add package Easy.Tools.Finance.TCMB
```

Or via NuGet Package Manager:

```
Install-Package Easy.Tools.Finance.TCMB
```

---

## Features

* **Easy Integration:** Fully compatible with .NET Dependency Injection (DI).
* **Resilience (Retry Logic):** Includes built-in retry mechanisms to handle temporary network glitches or TCMB server timeouts.
* **Type-Safe:** Automatically handles XML parsing and returns clean C# objects with `decimal` properties.
* **Configurable:** Retry counts and delay durations are fully customizable via options.

---

## Usage

### 1. Service Registration (Program.cs)

Register the service in your `Program.cs`:

```csharp
using Easy.Tools.Finance.TCMB.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Standard registration:
builder.Services.AddTcmbClient();

// OR: Registration with custom options:
builder.Services.AddTcmbClient(options => 
{
    options.RetryCount = 5;         // Retry 5 times on failure
    options.RetryDelaySeconds = 2;  // Wait 2 seconds between retries
});
//OR: Registration with custom TCMB Base URL (e.g., using a proxy or mirror):
builder.Services.AddTcmbClient(options =>
{
    options.BaseUrl = "https://my-proxy-server.com/tcmb-mirror/";
});

var app = builder.Build();
```


### 2. Fetching Rates (Controller Example)

Inject `ITcmbClient` into your controllers or services.

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

    [HttpGet]
    public async Task<IActionResult> GetRates()
    {
        // Fetch all rates
        var rates = await _tcmbClient.GetTodayRatesAsync();

        // Find USD Selling rate
        var usdRate = rates.FirstOrDefault(x => x.Code == "USD")?.ForexSelling;

        return Ok(new 
        { 
            Message = "Rates fetched successfully", 
            UsdSelling = usdRate,
            AllRates = rates 
        });
    }
}
```

---

## Models

The package returns a list of `TcmbCurrency` objects. Key properties include:

-   `Code`: Currency code (e.g., USD, EUR).
    
-   `CurrencyName`: Name of the currency (e.g., US DOLLAR).
    
-   `ForexBuying`: Forex buying rate (Decimal).
    
-   `ForexSelling`: Forex selling rate (Decimal).
    
-   `BanknoteBuying`: Banknote buying rate.
    
-   `BanknoteSelling`: Banknote selling rate


---

## License

MIT License.

---

© 2025 Elmin Alirzayev / Easy Code Tools
