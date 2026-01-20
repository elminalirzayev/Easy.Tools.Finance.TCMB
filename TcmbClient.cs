using Easy.Tools.Finance.TCMB.Models;
using Microsoft.Extensions.Options;
using System.Xml.Serialization;

namespace Easy.Tools.Finance.TCMB
{
    /// <summary>
    /// Implementation of the TCMB Client to fetch currency data via HTTP.
    /// </summary>
    public class TcmbClient : ITcmbClient
    {
        private readonly HttpClient _httpClient;
        private readonly TcmbOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="TcmbClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client instance.</param>
        /// <param name="options">Configuration options.</param>
        public TcmbClient(HttpClient httpClient, IOptions<TcmbOptions> options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

            // Validation: Ensure BaseUrl is valid
            if (string.IsNullOrWhiteSpace(_options.BaseUrl) || !Uri.IsWellFormedUriString(_options.BaseUrl, UriKind.Absolute))
            {
                throw new ArgumentException("A valid BaseUrl must be provided in TcmbOptions.");
            }
        }

        /// <inheritdoc />
        public async Task<List<TcmbCurrency>> GetTodayRatesAsync()
        {
            var url = $"{_options.BaseUrl.TrimEnd('/')}/today.xml";

            for (int i = 0; i < _options.RetryCount; i++)
            {
                try
                {
                    // 1. Send Request
                    // We use GetStreamAsync for better performance with XML Deserialization
                    using var responseStream = await _httpClient.GetStreamAsync(url);

                    // 2. Deserialize XML
                    XmlSerializer serializer = new XmlSerializer(typeof(TcmbResponse));

                    if (serializer.Deserialize(responseStream) is TcmbResponse response)
                    {
                        return response.Currencies ?? new List<TcmbCurrency>();
                    }
                }
                catch (HttpRequestException)
                {
                    // If it is the last attempt, rethrow the exception
                    if (i == _options.RetryCount - 1) throw;

                    // Wait before the next retry
                    await Task.Delay(TimeSpan.FromSeconds(_options.RetryDelaySeconds));
                }
                catch (InvalidOperationException ex)
                {
                    // XML parsing errors usually throw InvalidOperationException
                    throw new Exception("Failed to deserialize TCMB XML response.", ex);
                }
                catch (Exception)
                {
                    // Non-transient errors should not be retried
                    throw;
                }
            }

            return new List<TcmbCurrency>();
        }
    }
}