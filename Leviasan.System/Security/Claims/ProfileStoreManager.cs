using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System.Security.Claims
{
    /// <summary>
    /// Represents the APIs for generating claims identity from specified stores.
    /// </summary>
    public sealed class ProfileStoreManager : IProfileStoreManager
    {
        /// <summary>
        /// To detect redundant calls <see cref="Dispose()"/> method.
        /// </summary>
        private bool _disposedValue;

        /// <summary>
        /// Initializes a new instance of <see cref="ProfileStoreManager"/> class with a specified claim stores.
        /// </summary>
        /// <param name="stores">The claim stores.</param>
        public ProfileStoreManager(IEnumerable<IProfileStore> stores)
        {
            if (stores == null)
                throw new ArgumentNullException(nameof(stores));

            Stores = stores.ToList().AsReadOnly().ToDictionary(k => k.Issuer);
        }

        /// <summary>
        /// The profile stores.
        /// </summary>
        public IReadOnlyDictionary<string, IProfileStore> Stores { get; }
        /// <summary>
        /// The claim store name.
        /// </summary>
        public string Issuer => ClaimsIdentity.DefaultIssuer;

        /// <summary>
        /// Releases the unmanaged resources and releases the managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Finds and returns the claims, if any, who has the specified userId from external claim store.
        /// </summary>
        /// <param name="provider">The external claim store name.</param>
        /// <param name="userId">The user ID to search for.</param>
        /// <param name="includeClaims">If true in result will be added user's claims.</param>
        /// <exception cref="KeyNotFoundException">Provider not found.</exception>
        /// <returns>If user authentication is success, the property <see cref="ClaimsIdentity.IsAuthenticated"/> will be true.</returns>
        public ClaimsIdentity FindById(string provider, string userId, bool includeClaims)
        {
            if (!Stores.ContainsKey(provider))
                throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.KeyNotFoundException, provider));

            return Stores[provider].FindById(userId, includeClaims);
        }
        /// <summary>
        /// Finds and returns the claims identity, if any, who has the specified userId.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <param name="includeClaims">If true in result will be added user's claims.</param>
        /// <returns>If user authentication is success, the property <see cref="ClaimsIdentity.IsAuthenticated"/> will be true.</returns>
        public ClaimsIdentity FindById(string userId, bool includeClaims)
        {
            ClaimsIdentity claimsIdentity = null;
            foreach (var store in Stores.Values)
            {
                claimsIdentity = store.FindById(userId, includeClaims);
                if (claimsIdentity != null)
                    break;
            }
            return claimsIdentity;
        }
        /// <summary>
        /// Finds and returns the claims identity, if any, who has the specified normalized user name from external claim store.
        /// </summary>
        /// <param name="provider">The external claim store name.</param>
        /// <param name="username">The normalized user name to search for.</param>
        /// <param name="includeClaims">If true in result will be added user's claims.</param>
        /// <returns>If user authentication is success, the property <see cref="ClaimsIdentity.IsAuthenticated"/> will be true.</returns>
        public ClaimsIdentity FindByName(string provider, string username, bool includeClaims)
        {
            if (!Stores.ContainsKey(provider))
                throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.KeyNotFoundException, provider));

            return Stores[provider].FindByName(username, includeClaims);
        }
        /// <summary>
        /// Finds and returns the claims identity, if any, who has the specified normalized user name.
        /// </summary>
        /// <param name="username">The normalized user name to search for.</param>
        /// <param name="includeClaims">If true in result will be added user's claims.</param>
        /// <returns>If user authentication is success, the property <see cref="ClaimsIdentity.IsAuthenticated"/> will be true.</returns>
        public ClaimsIdentity FindByName(string username, bool includeClaims)
        {
            ClaimsIdentity claimsIdentity = null;
            foreach (var store in Stores.Values)
            {
                claimsIdentity = store.FindByName(username, includeClaims);
                if (claimsIdentity != null)
                    break;
            }
            return claimsIdentity;
        }
        /// <summary>
        /// Checks is account locked out from external claim store.
        /// </summary>
        /// <param name="provider">The external claim store name.</param>
        /// <param name="username">The normalized user name to search for.</param>
        /// <exception cref="KeyNotFoundException">Provider not found.</exception>
        /// <returns>True if account locked out, otherwise false.</returns>
        public bool IsLockedOut(string provider, string username)
        {
            if (!Stores.ContainsKey(provider))
                throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.KeyNotFoundException, provider));

            return Stores[provider].IsLockedOut(username);
        }
        /// <summary>
        /// Checks is account locked out.
        /// </summary>
        /// <param name="username">The normalized user name to search for.</param>
        /// <returns>True if account locked out, otherwise false.</returns>
        public bool IsLockedOut(string username)
        {
            foreach (var store in Stores.Values)
            {
                if (store.IsLockedOut(username))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Validates the credentials from external claim store.
        /// </summary>
        /// <param name="provider">The external claim store name.</param>
        /// <param name="username">The normalized user name to search for.</param>
        /// <param name="password">The account password.</param>
        /// <returns>True if the credential is valid, otherwise false.</returns>
        public bool IsValidate(string provider, string username, string password)
        {
            if (!Stores.ContainsKey(provider))
                throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.KeyNotFoundException, provider));

            return Stores[provider].IsValidate(username, password);
        }
        /// <summary>
        /// Validates the credentials.
        /// </summary>
        /// <param name="username">The normalized user name to search for.</param>
        /// <param name="password">The account password.</param>
        /// <returns>True if the credential is valid, otherwise false.</returns>
        public bool IsValidate(string username, string password)
        {
            foreach (var store in Stores.Values)
            {
                if (store.IsValidate(username, password))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Releases the unmanaged resources and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var store in Stores.Values)
                        store.Dispose();
                }
                _disposedValue = true;
            }
        }
    }
}
