using System.Collections.Generic;

namespace System.Security.Claims
{
    /// <summary>
    /// Provides the APIs for generating claims identity from specified stores.
    /// </summary>
    public interface IClaimsStoreManager : IClaimsStore
    {
        /// <summary>
        /// The claim stores.
        /// </summary>
        IReadOnlyDictionary<string, IClaimsStore> Stores { get; }

        /// <summary>
        /// Finds and returns the claims, if any, who has the specified userId from external claim store.
        /// </summary>
        /// <param name="provider">The external claim store name.</param>
        /// <param name="userId">The user ID to search for.</param>
        ClaimsIdentity FindById(string provider, string userId);
        /// <summary>
        /// Finds and returns the claims identity, if any, who has the specified normalized user name from external claim store.
        /// </summary>
        /// <param name="provider">The external claim store name.</param>
        /// <param name="username">The normalized user name to search for.</param>
        ClaimsIdentity FindByName(string provider, string username);
        /// <summary>
        /// Checks is account locked out from external claim store.
        /// </summary>
        /// <param name="provider">The external claim store name.</param>
        /// <param name="username">The normalized user name to search for.</param>
        bool IsLockedOut(string provider, string username);
        /// <summary>
        /// Validates the credentials from external claim store.
        /// </summary>
        /// <param name="provider">The external claim store name.</param>
        /// <param name="username">The normalized user name to search for.</param>
        /// <param name="password">The account password.</param>
        bool IsValidate(string provider, string username, string password);
    }
}
