namespace System.Runtime.InteropServices
{
    /// <summary>
    /// Provides a mechanism for management of an unmanaged library.
    /// </summary>
    public interface ILibraryLoader
    {
        /// <summary>
        /// If the function succeeds, the return value is the function return value, otherwise thrown exception.
        /// </summary>
        public bool ThrowIfError { get; set; }

        /// <summary>
        /// Frees the loaded dynamic-link library module and, if necessary, decrements its reference count.
        /// </summary>
        /// <param name="hModule">A handle to the loaded library module.</param>
        /// <returns>If releasing is successful return true, otherwise is false.</returns>
        bool FreeLibrary(IntPtr hModule);
        /// <summary>
        /// Retrieves the address of an exported function or variable from the specified dynamic-link library.
        /// </summary>
        /// <param name="hModule">A handle to the loaded library module.</param>
        /// <param name="methodName">The function or variable name.</param>
        /// <returns>If successful the return value is the address of the exported function or variable, otherwise the return value is zero.</returns>
        IntPtr GetProcAddress(IntPtr hModule, string methodName);
        /// <summary>
        /// Loads the specified module into the address space of the calling process.
        /// </summary>
        /// <param name="fileName">The path to the module.</param>
        /// <returns>If successful the return value is a handle to the module, otherwise the return value is zero.</returns>
        IntPtr LoadLibrary(string fileName);
    }
}
