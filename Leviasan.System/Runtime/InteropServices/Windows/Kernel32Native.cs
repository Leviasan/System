namespace System.Runtime.InteropServices.Windows
{
    /// <summary>
    /// WINAPI. Library: kernel32.dll.
    /// </summary>
    [Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1401:P/Invokes should not be visible", Justification = "Native OS API")]
    public static class Kernel32Native
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
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
        /// <summary>
        /// Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count.
        /// When the reference count reaches zero, the module is unloaded from the address space of the calling process and the handle is no longer valid.
        /// </summary>
        /// <param name="hModule">A handle to the loaded library module.</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.</returns>
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);
        /// <summary>
        /// Gets the handle to the window used by the console associated with the calling process or <see cref="IntPtr.Zero"/> if there is no such associated console.
        /// </summary>
        /// <returns>A handle to the window used by the console associated with the calling process or <see cref="IntPtr.Zero"/> if there is no such associated console.</returns>
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("kernel32.dll")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "WINAPI")]
        public static extern IntPtr GetConsoleWindow();
        /// <summary>
        /// Allocates the specified number of bytes from the heap.
        /// </summary>
        /// <param name="uFlags">The memory allocation attributes.</param>
        /// <param name="dwBytes">The number of bytes to allocate. If this parameter is zero and the uFlags parameter specifies GMEM.MOVEABLE, the function returns a handle to a memory object that is marked as discarded.</param>
        /// <returns>If the function succeeds, the return value is a handle to the newly allocated memory object. If the function fails, the return value is zero.</returns>
        /// <remarks>Memory allocated with this function is guaranteed to be aligned on an 8-byte boundary. To free the memory, use the GlobalFree function.</remarks>
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GlobalAlloc(GMEM uFlags, int dwBytes);
        /// <summary>
        /// Locks a global memory object and returns a pointer to the first byte of the object's memory block.
        /// </summary>
        /// <param name="hMem">A handle to the global memory object. This handle is returned by either the GlobalAlloc or GlobalReAlloc function.</param>
        /// <returns>If the function succeeds, the return value is a pointer to the first byte of the memory block. If the function fails, the return value is zero.</returns>
        /// <remarks>
        /// The internal data structures for each memory object include a lock count that is initially zero. For movable memory objects, GlobalLock increments the count by one, and the GlobalUnlock function decrements the count by one. 
        /// Each successful call that a process makes to GlobalLock for an object must be matched by a corresponding call to GlobalUnlock. 
        /// Locked memory will not be moved or discarded, unless the memory object is reallocated by using the GlobalReAlloc function. 
        /// The memory block of a locked memory object remains locked until its lock count is decremented to zero, at which time it can be moved or discarded.
        /// Memory objects allocated with GMEM_FIXED always have a lock count of zero.For these objects, the value of the returned pointer is equal to the value of the specified handle.
        /// If the specified memory block has been discarded or if the memory block has a zero-byte size, this function returns zero.
        /// Discarded objects always have a lock count of zero.
        /// </remarks>
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GlobalLock(IntPtr hMem);
        /// <summary>
        /// Decrements the lock count associated with a memory object that was allocated with GMEM_MOVEABLE. This function has no effect on memory objects allocated with <see cref="GMEM.FIXED"/>.
        /// </summary>
        /// <param name="hMem">A handle to the global memory object. This handle is returned by either the GlobalAlloc or GlobalReAlloc function.</param>
        /// <returns>
        /// If the memory object is still locked after decrementing the lock count, the return value is a nonzero value.
        /// If the memory object is unlocked after decrementing the lock count, the function returns zero and GetLastError returns zero.
        /// If the function fails, the return value is zero and GetLastError returns a value other than zero.
        /// </returns>
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GlobalUnlock(IntPtr hMem);
        /// <summary>
        /// Frees the specified global memory object and invalidates its handle.
        /// </summary>
        /// <param name="hMem">A handle to the global memory object. This handle is returned by either the GlobalAlloc or GlobalReAlloc function.</param>
        /// <returns>
        /// If the function succeeds, the return value is zero.
        /// If the function fails, the return value is equal to a handle to the global memory object.
        /// </returns>
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GlobalFree(IntPtr hMem);

        /// <summary>
        /// Global memory flags.
        /// </summary>
        [Flags]
        [Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1008:Enums should have zero value", Justification = "WINAPI")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1069:Enums values should not be duplicated", Justification = "WINAPI")]
        public enum GMEM : int
        {
            /// <summary>
            /// Allocates fixed memory.
            /// </summary>
            FIXED = 0x0000,
            /// <summary>
            /// Allocates movable memory. Memory blocks are never moved in physical memory, but they can be moved within the default heap.
            /// The return value is a handle to the memory object. To translate the handle into a pointer, use the GlobalLock function.
            /// This value cannot be combined with <see cref="FIXED"/>.
            /// </summary>
            /// <remarks>The movable-memory flags add unnecessary overhead and require locking to be used safely. They should be avoided unless documentation specifically states that they should be used.</remarks>
            MOVEABLE = 0x0002,
            /// <summary>
            /// Initializes memory contents to zero.
            /// </summary>
            ZEROINIT = 0x0040,
            /// <summary>
            /// Combines <see cref="MOVEABLE"/> and <see cref="ZEROINIT"/>.
            /// </summary>
            /// <remarks>The movable-memory flags add unnecessary overhead and require locking to be used safely. They should be avoided unless documentation specifically states that they should be used.</remarks>
            GHND = MOVEABLE | ZEROINIT,
            /// <summary>
            /// Combines <see cref="FIXED"/> and <see cref="ZEROINIT"/>.
            /// </summary>
            GPTR = FIXED | ZEROINIT
        }
    }
}
