namespace System.Runtime.InteropServices
{
    /// <summary>
    /// The Microsoft Windows API.
    /// </summary>
    internal static class WindowsNativeMethods
    {
        /// <summary>
        /// Loads the specified module into the address space of the calling process.
        /// </summary>
        /// <param name="fileName">
        /// The name of the module. This can be either a library module or an executable module. 
        /// If the string specifies a full path, the function searches only that path for the module.</param>
        /// <returns>If the function succeeds, the return value is a handle to the module. If the function fails, the return value is NULL.</returns>
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string fileName);
        /// <summary>
        /// Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
        /// </summary>
        /// <param name="hModule">A handle to the DLL module that contains the function or variable.</param>
        /// <param name="procName">The function or variable name, or the function's ordinal value. If this parameter is an ordinal value, it must be in the low-order word; the high-order word must be zero.</param>
        /// <returns>If the function succeeds, the return value is the address of the exported function or variable. If the function fails, the return value is NULL.</returns>
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, BestFitMapping = false, ExactSpelling = true, ThrowOnUnmappableChar = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
        /// <summary>
        /// Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count.
        /// When the reference count reaches zero, the module is unloaded from the address space of the calling process and the handle is no longer valid.
        /// </summary>
        /// <param name="hModule">A handle to the loaded library module.</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.</returns>
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);
    }
    /// <summary>
    /// The Unix API.
    /// </summary>
    [Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5392:Use DefaultDllImportSearchPaths attribute for P/Invokes", Justification = "The attribute is ignored on Linux")]
    internal static class UnixNativeMethods
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
        public static extern IntPtr dlopen(string fileName, int flags);
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
        /// Informs the system that the object referenced by the handle returned from the previous call to <see cref="dlopen(string, int)"/> is no longer needed by the application.
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
    }
}
