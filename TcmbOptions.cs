namespace Easy.Tools.Finance.TCMB
{
    /// <summary>
    /// Configuration options for the TCMB Client.
    /// </summary>
    public class TcmbOptions
    {
        /// <summary>
        /// Base URL for the Central Bank of the Republic of Turkey (TCMB) XML service.
        /// Default: https://www.tcmb.gov.tr/kurlar/
        /// </summary>
        public string BaseUrl { get; set; } = "https://www.tcmb.gov.tr/kurlar/";

        /// <summary>
        /// The number of times to retry the request if it fails due to network issues.
        /// Default: 3
        /// </summary>
        public int RetryCount { get; set; } = 3;

        /// <summary>
        /// The duration to wait (in seconds) between retry attempts.
        /// Default: 1 second
        /// </summary>
        public int RetryDelaySeconds { get; set; } = 1;
    }
}