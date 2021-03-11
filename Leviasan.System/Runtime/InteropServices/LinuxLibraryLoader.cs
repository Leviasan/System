using System.Runtime.InteropServices.Linux;

namespace System.Runtime.InteropServices
{
    /// <summary>
    /// Represents the management of a Unix unmanaged library.
    /// </summary>
    public sealed class LinuxLibraryLoader : ILibraryLoader
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
            var dlclose = LibdlNative.dlclose(hModule);
            var result = !Convert.ToBoolean(dlclose);
            if (result && ThrowIfError)
                throw new InvalidOperationException(Marshal.PtrToStringAnsi(LibdlNative.dlerror()));

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
            var ptr = LibdlNative.dlsym(hModule, methodName);
            if (ptr == IntPtr.Zero && ThrowIfError)
                throw new InvalidOperationException(Marshal.PtrToStringAnsi(LibdlNative.dlerror()));

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
            var ptr = LibdlNative.dlopen(fileName, LibdlNative.RTLD.NOW);
            if (ptr == IntPtr.Zero && ThrowIfError)
                throw new InvalidOperationException(Marshal.PtrToStringAnsi(LibdlNative.dlerror()));

            return ptr;
        }
    }
}
