using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.Extensions.Options;

namespace System.DirectoryServices.AccountManagement
{
    /// <summary>
    /// Represents an active directory authentication provider.
    /// </summary>
    public sealed class ActiveDirectoryAuthenticationProvider : IAuthenticationProvider
    {
        /// <summary>
        /// To detect redundant calls <see cref="Dispose"/> method.
        /// </summary>
        private bool _disposedValue;
        /// <summary>
        /// The server or domain context.
        /// </summary>
        private readonly PrincipalContext _principalContext;

        public event EventHandler<AuthenticationEventArgs> Authentication;

        public string Issuer => WindowsIdentity.DefaultIssuer;

        /// <summary>
        /// Initializes a new instance of <see cref="ActiveDirectoryAuthenticationProvider"/> class.
        /// </summary>
        /// <param name="options"></param>
        public ActiveDirectoryAuthenticationProvider(IOptionsSnapshot<PrincipalContextOptions> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var principalOptions = options.Value;
            _principalContext = new PrincipalContext(
                contextType: principalOptions.ContextType,
                name: principalOptions.Name,
                container: principalOptions.Container,
                options: principalOptions.ContextOptions,
                userName: principalOptions.Username,
                password: principalOptions.Password);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public ClaimsIdentity FindById(string userId, string password)
        {
            return CreateIdentityClaims(userId, password);
        }
        public ClaimsIdentity FindByUsername(string username, string password)
        {
            return CreateIdentityClaims(username, password);
        }
        public bool IsAccountLockedOut(string username)
        {
            using var user = UserPrincipal.FindByIdentity(_principalContext, username);
            return user != null ? user.IsAccountLockedOut() : false;
        }
        public bool ValidateCredentials(string username, string password)
        {
            var validate = _principalContext.ValidateCredentials(username, password);
            Authentication?.Invoke(this, new AuthenticationEventArgs(Issuer, username, validate));
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
                    _principalContext.Dispose();
                }
                _disposedValue = true;
            }
        }
        /// <summary>
        /// Finds and validate credentials by user. If authentication is success creates a claims-based identity.
        /// </summary>
        /// <param name="identifier">The account username or id.</param>
        /// <param name="password">The account password.</param>
        private ClaimsIdentity CreateIdentityClaims(string identifier, string password)
        {
            using var user = UserPrincipal.FindByIdentity(_principalContext, identifier);
            if (user != null)
            {
                ClaimsIdentity claimsIdentity = null;
                if (ValidateCredentials(user.UserPrincipalName, password))
                {
                    claimsIdentity = new ClaimsIdentity(ContextOptions.Negotiate.ToString(), ClaimTypes.Name, ClaimTypes.Role);
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.DisplayName ?? string.Empty, ClaimValueTypes.String, Issuer));
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, user.GivenName ?? string.Empty, ClaimValueTypes.String, Issuer));
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Sid.ToString(), ClaimValueTypes.String, Issuer));
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.EmailAddress ?? string.Empty, ClaimValueTypes.Email, Issuer));
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.WindowsAccountName, user.SamAccountName ?? string.Empty, ClaimValueTypes.String, Issuer));
                    claimsIdentity.AddClaims(user.GetGroups().Select(group => new Claim(ClaimTypes.Role, group.SamAccountName, ClaimValueTypes.String, Issuer)));
                }
                else
                {
                    throw new InvalidCredentialException(Properties.Resources.InvalidCredentialException);
                }
                return claimsIdentity;
            }
            return null;
        }
    }
}
