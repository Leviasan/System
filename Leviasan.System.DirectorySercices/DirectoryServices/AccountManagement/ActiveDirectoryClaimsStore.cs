using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.Extensions.Options;

namespace System.DirectoryServices.AccountManagement
{
    /// <summary>
    /// Represents an active directory claim store.
    /// </summary>
    public sealed class ActiveDirectoryClaimsStore : IClaimsStore
    {
        /// <summary>
        /// Default authentication scheme.
        /// </summary>
        public const string AuthenticationScheme = "Windows";

        /// <summary>
        /// To detect redundant calls <see cref="Dispose()"/> method.
        /// </summary>
        private bool _disposedValue;
        /// <summary>
        /// The server or domain context.
        /// </summary>
        private readonly PrincipalContext _principalContext;

        /// <summary>
        /// Initializes a new instance of <see cref="ActiveDirectoryClaimsStore"/> class.
        /// </summary>
        /// <param name="principalContextOptions">The principal context options.</param>
        public ActiveDirectoryClaimsStore(IOptionsMonitor<PrincipalContextOptions> principalContextOptions)
        {
            if (principalContextOptions == null)
                throw new ArgumentNullException(nameof(principalContextOptions));

            var principalOptions = principalContextOptions.CurrentValue;
            _principalContext = new PrincipalContext(
                contextType: principalOptions.ContextType,
                name: principalOptions.Name,
                container: principalOptions.Container,
                options: principalOptions.ContextOptions,
                userName: principalOptions.Username,
                password: principalOptions.Password);
        }

        /// <summary>
        /// The claim store name.
        /// </summary>
        public string Issuer => WindowsIdentity.DefaultIssuer;

        /// <summary>
        /// Releases the unmanaged resources and releases the managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Finds and returns the claims identity, if any, who has the specified userId.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        public ClaimsIdentity FindById(string userId)
        {
            return CreateIdentityClaims(userId);
        }
        /// <summary>
        /// Finds and returns the claims identity, if any, who has the specified normalized user name.
        /// </summary>
        /// <param name="username">The normalized user name to search for.</param>
        public ClaimsIdentity FindByName(string username)
        {
            return CreateIdentityClaims(username);
        }
        /// <summary>
        /// Checks is account locked out.
        /// </summary>
        /// <param name="username">The normalized user name to search for.</param>
        public bool IsLockedOut(string username)
        {
            using var user = UserPrincipal.FindByIdentity(_principalContext, username);
            return user != null ? user.IsAccountLockedOut() : false;
        }
        /// <summary>
        /// Validates the credentials.
        /// </summary>
        /// <param name="username">The normalized user name to search for.</param>
        /// <param name="password">The account password.</param>
        public bool IsValidate(string username, string password)
        {
            return _principalContext.ValidateCredentials(username, password);
        }

        /// <summary>
        /// Finds user. If authentication is success creates a claims-based identity.
        /// </summary>
        /// <param name="identityValue">The identity of the user principal. This parameter can be any format that is contained in the <see cref="IdentityType"/> enumeration.</param>
        private ClaimsIdentity CreateIdentityClaims(string identityValue)
        {
            ClaimsIdentity claimsIdentity = null;
            using var user = UserPrincipal.FindByIdentity(_principalContext, identityValue);
            if (user != null)
            {
                claimsIdentity = new ClaimsIdentity(AuthenticationScheme);
                using var windowsIdentity = new WindowsIdentity(user.UserPrincipalName);
                // Add native information
                claimsIdentity.AddClaims(windowsIdentity.Claims);
                // Add name information
                claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.DisplayName ?? string.Empty, ClaimValueTypes.String, null, Issuer));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, user.GivenName ?? string.Empty, ClaimValueTypes.String, null, Issuer));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Surname, user.Surname ?? string.Empty, ClaimValueTypes.String, null, Issuer));
                // Add windows information
                claimsIdentity.AddClaim(new Claim(ClaimTypes.WindowsAccountName, user.UserPrincipalName ?? string.Empty, ClaimValueTypes.String, null, Issuer));
                claimsIdentity.AddClaims(windowsIdentity.Groups.Translate(typeof(NTAccount)).Select(x => new Claim(ClaimTypes.Role, x.Value, ClaimValueTypes.String, null, Issuer)));
                // Add service information
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.EmailAddress ?? string.Empty, ClaimValueTypes.Email, null, Issuer));
            }
            return claimsIdentity ?? new ClaimsIdentity();
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
                    _principalContext.Dispose();
                }
                _disposedValue = true;
            }
        }
    }
}
