namespace System.Runtime.InteropServices.Windows
{
    /// <summary>
    /// WINAPI. Library: user32.dll.
    /// </summary>
    [Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "WINAPI")]
    [Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1401:P/Invokes should not be visible", Justification = "Native OS API")]
    public static class User32Native
    {
        /// <summary>
        /// Retrieves a message from the calling thread's message queue. The function dispatches incoming sent messages until a posted message is available for retrieval.
        /// </summary>
        /// <param name="lpMsg">A pointer to an MSG structure that receives message information from the thread's message queue.</param>
        /// <param name="hWnd">A handle to the window whose messages are to be retrieved. The window must belong to the current thread.</param>
        /// <param name="wMsgFilterMin">The integer value of the lowest message value to be retrieved.</param>
        /// <param name="wMsgFilterMax">The integer value of the highest message value to be retrieved.</param>
        /// <returns>
        /// If the function retrieves a message other than WM_QUIT, the return value is nonzero.
        /// If the function retrieves the WM_QUIT message, the return value is zero.
        /// If there is an error, the return value is -1.
        /// </returns>
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetMessage(ref MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);
        /// <summary>
        /// Dispatches incoming sent messages, checks the thread message queue for a posted message, and retrieves the message (if any exist).
        /// </summary>
        /// <param name="lpMsg">A pointer to an MSG structure that receives message information.</param>
        /// <param name="hWnd">A handle to the window whose messages are to be retrieved. The window must belong to the current thread.</param>
        /// <param name="wMsgFilterMin">The value of the first message in the range of messages to be examined.</param>
        /// <param name="wMsgFilterMax">The value of the last message in the range of messages to be examined.</param>
        /// <param name="wRemoveMsg">Specifies how messages are to be handled.</param>
        /// <returns>
        /// If a message is available, the return value is nonzero.
        /// If no messages are available, the return value is zero.
        /// </returns>
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("user32.dll")]
        public static extern int PeekMessage(ref MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, PM wRemoveMsg);
        /// <summary>
        /// Translates virtual-key messages into character messages. The character messages are posted to the calling thread's message queue, to be read the next time the thread calls the GetMessage or PeekMessage function.
        /// </summary>
        /// <param name="lpMsg">A pointer to an MSG structure that contains message information retrieved from the calling thread's message queue by using the GetMessage or PeekMessage function.</param>
        /// <returns>
        /// If the message is translated (that is, a character message is posted to the thread's message queue), the return value is nonzero.
        /// If the message is WM_KEYDOWN, WM_KEYUP, WM_SYSKEYDOWN, or WM_SYSKEYUP, the return value is nonzero, regardless of the translation.
        /// If the message is not translated (that is, a character message is not posted to the thread's message queue), the return value is zero.
        /// </returns>
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("user32.dll")]
        public static extern int TranslateMessage(ref MSG lpMsg);
        /// <summary>
        /// Dispatches a message to a window procedure. It is typically used to dispatch a message retrieved by the GetMessage function.
        /// </summary>
        /// <param name="lpMsg">A pointer to a structure that contains the message.</param>
        /// <returns>The return value specifies the value returned by the window procedure. Although its meaning depends on the message being dispatched, the return value generally is ignored.</returns>
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage(ref MSG lpMsg);
        /// <summary>
        /// Indicates to the system that a thread has made a request to terminate (quit). It is typically used in response to a WM_DESTROY message.
        /// </summary>
        /// <param name="nExitCode">The application exit code. This value is used as the wParam parameter of the WM_QUIT message.</param>
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("user32.dll")]
        public static extern void PostQuitMessage(int nExitCode);
        /// <summary>
        /// Determines whether a message is intended for the specified dialog box and, if it is, processes the message.
        /// </summary>
        /// <param name="hDlg">A handle to the dialog box.</param>
        /// <param name="lpMsg">A pointer to an MSG structure that contains the message to be checked.</param>
        /// <returns>
        /// If the message has been processed, the return value is nonzero.
        /// If the message has not been processed, the return value is zero.
        /// </returns>
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsDialogMessage(IntPtr hDlg, ref MSG lpMsg);
        /// <summary>
        /// Retrieves the type of messages found in the calling thread's message queue.
        /// </summary>
        /// <param name="flags">The types of messages for which to check.</param>
        /// <returns>
        /// The high-order word of the return value indicates the types of messages currently in the queue. 
        /// The low-order word indicates the types of messages that have been added to the queue and that are still in the queue since the last call to the GetQueueStatus, GetMessage, or PeekMessage function.
        /// </returns>
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("user32.dll")]
        public static extern uint GetQueueStatus(QS flags);

        /// <summary>
        /// Contains message information from a thread's message queue.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "WINAPI")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "WINAPI")]
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct MSG
        {
            /// <summary>
            /// A handle to the window whose window procedure receives the message. This member is NULL when the message is a thread message.
            /// </summary>
            public IntPtr hwnd;
            /// <summary>
            /// The message identifier. Applications can only use the low word; the high word is reserved by the system.
            /// </summary>
            public int message;
            /// <summary>
            /// Additional information about the message. The exact meaning depends on the value of the message member.
            /// </summary>
            public UIntPtr wParam;
            /// <summary>
            /// Additional information about the message. The exact meaning depends on the value of the message member.
            /// </summary>
            public IntPtr lParam;
            /// <summary>
            /// The time at which the message was posted.
            /// </summary>
            public uint time;
            /// <summary>
            /// The cursor position, in screen coordinates, when the message was posted.
            /// </summary>
            public POINT pt;
        }
        /// <summary>
        /// The POINT structure defines the x- and y- coordinates of a point.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "WINAPI")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "WINAPI")]
        public struct POINT
        {
            /// <summary>
            /// The x-coordinate of the point.
            /// </summary>
            public int x;
            /// <summary>
            /// The y-coordinate of the point.
            /// </summary>
            public int y;
        }
        /// <summary>
        /// The flags in GetQueueStatus function.
        /// </summary>
        [Flags]
        public enum QS
        {
            /// <summary>
            /// A WM_KEYUP, WM_KEYDOWN, WM_SYSKEYUP, or WM_SYSKEYDOWN message is in the queue.
            /// </summary>
            KEY = 0x0001,
            /// <summary>
            /// A WM_MOUSEMOVE message is in the queue.
            /// </summary>
            MOUSEMOVE = 0x0002,
            /// <summary>
            /// A mouse-button message (WM_LBUTTONUP, WM_RBUTTONDOWN, and so on).
            /// </summary>
            MOUSEBUTTON = 0x0004,
            /// <summary>
            /// A WM_MOUSEMOVE message or mouse-button message (WM_LBUTTONUP, WM_RBUTTONDOWN, and so on).
            /// </summary>
            MOUSE = MOUSEMOVE | MOUSEBUTTON,
            /// <summary>
            /// A posted message (other than those listed here) is in the queue.
            /// </summary>
            POSTMESSAGE = 0x0008,
            /// <summary>
            /// A WM_TIMER message is in the queue.
            /// </summary>
            TIMER = 0x0010,
            /// <summary>
            /// A WM_PAINT message is in the queue.
            /// </summary>
            PAINT = 0x0020,
            /// <summary>
            /// A message sent by another thread or application is in the queue.
            /// </summary>
            SENDMESSAGE = 0x0040,
            /// <summary>
            /// A WM_HOTKEY message is in the queue.
            /// </summary>
            HOTKEY = 0x0080,
            /// <summary>
            /// A posted message (other than those listed here) is in the queue.
            /// </summary>
            ALLPOSTMESSAGE = 0x0100,
            /// <summary>
            /// A raw input message is in the queue. For more information, see Raw Input. Windows 2000: This flag is not supported.
            /// </summary>
            RAWINPUT = 0x0400,
            /// <summary>
            /// An input message is in the queue.
            /// </summary>
            INPUT = MOUSE | KEY | RAWINPUT,
            /// <summary>
            /// An input, WM_TIMER, WM_PAINT, WM_HOTKEY, or posted message is in the queue.
            /// </summary>
            ALLEVENTS = INPUT | POSTMESSAGE | TIMER | PAINT | HOTKEY,
            /// <summary>
            /// Any message is in the queue.
            /// </summary>
            ALLINPUT = ALLEVENTS | SENDMESSAGE
        }
        /// <summary>
        /// Specifies how messages are to be handled.
        /// </summary>
        [Flags]
        [Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1008:Enums should have zero value", Justification = "WINAPI")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "WINAPI")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2217:Do not mark enums with FlagsAttribute", Justification = "WINAPI")]
        public enum PM
        {
            /// <summary>
            /// Messages are not removed from the queue after processing by PeekMessage.
            /// </summary>
            NOREMOVE = 0x0000,
            /// <summary>
            /// Messages are removed from the queue after processing by PeekMessage.
            /// </summary>
            REMOVE = 0x0001,
            /// <summary>
            /// Prevents the system from releasing any thread that is waiting for the caller to go idle. Combine this value with either NOREMOVE or REMOVE.
            /// </summary>
            NOYIELD = 0x0002,

            #region By default, all message types are processed. To specify that only certain message should be processed, specify one or more of the following values.
            /// <summary>
            /// Process mouse and keyboard messages.
            /// </summary>
            QS_INPUT = QS.INPUT << 16,
            /// <summary>
            /// Process paint messages.
            /// </summary>
            QS_PAINT = QS.PAINT << 16,
            /// <summary>
            /// Process all posted messages, including timers and hotkeys.
            /// </summary>
            QS_POSTMESSAGE = (QS.POSTMESSAGE | QS.HOTKEY | QS.TIMER) << 16,
            /// <summary>
            /// Process all sent messages.
            /// </summary>
            QS_SENDMESSAGE = QS.SENDMESSAGE << 16
            #endregion
        }
    }
}
