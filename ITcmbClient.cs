using Easy.Tools.Finance.TCMB.Models;
#if NETFRAMEWORK
using System.Net.Http;
#endif
namespace Easy.Tools.Finance.TCMB
{
    /// <summary>
    /// Interface for fetching currency rates from TCMB (Central Bank of the Republic of Turkey).
    /// </summary>
    public interface ITcmbClient
    {
        /// <summary>
        /// Retrieves today's exchange rates asynchronously.
        /// Includes built-in retry logic for resilience.
        /// </summary>
        /// <returns>A list of currency rates.</returns>
        /// <exception cref="HttpRequestException">Thrown when the connection to TCMB fails after retries.</exception>
        Task<List<TcmbCurrency>> GetTodayRatesAsync();
    }
}