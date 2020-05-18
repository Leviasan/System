namespace System
{
    /// <summary>
    /// Type class extensions.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets the path to the section in appsettings.json based on the full name of the type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>The path to the section.</returns>
        public static string GetConfigurePath(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var path = type.FullName.Replace('.', ':');
            return path;
        }
    }
}
