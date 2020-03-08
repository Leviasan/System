namespace System.Security.Authentication
{
    /// <summary>
    /// Provides data for <see cref="IAuthenticationProvider.Authentication"/> event.
    /// </summary>
    public sealed class AuthenticationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AuthenticationEventArgs"/> class.
        /// </summary>
        /// <param name="provider">The authentication provider name.</param>
        /// <param name="userIdentifier">The username or id.</param>
        /// <param name="isAuthentication">The authentication result.</param>
        /// <exception cref="ArgumentNullException">The provider or userIdentifier is null.</exception>
        public AuthenticationEventArgs(string provider, string userIdentifier, bool isAuthentication)
        {
            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
            UserIdentifier = userIdentifier ?? throw new ArgumentNullException(nameof(userIdentifier));
            IsAuthentication = isAuthentication;
        }

        /// <summary>
        /// The authentication provider name.
        /// </summary>
        public string Provider { get; }
        /// <summary>
        /// Username, user id, etc.
        /// </summary>
        public string UserIdentifier { get; }
        /// <summary>
        /// The authentication result.
        /// </summary>
        public bool IsAuthentication { get; }
    }
}
