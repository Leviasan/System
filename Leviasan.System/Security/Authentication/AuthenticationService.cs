using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace System.Security.Authentication
{
    /// <summary>
    /// Represents an authentication service with providers collection for authentication.
    /// </summary>
    public sealed class AuthenticationService : IAuthenticationService
    {
        /// <summary>
        /// To detect redundant calls <see cref="Dispose"/> method.
        /// </summary>
        private bool _disposedValue;

        /// <summary>
        /// Initializes a new instance of <see cref="AuthenticationService"/> class with a specified providers for user authentivation.
        /// </summary>
        /// <param name="providers">The collection of authentication provider.</param>
        public AuthenticationService(IEnumerable<IAuthenticationProvider> providers)
        {
            if (providers == null)
                throw new ArgumentNullException(nameof(providers));

            Providers = new ReadOnlyDictionary<string, IAuthenticationProvider>(providers.ToDictionary(k => k.Issuer));
        }

        public IReadOnlyDictionary<string, IAuthenticationProvider> Providers { get; }
        public string Issuer => nameof(AuthenticationService);

        public event EventHandler<AuthenticationEventArgs> Authentication;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public ClaimsIdentity FindByExternalProvider(string provider, string userId, string password)
        {
            if (!Providers.ContainsKey(provider))
                throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.KeyNotFoundException, provider));

            return Providers[provider].FindById(userId, password);
        }
        public ClaimsIdentity FindById(string userId, string password)
        {
            ClaimsIdentity claimsIdentity = null;
            foreach (var provider in Providers.Values)
            {
                claimsIdentity = provider.FindById(userId, password);
                if (claimsIdentity != null)
                    break;
            }
            return claimsIdentity;
        }
        public ClaimsIdentity FindByUsername(string username, string password)
        {
            ClaimsIdentity claimsIdentity = null;
            foreach (var provider in Providers.Values)
            {
                claimsIdentity = provider.FindByUsername(username, password);
                if (claimsIdentity != null)
                    break;
            }
            return claimsIdentity;
        }
        public bool IsAccountLockedOut(string username)
        {
            foreach (var provider in Providers.Values)
            {
                if (provider.IsAccountLockedOut(username))
                    return true;
            }
            return false;
        }
        public bool ValidateCredentials(string username, string password)
        {
            var validate = false;
            foreach (var provider in Providers.Values)
            {
                validate = provider.ValidateCredentials(username, password);
                Authentication?.Invoke(this, new AuthenticationEventArgs(provider.Issuer, username, validate));
                if (validate) break;
            }
            return validate;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">This indicates whether the method call comes from a <see cref="Dispose"/> method (its value is true) or from a finalizer (its value is false).</param>
        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var provider in Providers.Values)
                        provider.Dispose();
                }
                _disposedValue = true;
            }
        }
    }
}
