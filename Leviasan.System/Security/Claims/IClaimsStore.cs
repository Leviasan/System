namespace System.Security.Claims
{
    /// <summary>
    /// Provides the APIs for generate claims identity.
    /// </summary>
    public interface IClaimsStore : IDisposable
    {
        /// <summary>
        /// The claim store name.
        /// </summary>
        string Issuer { get; }

        /// <summary>
        /// Finds and returns the claims identity, if any, who has the specified userId.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        ClaimsIdentity FindById(string userId);
        /// <summary>
        /// Finds and returns the claims identity, if any, who has the specified normalized user name.
        /// </summary>
        /// <param name="username">The normalized user name to search for.</param>
        ClaimsIdentity FindByName(string username);
        /// <summary>
        /// Checks is account locked out.
        /// </summary>
        /// <param name="username">The normalized user name to search for.</param>
        bool IsLockedOut(string username);
        /// <summary>
        /// Validates the credentials.
        /// </summary>
        /// <param name="username">The normalized user name to search for.</param>
        /// <param name="password">The account password.</param>
        bool IsValidate(string username, string password);
    }
}
