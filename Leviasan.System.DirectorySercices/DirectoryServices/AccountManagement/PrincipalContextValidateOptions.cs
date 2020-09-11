using Microsoft.Extensions.Options;

namespace System.DirectoryServices.AccountManagement
{
    /// <summary>
    /// Represents the validation class for <see cref="PrincipalContextOptions"/>.
    /// </summary>
    public sealed class PrincipalContextValidateOptions : IValidateOptions<PrincipalContextOptions>
    {
        /// <summary>
        /// Validates a specific named options instance (or all when name is null).
        /// </summary>
        /// <param name="name">The name of the options instance being validated.</param>
        /// <param name="options">The options instance.</param>
        public ValidateOptionsResult Validate(string name, PrincipalContextOptions options)
        {
            if (options == null)
                return ValidateOptionsResult.Fail("Configuration object is null.");

            if (options.ContextType == ContextType.ApplicationDirectory && string.IsNullOrWhiteSpace(options.Name))
                return ValidateOptionsResult.Fail($"The {nameof(options.Name)} parameter cannot be null for {ContextType.ApplicationDirectory} context types.");

            if (options.ContextType == ContextType.Machine && !string.IsNullOrWhiteSpace(options.Container))
                return ValidateOptionsResult.Fail($"For {ContextType.Machine} context types, the parameter {nameof(options.Container)} must be set to null.");

            if (options.Username != null && options.Password == null || options.Username == null && options.Password != null)
                return ValidateOptionsResult.Fail($"The {nameof(options.Username)} and {nameof(options.Password)} parameters must either be null or contain a value.");

            return ValidateOptionsResult.Success;
        }
    }
}
