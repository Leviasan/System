using System.Security.Claims;

namespace System.Security.Authentication
{
    /// <summary>
    /// Provides the mechanism for user authentication.
    /// </summary>
    public interface IAuthenticationProvider : IDisposable
    {
        /// <summary>
        /// Notification after calling <see cref="ValidateCredentials"/>.
        /// </summary>
        event EventHandler<AuthenticationEventArgs> Authentication;

        /// <summary>
        /// The unique name of the provider.
        /// </summary>
        string Issuer { get; }
        /// <summary>
        /// Finds the user by identifier.
        /// </summary>
        /// <param name="userId">The unique user identifier.</param>
        /// <param name="password">The password.</param>
        ClaimsIdentity FindById(string userId, string password);
        /// <summary>
        /// Finds the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        ClaimsIdentity FindByUsername(string username, string password);
        /// <summary>
        /// Checks is account locked out.
        /// </summary>
        /// <param name="username">The username.</param>
        bool IsAccountLockedOut(string username);
        /// <summary>
        /// Validates the credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        bool ValidateCredentials(string username, string password);
    }
}
