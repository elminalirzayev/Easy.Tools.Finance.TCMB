using Microsoft.Extensions.DependencyInjection;

namespace Easy.Tools.Finance.TCMB.Extensions
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
            services.AddHttpClient<ITcmbClient, TcmbClient>();

            // Configure options if provided
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }

            return services;
        }
    }
}