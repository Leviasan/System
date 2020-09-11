using System.Security.Claims;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// ASP.NET Core extensions for adds profile store manager in services.
    /// </summary>
    public static class ProfileStoreManagerExtensions
    {
        /// <summary>
        /// Adds the service <see cref="IProfileStoreManager"/> with implementation <see cref="ProfileStoreManager"/>.
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddProfileStoreManager(this IServiceCollection services)
        {
            services.AddScoped<IProfileStoreManager, ProfileStoreManager>();
            return services;
        }
    }
}
