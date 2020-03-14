using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// ASP.NET Core extensions for active directory claims store.
    /// </summary>
    public static class ActiveDirectoryClaimsStoreExtensions
    {
        /// <summary>
        /// Appsettings.json section
        /// </summary>
        public const string PrincipalContextOptionsSection = "System:DirectoryServices:AccountManagement:PrincipalContext";

        /// <summary>
        /// Adds scoped service <see cref="IClaimsStore"/> with implementation <see cref="ActiveDirectoryClaimsStore"/>.
        /// </summary>
        /// <param name="services">The collection of service descriptors.</param>
        public static IServiceCollection AddActiveDirectoryClaimsStore(this IServiceCollection services)
        {
            // Get configuration service
            var configuration = services
                .First(x => x.ServiceType == typeof(IConfiguration))
                .ImplementationFactory.Invoke(null) as IConfiguration;
            // Add option validator
            services.AddTransient<PrincipalContextValidateOptions>();
            // Register options
            services.AddOptions<PrincipalContextOptions>()
                .Bind(configuration.GetSection(PrincipalContextOptionsSection))
                .Validate<PrincipalContextValidateOptions>(
                    validation: (options, validator) => validator.Validate(null, options).Succeeded,
                    failureMessage: $"appsettings.json file has error in section: {PrincipalContextOptionsSection}.");
            // Add service
            services.AddScoped<IClaimsStore, ActiveDirectoryClaimsStore>();
            return services;
        }
    }
}
