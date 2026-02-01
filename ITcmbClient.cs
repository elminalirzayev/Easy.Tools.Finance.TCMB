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
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A list of currency rates.</returns>
        /// <exception cref="HttpRequestException">Thrown when the connection to TCMB fails after retries.</exception>
        Task<List<TcmbCurrency>> GetTodayRatesAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// Retrieves exchange rates for a specific date asynchronously.
        /// </summary>
        /// <param name="date">The date for which rates are requested.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A list of currency rates for the specified date.</returns>
        /// <exception cref="HttpRequestException">Thrown when the connection to TCMB fails after retries.</exception>
        Task<List<TcmbCurrency>> GetRatesByDateAsync(DateTime date, CancellationToken cancellationToken = default);
    }
}