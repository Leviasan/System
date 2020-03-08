using System.Collections.Generic;
using System.Security.Claims;

namespace System.Security.Authentication
{
    /// <summary>
    /// Provides the providers collection for user authentication.
    /// </summary>
    public interface IAuthenticationService : IAuthenticationProvider
    {
        /// <summary>
        /// The authentication providers.
        /// </summary>
        IReadOnlyDictionary<string, IAuthenticationProvider> Providers { get; }
        /// <summary>
        /// Finds the user by external provider.
        /// </summary>
        /// <param name="provider">The provider name.</param>
        /// <param name="userId">The unique user identifier.</param>
        /// <param name="password">The password.</param>
        ClaimsIdentity FindByExternalProvider(string provider, string userId, string password);
    }
}
