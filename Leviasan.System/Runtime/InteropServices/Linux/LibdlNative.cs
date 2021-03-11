namespace System.Runtime.InteropServices.Linux
{
    /// <summary>
    /// The Unix API.
    /// </summary>
    [Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5392:Use DefaultDllImportSearchPaths attribute for P/Invokes", Justification = "Unix OS ignored DefaultDllImportSearchPaths attrbite")]
    [Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1401:P/Invokes should not be visible", Justification = "Native OS API")]
    public static class LibdlNative
    {
        /// <summary>
        /// Gains access to an executable object file.
        /// </summary>
        /// <param name="fileName">The path to the module.</param>
        /// <param name="flags">The flag.</param>
        /// <returns>
        /// If successful the return value is a handle to the module.
        /// Otherwise if file cannot be found, cannot be opened for reading, is not of an appropriate object format for processing, or if an error occurs during the process of loading file or relocating its symbolic references return zero.
        /// </returns>
        [DllImport("libdl.so", CharSet = CharSet.Ansi, BestFitMapping = false)]
        public static extern IntPtr dlopen(string fileName, RTLD flags);
        /// <summary>
        /// Obtains the address of a symbol from a dlopen object.
        /// </summary>
        /// <param name="handle">A handle to the loaded module that contains the function or variable.</param>
        /// <param name="symbol">The function or variable name.</param>
        /// <returns>
        /// If successful the return value is the address of the exported function or variable. 
        /// Otherwise if handle does not refer to a valid object opened by dlopen, or if the named symbol cannot be found within any of the objects associated with handle returned zero.
        /// </returns>
        [DllImport("libdl.so", CharSet = CharSet.Ansi, BestFitMapping = false)]
        public static extern IntPtr dlsym(IntPtr handle, string symbol);
        /// <summary>
        /// Informs the system that the object referenced by the handle returned from the previous call to dlopen is no longer needed by the application.
        /// </summary>
        /// <param name="handle">A handle to the module that will be closed.</param>
        /// <returns>If the specified object was successfully closed returns 0. If the object cannot be closed, or if the handle is not related to an open object, returns nonzero.</returns>
        [DllImport("libdl.so", CharSet = CharSet.Ansi, BestFitMapping = false)]
        public static extern int dlclose(IntPtr handle);
        /// <summary>
        /// Gets the diagnostic information.
        /// </summary>
        /// <returns>If successful return a null-terminated character string; otherwise, null.</returns>
        [DllImport("libdl.so")]
        public static extern IntPtr dlerror();

        /// <summary>
        /// dlopen flags.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1027:Mark enums with FlagsAttribute", Justification = "Enum is not flag")]
        public enum RTLD
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
