using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Easy.Tools.Finance.TCMB
{
    /// <summary>
    /// Extension methods for setting up TCMB Client in an IServiceCollection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the TCMB Client services to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="configureOptions">An optional action to configure the <see cref="TcmbOptions"/>.</param>
        /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
        public static IServiceCollection AddTcmbClient(this IServiceCollection services, Action<TcmbOptions>? configureOptions = null)
        {
            // Configure options
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }

            // Register HttpClient with Options
            services.AddHttpClient<ITcmbClient, TcmbClient>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<TcmbOptions>>().Value;
                if (!string.IsNullOrEmpty(options.BaseUrl))
                {
                    client.BaseAddress = new Uri(options.BaseUrl);
                }
            });

            return services;
        }
    }
}