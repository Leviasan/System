using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using IdentityModel;
using Microsoft.Extensions.Options;

namespace System.DirectoryServices.AccountManagement
{
    /// <summary>
    /// Represents an active directory claim store.
    /// </summary>
    public sealed class ActiveDirectoryProfileStore : IProfileStore, IDisposable
    {
        /// <summary>
        /// To detect redundant calls <see cref="Dispose()"/> method.
        /// </summary>
        private bool _disposedValue;
        /// <summary>
        /// The server or domain context.
        /// </summary>
        private readonly PrincipalContext _principalContext;

        /// <summary>
        /// Initializes a new instance of <see cref="ActiveDirectoryProfileStore"/> class.
        /// </summary>
        /// <param name="principalContextOptions">The principal context options.</param>
        public ActiveDirectoryProfileStore(IOptionsMonitor<PrincipalContextOptions> principalContextOptions)
        {
            var principalOptions = principalContextOptions?.CurrentValue ?? throw new ArgumentNullException(nameof(principalContextOptions));

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
        /// Gets claims identity.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>If user authentication is success, the property <see cref="ClaimsIdentity.IsAuthenticated"/> will be true.</returns>
        public ClaimsIdentity FindById(string userId)
        {
            return CreateClaimsIdentity(userId);
        }
        /// <summary>
        /// Gets claims identity.
        /// </summary>
        /// <param name="username">The account name.</param>
        /// <returns>If user authentication is success, the property <see cref="ClaimsIdentity.IsAuthenticated"/> will be true.</returns>
        public ClaimsIdentity FindByName(string username)
        {
            return CreateClaimsIdentity(username);
        }
        /// <summary>
        /// Checks is account locked out.
        /// </summary>
        /// <param name="username">The account name.</param>
        /// <returns>True if account locked out, otherwise false.</returns>
        public bool IsLockedOut(string username)
        {
            using var user = UserPrincipal.FindByIdentity(_principalContext, username);
            return user != null ? user.IsAccountLockedOut() : false;
        }
        /// <summary>
        /// Validates the credentials.
        /// </summary>
        /// <param name="username">The account name.</param>
        /// <param name="password">The account password.</param>
        /// <returns>True if the credential is valid, otherwise false.</returns>
        public bool IsValidate(string username, string password)
        {
            return _principalContext.ValidateCredentials(username, password);
        }

        /// <summary>
        /// Gets user identifier if any.
        /// </summary>
        /// <param name="identityValue"></param>
        /// <returns>Return user identifier if any, otherwise null.</returns>
        private string GetUserId(string identityValue)
        {
            using var user = UserPrincipal.FindByIdentity(_principalContext, identityValue);
            return user?.Sid.ToString();
        }
        /// <summary>
        /// Gets the authentication result. 
        /// Specifications: https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims
        /// </summary>
        /// <param name="identityValue">The identity of the user principal. This parameter can be any format that is contained in the <see cref="IdentityType"/> enumeration.</param>
        private ClaimsIdentity CreateClaimsIdentity(string identityValue)
        {
            ClaimsIdentity claimsIdentity = null;
            using var user = UserPrincipal.FindByIdentity(_principalContext, identityValue);
            if (user != null)
            {
                claimsIdentity = new ClaimsIdentity(authenticationType: "Windows", nameType: JwtClaimTypes.Name, roleType: JwtClaimTypes.Role);
                // ========== Mandatory identity resource: openid  ==========
                claimsIdentity.AddClaim(new Claim(JwtClaimTypes.Subject, user.Sid.ToString(), ClaimValueTypes.Sid, nameof(ActiveDirectoryProfileStore), Issuer));
                // ========== Optional identity resource : profile  ==========
                if (!string.IsNullOrWhiteSpace(user.DisplayName)) claimsIdentity.AddClaim(new Claim(JwtClaimTypes.Name, user.DisplayName, ClaimValueTypes.String, nameof(ActiveDirectoryProfileStore), Issuer));
                if (!string.IsNullOrWhiteSpace(user.Surname)) claimsIdentity.AddClaim(new Claim(JwtClaimTypes.FamilyName, user.Surname, ClaimValueTypes.String, nameof(ActiveDirectoryProfileStore), Issuer));
                if (!string.IsNullOrWhiteSpace(user.GivenName)) claimsIdentity.AddClaim(new Claim(JwtClaimTypes.GivenName, user.GivenName, ClaimValueTypes.String, nameof(ActiveDirectoryProfileStore), Issuer));
                if (!string.IsNullOrWhiteSpace(user.MiddleName)) claimsIdentity.AddClaim(new Claim(JwtClaimTypes.MiddleName, user.MiddleName, ClaimValueTypes.String, nameof(ActiveDirectoryProfileStore), Issuer));
                if (!string.IsNullOrWhiteSpace(user.SamAccountName)) claimsIdentity.AddClaim(new Claim(JwtClaimTypes.NickName, user.SamAccountName, ClaimValueTypes.String, nameof(ActiveDirectoryProfileStore), Issuer));
                // JwtClaimTypes.PreferredUserName;
                // JwtClaimTypes.Profile;
                // JwtClaimTypes.Picture;
                // JwtClaimTypes.WebSite;
                // JwtClaimTypes.Gender;
                // JwtClaimTypes.BirthDate;
                // JwtClaimTypes.ZoneInfo;
                // JwtClaimTypes.Locale;
                // JwtClaimTypes.UpdatedAt;
                // ========== Optional identity resource: email ==========
                var hasEmail = !string.IsNullOrWhiteSpace(user.EmailAddress);
                if (hasEmail) claimsIdentity.AddClaim(new Claim(JwtClaimTypes.Email, user.EmailAddress, ClaimValueTypes.Email, nameof(ActiveDirectoryProfileStore), Issuer));
                claimsIdentity.AddClaim(new Claim(JwtClaimTypes.EmailVerified, hasEmail.ToString(), ClaimValueTypes.Boolean, nameof(ActiveDirectoryProfileStore), Issuer));
                // ========== Optional identity resource: address ==========
                // JwtClaimTypes.Address
                // ========== Optional identity resource: phone ==========
                var hasPhone = !string.IsNullOrWhiteSpace(user.VoiceTelephoneNumber);
                if (hasPhone) claimsIdentity.AddClaim(new Claim(JwtClaimTypes.PhoneNumber, user.VoiceTelephoneNumber, ClaimValueTypes.String, nameof(ActiveDirectoryProfileStore), Issuer));
                claimsIdentity.AddClaim(new Claim(JwtClaimTypes.PhoneNumberVerified, hasPhone.ToString(), ClaimValueTypes.Boolean, nameof(ActiveDirectoryProfileStore), Issuer));
                // ========== Optional identity resource : role  ==========
                var groups = user.GetGroups();
                claimsIdentity.AddClaims(groups.Select(x => new Claim(JwtClaimTypes.Role, x.SamAccountName, ClaimValueTypes.String, nameof(ActiveDirectoryProfileStore), Issuer)));
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

