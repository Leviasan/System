using System;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// ASP.NET Core extensions for active directory claims store.
    /// </summary>
    public static class ActiveDirectoryProfileStoreExtensions
    {
        /// <summary>
        /// Adds service <see cref="IProfileStore"/> with implementation <see cref="ActiveDirectoryProfileStore"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="section"></param>
        public static IServiceCollection AddActiveDirectoryProfileStore(this IServiceCollection services, string section)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            // Get configuration service
            var configuration = services
                .First(x => x.ServiceType == typeof(IConfiguration))
                .ImplementationFactory.Invoke(null) as IConfiguration;
            // Add services
            services.Configure<PrincipalContextOptions>(configuration.GetSection(section));
            services.AddSingleton<IValidateOptions<PrincipalContextOptions>, PrincipalContextValidateOptions>();
            services.AddScoped<IProfileStore, ActiveDirectoryProfileStore>();

            return services;
        }
    }
}
