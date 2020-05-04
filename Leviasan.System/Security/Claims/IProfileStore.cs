namespace System.Security.Claims
{
    /// <summary>
    /// Represents the service for authenticating users.
    /// </summary>
    public interface IProfileStore : IDisposable
    {
        /// <summary>
        /// The profile store name.
        /// </summary>
        string Issuer { get; }

        /// <summary>
        /// Gets claims identity if any.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>If user authentication is success, the property <see cref="ClaimsIdentity.IsAuthenticated"/> will be true.</returns>
        ClaimsIdentity FindById(string userId);
        /// <summary>
        /// Gets claims identity if any.
        /// </summary>
        /// <param name="username">The account name.</param>
        /// <returns>If user authentication is success, the property <see cref="ClaimsIdentity.IsAuthenticated"/> will be true.</returns>
        ClaimsIdentity FindByName(string username);
        /// <summary>
        /// Gets account identifier if any.
        /// </summary>
        /// <param name="username">The account name.</param>
        /// <returns>The user identifier if any otherwise null.</returns>
        string FindUserIdByName(string username);
        /// <summary>
        /// Checks is account locked out.
        /// </summary>
        /// <param name="username">The account name.</param>
        bool IsLockedOut(string username);
        /// <summary>
        /// Validates the credentials.
        /// </summary>
        /// <param name="username">The account name.</param>
        /// <param name="password">The account password.</param>
        bool IsValidate(string username, string password);
    }
}
