namespace System.Security.Claims
{
    /// <summary>
    /// Represents the service for authenticating users.
    /// </summary>
    public interface IProfileStore
    {
        /// <summary>
        /// The profile store name.
        /// </summary>
        string Issuer { get; }

        /// <summary>
        /// Gets claims identity.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>If user authentication is success, the property <see cref="ClaimsIdentity.IsAuthenticated"/> will be true.</returns>
        ClaimsIdentity FindById(string userId);
        /// <summary>
        /// Gets claims identity.
        /// </summary>
        /// <param name="username">The account name.</param>
        /// <returns>If user authentication is success, the property <see cref="ClaimsIdentity.IsAuthenticated"/> will be true.</returns>
        ClaimsIdentity FindByName(string username);
        /// <summary>
        /// Checks is account locked out.
        /// </summary>
        /// <param name="username">The account name.</param>
        /// <returns>True if account locked out, otherwise false.</returns>
        bool IsLockedOut(string username);
        /// <summary>
        /// Validates the credentials.
        /// </summary>
        /// <param name="username">The account name.</param>
        /// <param name="password">The account password.</param>
        /// <returns>True if the credential is valid, otherwise false.</returns>
        bool IsValidate(string username, string password);
    }
}
