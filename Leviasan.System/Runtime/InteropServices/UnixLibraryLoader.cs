namespace System.Runtime.InteropServices
{
    /// <summary>
    /// Represents the management of a Unix unmanaged library.
    /// </summary>
    public sealed class UnixLibraryLoader : ILibraryLoader
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
        /// <exception cref="InvalidOperationException">If operation is failed and allowed throwing exception return value is exception.</exception>
        public bool FreeLibrary(IntPtr hModule)
        {
            var dlclose = UnixNativeMethods.dlclose(hModule);
            var result = !Convert.ToBoolean(dlclose);
            if (result && ThrowIfError)
                throw new InvalidOperationException(Marshal.PtrToStringAnsi(UnixNativeMethods.dlerror()));

            return result;
        }
        /// <summary>
        /// Retrieves the address of an exported function or variable from the specified dynamic-link library.
        /// </summary>
        /// <param name="hModule">A handle to the loaded library module.</param>
        /// <param name="methodName">The function or variable name.</param>
        /// <returns>If successful the return value is the address of the exported function or variable, otherwise the return value is zero.</returns>
        /// <exception cref="InvalidOperationException">If operation is failed and allowed throwing exception return value is exception.</exception>
        public IntPtr GetProcAddress(IntPtr hModule, string methodName)
        {
            var ptr = UnixNativeMethods.dlsym(hModule, methodName);
            if (ptr == IntPtr.Zero && ThrowIfError)
                throw new InvalidOperationException(Marshal.PtrToStringAnsi(UnixNativeMethods.dlerror()));

            return ptr;
        }
        /// <summary>
        /// Loads the specified module into the address space of the calling process.
        /// </summary>
        /// <param name="fileName">The path to the module.</param>
        /// <returns>If successful the return value is a handle to the module, otherwise the return value is zero.</returns>
        /// <exception cref="InvalidOperationException">If operation is failed and allowed throwing exception return value is exception.</exception>
        public IntPtr LoadLibrary(string fileName)
        {
            var ptr = UnixNativeMethods.dlopen(fileName, (int)RTLD.NOW);
            if (ptr == IntPtr.Zero && ThrowIfError)
                throw new InvalidOperationException(Marshal.PtrToStringAnsi(UnixNativeMethods.dlerror()));

            return ptr;
        }

        /// <summary>
        /// dlopen flags.
        /// </summary>
        [Flags]
        internal enum RTLD
        {
            /// <summary>
            /// All symbols are not made available for relocation processing by other modules.
            /// </summary>
            LOCAL = 0,
            /// <summary>
            /// Relocations are performed at an implementation-defined time.
            /// </summary>
            LAZY = 0x1,
            /// <summary>
            /// Relocations are performed when the object is loaded.
            /// </summary>
            NOW = 0x2,
            /// <summary>
            /// All symbols are available for relocation processing of other modules.
            /// </summary>
            GLOBAL = 0x100
        }
    }
}
