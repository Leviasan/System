namespace System.Runtime.InteropServices
{
    /// <summary>
    /// Represents a helper class for determining which operating system the current application is running on.
    /// </summary>
    public static class OperatingSystem
    {
        /// <summary>
        /// Indicates whether the current application is running on the Windows platform.
        /// </summary>
        /// <returns>true if the current app is running on the Windows platform; otherwise, false.</returns>
        public static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        /// <summary>
        /// Indicates whether the current application is running on the OSX platform.
        /// </summary>
        /// <returns>true if the current app is running on the OSX platform; otherwise, false.</returns>
        public static bool IsMacOS() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        /// <summary>
        /// Indicates whether the current application is running on the Linux platform.
        /// </summary>
        /// <returns>true if the current app is running on the Linux platform; otherwise, false.</returns>
        public static bool IsLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }
}
