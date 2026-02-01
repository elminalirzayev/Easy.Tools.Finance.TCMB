using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Xml.Serialization;
using System.Globalization;


namespace Easy.Tools.Finance.TCMB
{
    /// <summary>
    /// High-performance implementation of the TCMB Client.
    /// </summary>
    public class TcmbClient : ITcmbClient
    {
        private readonly HttpClient _httpClient;
        private readonly TcmbOptions _options;

        // PERF: XmlSerializer is made static to prevent memory leaks (cached instance).
        private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(TcmbResponse));

        public TcmbClient(HttpClient httpClient, IOptions<TcmbOptions> options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrWhiteSpace(_options.BaseUrl) || !Uri.IsWellFormedUriString(_options.BaseUrl, UriKind.Absolute))
            {
                throw new ArgumentException("A valid BaseUrl must be provided in TcmbOptions.");
            }
        }

        public async Task<List<TcmbCurrency>> GetTodayRatesAsync(CancellationToken cancellationToken = default)
        {
            // Endpoint for current rates
            var url = $"{_options.BaseUrl.TrimEnd('/')}/today.xml";
            return await FetchRatesInternalAsync(url, cancellationToken);
        }

        public async Task<List<TcmbCurrency>> GetRatesByDateAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            // TCMB Archive URL Structure: https://www.tcmb.gov.tr/kurlar/yyyyMM/ddMMyyyy.xml

            var yearMonth = date.ToString("yyyyMM", CultureInfo.InvariantCulture);
            var dayMonthYear = date.ToString("ddMMyyyy", CultureInfo.InvariantCulture);

            var url = $"{_options.BaseUrl.TrimEnd('/')}/{yearMonth}/{dayMonthYear}.xml";

            return await FetchRatesInternalAsync(url, cancellationToken);
        }


        /// <summary>
        /// Private helper method to handle Retry Logic, XML Fetching, and Deserialization.
        /// Prevents code duplication between GetTodayRatesAsync and GetRatesByDateAsync.
        /// </summary>
        private async Task<List<TcmbCurrency>> FetchRatesInternalAsync(string url, CancellationToken cancellationToken)
        {
            for (int i = 0; i < _options.RetryCount; i++)
            {
                try
                {
                    // 1. Send Request (HeadersRead for performance)
                    using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

                    // 2. Ensure Success (Throws if 404, 500, etc.)
                    response.EnsureSuccessStatusCode();

                    // 3. Get Stream
#if NET5_0_OR_GREATER
                    using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
#else
                    using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
#endif

                    // 4. Deserialize
                    if (_serializer.Deserialize(responseStream) is TcmbResponse xmlResponse)
                    {
                        return xmlResponse.Currencies ?? new List<TcmbCurrency>();
                    }
                }
                catch (HttpRequestException)
                {
                    // Last attempt? Throw.
                    if (i == _options.RetryCount - 1) throw;

                    // Wait before retry
                    await Task.Delay(TimeSpan.FromSeconds(_options.RetryDelaySeconds), cancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    throw; // Exit immediately
                }
                catch (Exception)
                {
                    throw; // Parsing errors or other critical failures
                }
            }

            return new List<TcmbCurrency>();
        }
    }
}