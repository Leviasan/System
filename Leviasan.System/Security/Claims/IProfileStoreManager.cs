﻿using System.Collections.Generic;

namespace System.Security.Claims
{
    /// <summary>
    /// Represents the service for authenticating users through registered profile stores.
    /// </summary>
    public interface IProfileStoreManager : IProfileStore
    {
        /// <summary>
        /// The profile stores.
        /// </summary>
        IReadOnlyDictionary<string, IProfileStore> Stores { get; }

        /// <summary>
        /// Gets claims identity if any from a specified profile store.
        /// </summary>
        /// <param name="provider">The profile store name.</param>
        /// <param name="userId">The user ID.</param>
        /// <returns>If user authentication is success, the property <see cref="ClaimsIdentity.IsAuthenticated"/> will be true.</returns>
        ClaimsIdentity FindById(string provider, string userId);
        /// <summary>
        /// Gets claims identity if any from a specified profile store.
        /// </summary>
        /// <param name="provider">The profile store name.</param>
        /// <param name="username">The account name.</param>
        /// <returns>If user authentication is success, the property <see cref="ClaimsIdentity.IsAuthenticated"/> will be true.</returns>
        ClaimsIdentity FindByName(string provider, string username);
        /// <summary>
        /// Gets account identifier if any from a specified profile store.
        /// </summary>
        /// <param name="provider">The profile store name.</param>
        /// <param name="username">The account name.</param>
        /// <returns>The user identifier, if any, otherwise null.</returns>
        string FindUserIdByName(string provider, string username);
        /// <summary>
        /// Checks is account locked out from a specified profile store.
        /// </summary>
        /// <param name="provider">The profile store name.</param>
        /// <param name="username">The account name.</param>
        bool IsLockedOut(string provider, string username);
        /// <summary>
        /// Validates the credentials from a specified profile store.
        /// </summary>
        /// <param name="provider">The profile store name.</param>
        /// <param name="username">The account name.</param>
        /// <param name="password">The account password.</param>
        bool IsValidate(string provider, string username, string password);
    }
}