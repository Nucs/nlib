using System;
using System.Drawing;
using System.Runtime.InteropServices;
namespace nucs.WinForms {

    internal static partial class NativeWin32 {
        [DllImport("kernel32.dll")]
        public static extern int FreeConsole();
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int ShowCursor(bool show);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void mouse_event(int flags, int dX, int dY, int buttons, int extraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        // The GetWindowThreadProcessId function retrieves the identifier of the thread
        // that created the specified window and, optionally, the identifier of the
        // process that created the window.
        [DllImport("user32.dll", SetLastError = true)]
        public static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RegisterHotKey(IntPtr hwnd, int uid, uint fsModifiers, uint vk);

        [DllImport("user32", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hwnd, int uid);

        [DllImport("kernel32", SetLastError = true)]
        public static extern short GlobalAddAtom(string lpString);

        [DllImport("kernel32", SetLastError = true)]
        public static extern short GlobalDeleteAtom(short nAtom);

        [DllImport("kernel32")]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AttachConsole(int dwProcessId);

        [DllImport("user32.dll")]
        public static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        public static extern IntPtr GetKeyboardLayout(UIntPtr idThread);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern Int32 GetLastError();

        public static uint GetTopMostThreadId() {
            return GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
        }

        /// <summary>
        /// The SendInput function synthesizes keystrokes, mouse motions, and button clicks.
        /// 
        /// </summary>
        /// <param name="numberOfInputs">Number of structures in the Inputs array.</param><param name="inputs">Pointer to an array of INPUT structures. Each structure represents an event to be inserted into the keyboard or mouse input stream.</param><param name="sizeOfInputStructure">Specifies the size, in bytes, of an INPUT structure. If cbSize is not the size of an INPUT structure, the function fails.</param>
        /// <returns>
        /// The function returns the number of events that it successfully inserted into the keyboard or mouse input stream. If the function returns zero, the input was already blocked by another thread. To get extended error information, call GetLastError.Microsoft Windows Vista. This function fails when it is blocked by User Interface Privilege Isolation (UIPI). Note that neither GetLastError nor the return value will indicate the failure was caused by UIPI blocking.
        /// </returns>
        /// 
        /// <remarks>
        /// Microsoft Windows Vista. This function is subject to UIPI. Applications are permitted to inject input only into applications that are at an equal or lesser integrity level.
        ///             The SendInput function inserts the events in the INPUT structures serially into the keyboard or mouse input stream. These events are not interspersed with other keyboard or mouse input events inserted either by the user (with the keyboard or mouse) or by calls to keybd_event, mouse_event, or other calls to SendInput.
        ///             This function does not reset the keyboard's current state. Any keys that are already pressed when the function is called might interfere with the events that this function generates. To avoid this problem, check the keyboard's state with the GetAsyncKeyState function and correct as necessary.
        /// 
        /// </remarks>
        [DllImport("User32.dll")]
        internal static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);
        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646270(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct INPUT
        {
            public uint Type;
            public MOUSEKEYBDHARDWAREINPUT Data;
        }

        /// <summary>
        /// http://social.msdn.microsoft.com/Forums/en/csharplanguage/thread/f0e82d6e-4999-4d22-b3d3-32b25f61fb2a
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        internal struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public HARDWAREINPUT Hardware;
            [FieldOffset(0)]
            public KEYBDINPUT Keyboard;
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct HARDWAREINPUT
        {
            public uint Msg;
            public ushort ParamL;
            public ushort ParamH;
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            public ushort Vk;
            public ushort Scan;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        /// <summary>
        /// http://social.msdn.microsoft.com/forums/en-US/netfxbcl/thread/2abc6be8-c593-4686-93d2-89785232dacd
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            public int X;
            public int Y;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        public enum KeyCode : ushort
        {
            #region Media

            /// <summary>
            /// Next track if a song is playing
            /// </summary>
            MEDIA_NEXT_TRACK = 0xb0,

            /// <summary>
            /// Play pause
            /// </summary>
            MEDIA_PLAY_PAUSE = 0xb3,

            /// <summary>
            /// Previous track
            /// </summary>
            MEDIA_PREV_TRACK = 0xb1,

            /// <summary>
            /// Stop
            /// </summary>
            MEDIA_STOP = 0xb2,

            #endregion

            #region math

            /// <summary>Key "+"</summary>
            ADD = 0x6b,
            /// <summary>
            /// "*" key
            /// </summary>
            MULTIPLY = 0x6a,

            /// <summary>
            /// "/" key
            /// </summary>
            DIVIDE = 0x6f,

            /// <summary>
            /// Subtract key "-"
            /// </summary>
            SUBTRACT = 0x6d,

            #endregion

            #region Browser
            /// <summary>
            /// Go Back
            /// </summary>
            BROWSER_BACK = 0xa6,
            /// <summary>
            /// Favorites
            /// </summary>
            BROWSER_FAVORITES = 0xab,
            /// <summary>
            /// Forward
            /// </summary>
            BROWSER_FORWARD = 0xa7,
            /// <summary>
            /// Home
            /// </summary>
            BROWSER_HOME = 0xac,
            /// <summary>
            /// Refresh
            /// </summary>
            BROWSER_REFRESH = 0xa8,
            /// <summary>
            /// browser search
            /// </summary>
            BROWSER_SEARCH = 170,
            /// <summary>
            /// Stop
            /// </summary>
            BROWSER_STOP = 0xa9,
            #endregion

            #region Numpad numbers
            /// <summary>
            /// 
            /// </summary>
            NUMPAD0 = 0x60,
            /// <summary>
            /// 
            /// </summary>
            NUMPAD1 = 0x61,
            /// <summary>
            /// 
            /// </summary>
            NUMPAD2 = 0x62,
            /// <summary>
            /// 
            /// </summary>
            NUMPAD3 = 0x63,
            /// <summary>
            /// 
            /// </summary>
            NUMPAD4 = 100,
            /// <summary>
            /// 
            /// </summary>
            NUMPAD5 = 0x65,
            /// <summary>
            /// 
            /// </summary>
            NUMPAD6 = 0x66,
            /// <summary>
            /// 
            /// </summary>
            NUMPAD7 = 0x67,
            /// <summary>
            /// 
            /// </summary>
            NUMPAD8 = 0x68,
            /// <summary>
            /// 
            /// </summary>
            NUMPAD9 = 0x69,

            #endregion

            #region Fkeys
            /// <summary>
            /// F1
            /// </summary>
            F1 = 0x70,
            /// <summary>
            /// F10
            /// </summary>
            F10 = 0x79,
            /// <summary>
            /// 
            /// </summary>
            F11 = 0x7a,
            /// <summary>
            /// 
            /// </summary>
            F12 = 0x7b,
            /// <summary>
            /// 
            /// </summary>
            F13 = 0x7c,
            /// <summary>
            /// 
            /// </summary>
            F14 = 0x7d,
            /// <summary>
            /// 
            /// </summary>
            F15 = 0x7e,
            /// <summary>
            /// 
            /// </summary>
            F16 = 0x7f,
            /// <summary>
            /// 
            /// </summary>
            F17 = 0x80,
            /// <summary>
            /// 
            /// </summary>
            F18 = 0x81,
            /// <summary>
            /// 
            /// </summary>
            F19 = 130,
            /// <summary>
            /// 
            /// </summary>
            F2 = 0x71,
            /// <summary>
            /// 
            /// </summary>
            F20 = 0x83,
            /// <summary>
            /// 
            /// </summary>
            F21 = 0x84,
            /// <summary>
            /// 
            /// </summary>
            F22 = 0x85,
            /// <summary>
            /// 
            /// </summary>
            F23 = 0x86,
            /// <summary>
            /// 
            /// </summary>
            F24 = 0x87,
            /// <summary>
            /// 
            /// </summary>
            F3 = 0x72,
            /// <summary>
            /// 
            /// </summary>
            F4 = 0x73,
            /// <summary>
            /// 
            /// </summary>
            F5 = 0x74,
            /// <summary>
            /// 
            /// </summary>
            F6 = 0x75,
            /// <summary>
            /// 
            /// </summary>
            F7 = 0x76,
            /// <summary>
            /// 
            /// </summary>
            F8 = 0x77,
            /// <summary>
            /// 
            /// </summary>
            F9 = 120,

            #endregion

            #region Other
            /// <summary>
            /// 
            /// </summary>
            OEM_1 = 0xba,
            /// <summary>
            /// 
            /// </summary>
            OEM_102 = 0xe2,
            /// <summary>
            /// 
            /// </summary>
            OEM_2 = 0xbf,
            /// <summary>
            /// 
            /// </summary>
            OEM_3 = 0xc0,
            /// <summary>
            /// 
            /// </summary>
            OEM_4 = 0xdb,
            /// <summary>
            /// 
            /// </summary>
            OEM_5 = 220,
            /// <summary>
            /// 
            /// </summary>
            OEM_6 = 0xdd,
            /// <summary>
            /// 
            /// </summary>
            OEM_7 = 0xde,
            /// <summary>
            /// 
            /// </summary>
            OEM_8 = 0xdf,
            /// <summary>
            /// 
            /// </summary>
            OEM_CLEAR = 0xfe,
            /// <summary>
            /// 
            /// </summary>
            OEM_COMMA = 0xbc,
            /// <summary>
            /// 
            /// </summary>
            OEM_MINUS = 0xbd,
            /// <summary>
            /// 
            /// </summary>
            OEM_PERIOD = 190,
            /// <summary>
            /// 
            /// </summary>
            OEM_PLUS = 0xbb,

            #endregion

            #region KEYS

            /// <summary>
            /// 
            /// </summary>
            KEY_0 = 0x30,
            /// <summary>
            /// 
            /// </summary>
            KEY_1 = 0x31,
            /// <summary>
            /// 
            /// </summary>
            KEY_2 = 50,
            /// <summary>
            /// 
            /// </summary>
            KEY_3 = 0x33,
            /// <summary>
            /// 
            /// </summary>
            KEY_4 = 0x34,
            /// <summary>
            /// 
            /// </summary>
            KEY_5 = 0x35,
            /// <summary>
            /// 
            /// </summary>
            KEY_6 = 0x36,
            /// <summary>
            /// 
            /// </summary>
            KEY_7 = 0x37,
            /// <summary>
            /// 
            /// </summary>
            KEY_8 = 0x38,
            /// <summary>
            /// 
            /// </summary>
            KEY_9 = 0x39,
            /// <summary>
            /// 
            /// </summary>
            KEY_A = 0x41,
            /// <summary>
            /// 
            /// </summary>
            KEY_B = 0x42,
            /// <summary>
            /// 
            /// </summary>
            KEY_C = 0x43,
            /// <summary>
            /// 
            /// </summary>
            KEY_D = 0x44,
            /// <summary>
            /// 
            /// </summary>
            KEY_E = 0x45,
            /// <summary>
            /// 
            /// </summary>
            KEY_F = 70,
            /// <summary>
            /// 
            /// </summary>
            KEY_G = 0x47,
            /// <summary>
            /// 
            /// </summary>
            KEY_H = 0x48,
            /// <summary>
            /// 
            /// </summary>
            KEY_I = 0x49,
            /// <summary>
            /// 
            /// </summary>
            KEY_J = 0x4a,
            /// <summary>
            /// 
            /// </summary>
            KEY_K = 0x4b,
            /// <summary>
            /// 
            /// </summary>
            KEY_L = 0x4c,
            /// <summary>
            /// 
            /// </summary>
            KEY_M = 0x4d,
            /// <summary>
            /// 
            /// </summary>
            KEY_N = 0x4e,
            /// <summary>
            /// 
            /// </summary>
            KEY_O = 0x4f,
            /// <summary>
            /// 
            /// </summary>
            KEY_P = 80,
            /// <summary>
            /// 
            /// </summary>
            KEY_Q = 0x51,
            /// <summary>
            /// 
            /// </summary>
            KEY_R = 0x52,
            /// <summary>
            /// 
            /// </summary>
            KEY_S = 0x53,
            /// <summary>
            /// 
            /// </summary>
            KEY_T = 0x54,
            /// <summary>
            /// 
            /// </summary>
            KEY_U = 0x55,
            /// <summary>
            /// 
            /// </summary>
            KEY_V = 0x56,
            /// <summary>
            /// 
            /// </summary>
            KEY_W = 0x57,
            /// <summary>
            /// 
            /// </summary>
            KEY_X = 0x58,
            /// <summary>
            /// 
            /// </summary>
            KEY_Y = 0x59,
            /// <summary>
            /// 
            /// </summary>
            KEY_Z = 90,

            #endregion

            #region volume
            /// <summary>
            /// Decrese volume
            /// </summary>
            VOLUME_DOWN = 0xae,

            /// <summary>
            /// Mute volume
            /// </summary>
            VOLUME_MUTE = 0xad,

            /// <summary>
            /// Increase volue
            /// </summary>
            VOLUME_UP = 0xaf,

            #endregion


            /// <summary>
            /// Take snapshot of the screen and place it on the clipboard
            /// </summary>
            SNAPSHOT = 0x2c,

            /// <summary>Send right click from keyboard "key that is 2 keys to the right of space bar"</summary>
            RightClick = 0x5d,

            /// <summary>
            /// Go Back or delete
            /// </summary>
            BACKSPACE = 8,

            /// <summary>
            /// Control + Break "When debuging if you step into an infinite loop this will stop debug"
            /// </summary>
            CANCEL = 3,
            /// <summary>
            /// Caps lock key to send cappital letters
            /// </summary>
            CAPS_LOCK = 20,
            /// <summary>
            /// Ctlr key
            /// </summary>
            CONTROL = 0x11,

            /// <summary>
            /// Alt key
            /// </summary>
            ALT = 18,

            /// <summary>
            /// "." key
            /// </summary>
            DECIMAL = 110,

            /// <summary>
            /// Delete Key
            /// </summary>
            DELETE = 0x2e,


            /// <summary>
            /// Arrow down key
            /// </summary>
            DOWN = 40,

            /// <summary>
            /// End key
            /// </summary>
            END = 0x23,

            /// <summary>
            /// Escape key
            /// </summary>
            ESC = 0x1b,

            /// <summary>
            /// Home key
            /// </summary>
            HOME = 0x24,

            /// <summary>
            /// Insert key
            /// </summary>
            INSERT = 0x2d,

            /// <summary>
            /// Open my computer
            /// </summary>
            LAUNCH_APP1 = 0xb6,
            /// <summary>
            /// Open calculator
            /// </summary>
            LAUNCH_APP2 = 0xb7,

            /// <summary>
            /// Open default email in my case outlook
            /// </summary>
            LAUNCH_MAIL = 180,

            /// <summary>
            /// Opend default media player (itunes, winmediaplayer, etc)
            /// </summary>
            LAUNCH_MEDIA_SELECT = 0xb5,

            /// <summary>
            /// Left control
            /// </summary>
            LCONTROL = 0xa2,

            /// <summary>
            /// Left arrow
            /// </summary>
            LEFT = 0x25,

            /// <summary>
            /// Left shift
            /// </summary>
            LSHIFT = 160,

            /// <summary>
            /// left windows key
            /// </summary>
            LWIN = 0x5b,


            /// <summary>
            /// Next "page down"
            /// </summary>
            PAGEDOWN = 0x22,

            /// <summary>
            /// Num lock to enable typing numbers
            /// </summary>
            NUMLOCK = 0x90,

            /// <summary>
            /// Page up key
            /// </summary>
            PAGE_UP = 0x21,

            /// <summary>
            /// Right control
            /// </summary>
            RCONTROL = 0xa3,

            /// <summary>
            /// Return key
            /// </summary>
            ENTER = 13,

            /// <summary>
            /// Right arrow key
            /// </summary>
            RIGHT = 0x27,

            /// <summary>
            /// Right shift
            /// </summary>
            RSHIFT = 0xa1,

            /// <summary>
            /// Right windows key
            /// </summary>
            RWIN = 0x5c,

            /// <summary>
            /// Shift key
            /// </summary>
            SHIFT = 0x10,

            /// <summary>
            /// Space back key
            /// </summary>
            SPACE_BAR = 0x20,

            /// <summary>
            /// Tab key
            /// </summary>
            TAB = 9,

            /// <summary>
            /// Up arrow key
            /// </summary>
            UP = 0x26,

        }
        /// <summary>
        /// The GetAsyncKeyState function determines whether a key is up or down at the time the function is called, and whether the key was pressed after a previous call to GetAsyncKeyState. (See: http://msdn.microsoft.com/en-us/library/ms646293(VS.85).aspx)
        /// 
        /// </summary>
        /// <param name="virtualKeyCode">Specifies one of 256 possible virtual-key codes. For more information, see Virtual Key Codes. Windows NT/2000/XP: You can use left- and right-distinguishing constants to specify certain keys. See the Remarks section for further information.</param>
        /// <returns>
        /// If the function succeeds, the return value specifies whether the key was pressed since the last call to GetAsyncKeyState, and whether the key is currently up or down. If the most significant bit is set, the key is down, and if the least significant bit is set, the key was pressed after the previous call to GetAsyncKeyState. However, you should not rely on this last behavior; for more information, see the Remarks.
        /// 
        ///             Windows NT/2000/XP: The return value is zero for the following cases:
        ///             - The current desktop is not the active desktop
        ///             - The foreground thread belongs to another process and the desktop does not allow the hook or the journal record.
        /// 
        ///             Windows 95/98/Me: The return value is the global asynchronous key state for each virtual key. The system does not check which thread has the keyboard focus.
        /// 
        ///             Windows 95/98/Me: Windows 95 does not support the left- and right-distinguishing constants. If you call GetAsyncKeyState with these constants, the return value is zero.
        /// 
        /// </returns>
        /// 
        /// <remarks>
        /// The GetAsyncKeyState function works with mouse buttons. However, it checks on the state of the physical mouse buttons, not on the logical mouse buttons that the physical buttons are mapped to. For example, the call GetAsyncKeyState(VK_LBUTTON) always returns the state of the left physical mouse button, regardless of whether it is mapped to the left or right logical mouse button. You can determine the system's current mapping of physical mouse buttons to logical mouse buttons by calling
        ///             Copy CodeGetSystemMetrics(SM_SWAPBUTTON) which returns TRUE if the mouse buttons have been swapped.
        /// 
        ///             Although the least significant bit of the return value indicates whether the key has been pressed since the last query, due to the pre-emptive multitasking nature of Windows, another application can call GetAsyncKeyState and receive the "recently pressed" bit instead of your application. The behavior of the least significant bit of the return value is retained strictly for compatibility with 16-bit Windows applications (which are non-preemptive) and should not be relied upon.
        /// 
        ///             You can use the virtual-key code constants VK_SHIFT, VK_CONTROL, and VK_MENU as values for the vKey parameter. This gives the state of the SHIFT, CTRL, or ALT keys without distinguishing between left and right.
        /// 
        ///             Windows NT/2000/XP: You can use the following virtual-key code constants as values for vKey to distinguish between the left and right instances of those keys.
        /// 
        ///             Code Meaning
        ///             VK_LSHIFT Left-shift key.
        ///             VK_RSHIFT Right-shift key.
        ///             VK_LCONTROL Left-control key.
        ///             VK_RCONTROL Right-control key.
        ///             VK_LMENU Left-menu key.
        ///             VK_RMENU Right-menu key.
        /// 
        ///             These left- and right-distinguishing constants are only available when you call the GetKeyboardState, SetKeyboardState, GetAsyncKeyState, GetKeyState, and MapVirtualKey functions.
        /// 
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        public static extern short GetAsyncKeyState(ushort virtualKeyCode);

        /// <summary>
        /// The GetKeyState function retrieves the status of the specified virtual key. The status specifies whether the key is up, down, or toggled (on, off alternating each time the key is pressed). (See: http://msdn.microsoft.com/en-us/library/ms646301(VS.85).aspx)
        /// 
        /// </summary>
        /// <param name="virtualKeyCode">Specifies a virtual key. If the desired virtual key is a letter or digit (A through Z, a through z, or 0 through 9), nVirtKey must be set to the ASCII value of that character. For other keys, it must be a virtual-key code.
        ///             If a non-English keyboard layout is used, virtual keys with values in the range ASCII A through Z and 0 through 9 are used to specify most of the character keys. For example, for the German keyboard layout, the virtual key of value ASCII O (0x4F) refers to the "o" key, whereas VK_OEM_1 refers to the "o with umlaut" key.
        ///             </param>
        /// <returns>
        /// The return value specifies the status of the specified virtual key, as follows:
        ///             If the high-order bit is 1, the key is down; otherwise, it is up.
        ///             If the low-order bit is 1, the key is toggled. A key, such as the CAPS LOCK key, is toggled if it is turned on. The key is off and untoggled if the low-order bit is 0. A toggle key's indicator light (if any) on the keyboard will be on when the key is toggled, and off when the key is untoggled.
        /// 
        /// </returns>
        /// 
        /// <remarks>
        /// The key status returned from this function changes as a thread reads key messages from its message queue. The status does not reflect the interrupt-level state associated with the hardware. Use the GetAsyncKeyState function to retrieve that information.
        ///             An application calls GetKeyState in response to a keyboard-input message. This function retrieves the state of the key when the input message was generated.
        ///             To retrieve state information for all the virtual keys, use the GetKeyboardState function.
        ///             An application can use the virtual-key code constants VK_SHIFT, VK_CONTROL, and VK_MENU as values for the nVirtKey parameter. This gives the status of the SHIFT, CTRL, or ALT keys without distinguishing between left and right. An application can also use the following virtual-key code constants as values for nVirtKey to distinguish between the left and right instances of those keys.
        ///             VK_LSHIFT
        ///             VK_RSHIFT
        ///             VK_LCONTROL
        ///             VK_RCONTROL
        ///             VK_LMENU
        ///             VK_RMENU
        /// 
        ///             These left- and right-distinguishing constants are available to an application only through the GetKeyboardState, SetKeyboardState, GetAsyncKeyState, GetKeyState, and MapVirtualKey functions.
        /// 
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        public static extern short GetKeyState(int virtualKeyCode);

        /// <summary>
        /// The GetMessageExtraInfo function retrieves the extra message information for the current thread. Extra message information is an application- or driver-defined value associated with the current thread's message queue.
        /// 
        /// </summary>
        /// 
        /// <returns/>
        /// 
        /// <remarks>
        /// To set a thread's extra message information, use the SetMessageExtraInfo function.
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern IntPtr GetMessageExtraInfo();


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct POINTAPI {
            public int x;
            public int y;
        }

        // Type definitions for Windows' basic types.
        public const int ANYSIZE_ARRAY = unchecked((int) (1));

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct RECT {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        /// <summary>
        /// Gets the text of the specified window.
        /// </summary>
        internal static string GetWindowText(IntPtr hWnd) {
            int cch;
            IntPtr lpString;
            string sResult;

            // !!!!! System.Text.Encoding
            if (IsWindow(hWnd) == false)
                return "";

            if (IsWindowUnicode(hWnd) != 0) {
                // Allocate new Unicode string
                lpString = Marshal.AllocHGlobal((cch = (GetWindowTextLengthW(hWnd) + 1))*2);

                // Get window Unicode text
                GetWindowTextW(hWnd, lpString, cch);

                // Get managed string from Unicode string
                sResult = Marshal.PtrToStringUni(lpString, cch);

                // Free allocated Unicode string
                Marshal.FreeHGlobal(lpString);
                lpString = IntPtr.Zero;

                // Return managed string
                return sResult;
            } else {
                // Allocate new ANSI string
                lpString = Marshal.AllocHGlobal((cch = (GetWindowTextLengthA(hWnd) + 1)));

                // Get window ANSI text
                GetWindowTextA(hWnd, lpString, cch);

                // Get managed string from ANSI string
                sResult = Marshal.PtrToStringAnsi(lpString, cch);

                // Free allocated ANSI string
                Marshal.FreeHGlobal(lpString);
                lpString = IntPtr.Zero;

                // Return managed string
                return sResult;
            }
        }

        /// <summary>
        /// Gets the class name of the specified window.
        /// </summary>
        internal static string GetClassName(IntPtr hWnd) {
            const int windowClassNameLength = 255;
            int cch;
            IntPtr lpString;
            string sResult;

            // !!!!! System.Text.Encoding
            if (IsWindow(hWnd) == false)
                return "";

            if (IsWindowUnicode(hWnd) != 0) {
                // Allocate new Unicode string
                lpString = Marshal.AllocHGlobal((cch = (windowClassNameLength + 1))*2);

                // Get window class Unicode text
                GetClassNameW(hWnd, lpString, cch);

                // Get managed string from Unicode string
                sResult = Marshal.PtrToStringUni(lpString, cch);

                // Free allocated Unicode string
                Marshal.FreeHGlobal(lpString);
                lpString = IntPtr.Zero;

                // Return managed string
                return sResult;
            } else {
                // Allocate new ANSI string
                lpString = Marshal.AllocHGlobal((cch = (GetWindowTextLengthA(hWnd) + 1)));

                // Get window class ANSI text
                GetClassNameA(hWnd, lpString, cch);

                // Get managed string from ANSI string
                sResult = Marshal.PtrToStringAnsi(lpString, cch);

                // Free allocated ANSI string
                Marshal.FreeHGlobal(lpString);
                lpString = IntPtr.Zero;

                // Return managed string
                return sResult;
            }
        }

        /// <summary>
        /// Retrieves the window from the client point.
        /// </summary>
        internal static IntPtr WindowFromPoint(IntPtr hClientWnd, int xPoint, int yPoint) {
            POINTAPI pt;

            pt.x = xPoint;
            pt.y = yPoint;
            ClientToScreen(hClientWnd, ref pt);

            return (IntPtr) WindowFromPoint(pt.x, pt.y);
        }

        /// <summary>
        /// Highlights the specified window.
        /// </summary>
        internal static bool HighlightWindow(IntPtr hWnd) {
            IntPtr hDC; // The DC of the window.
            RECT rt; // Rectangle area of the window.

            // Get the window DC of the window.
            if ((hDC = (IntPtr) GetWindowDC(hWnd)) == IntPtr.Zero)
                return false;

            // Get the screen coordinates of the rectangle of the window.
            GetWindowRect(hWnd, out rt);
            rt.Right -= rt.Left;
            rt.Left = 0;
            rt.Bottom -= rt.Top;
            rt.Top = 0;

            // Draw a border in the DC covering the entire window area of the window.
            IntPtr hRgn = (IntPtr) CreateRectRgnIndirect(ref rt);
            GetWindowRgn(hWnd, hRgn);
            SetROP2(hDC, R2_NOT);
            FrameRgn(hDC, hRgn, (IntPtr) GetStockObject(WHITE_BRUSH), 3, 3);
            DeleteObject(hRgn);

            // Finally release the DC.
            ReleaseDC(hWnd, hDC);

            return true;
        }

        /// <summary>
        /// Determines whether the two windows are related.
        /// </summary>
        internal static bool IsRelativeWindow(IntPtr hWnd, IntPtr hRelativeWindow, bool bProcessAncestor) {
            int dwProcess = new int(), dwProcessOwner = new int();
            int dwThread = new int(), dwThreadOwner = new int();
            ;

            // Failsafe
            if (hWnd == IntPtr.Zero)
                return false;
            if (hRelativeWindow == IntPtr.Zero)
                return false;
            if (hWnd == hRelativeWindow)
                return true;

            // Get processes and threads
            dwThread = GetWindowThreadProcessId(hWnd, ref dwProcess);
            dwThreadOwner = GetWindowThreadProcessId(hRelativeWindow, ref dwProcessOwner);

            // Get relative info
            if (bProcessAncestor)
                return (dwProcess == dwProcessOwner);
            return (dwThread == dwThreadOwner);
        }

        /// <summary>
        /// Gets the rectangle of a window using it's handle.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static Rectangle GetWindowRect(IntPtr hWnd) {
            RECT rect;
            if(GetWindowRect(hWnd, out rect) == false)
                return Rectangle.Empty;
            return new Rectangle {X = rect.Left, Y = rect.Top, Width = rect.Right - rect.Left + 1, Height = rect.Bottom - rect.Top + 1};
        }


        [DllImport("user32", EntryPoint = "IsWindowUnicode", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        internal static extern int IsWindowUnicode(IntPtr hWnd);

        [DllImport("user32", EntryPoint = "SetCapture", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        internal static extern int SetCapture(IntPtr hWnd);

        [DllImport("user32", EntryPoint = "ClientToScreen", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int ClientToScreen(IntPtr hWnd, ref POINTAPI lpPoint);

        [DllImport("user32", EntryPoint = "MapWindowPoints", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int MapWindowPoints(IntPtr hwndFrom, IntPtr hwndTo, ref RECT lprt, int cPoints);

        [DllImport("user32", EntryPoint = "MapWindowPoints", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int MapWindowPoints(IntPtr hwndFrom, IntPtr hwndTo, ref POINTAPI lppt, int cPoints);

        [DllImport("user32", EntryPoint = "ChildWindowFromPoint", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int ChildWindowFromPoint(IntPtr hWnd, int xPoint, int yPoint);

        [DllImport("user32", EntryPoint = "GetParent", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32", EntryPoint = "GetWindowTextLengthA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowTextLengthA(IntPtr hWnd);

        [DllImport("user32", EntryPoint = "GetWindowTextLengthW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowTextLengthW(IntPtr hWnd);

        [DllImport("user32", EntryPoint = "GetWindowTextA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowTextA(IntPtr hWnd, IntPtr lpString, int cch);

        [DllImport("user32", EntryPoint = "GetWindowTextW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowTextW(IntPtr hWnd, IntPtr lpString, int cch);

        [DllImport("user32", EntryPoint = "GetClassNameA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetClassNameA(IntPtr hWnd, IntPtr lpClassName, int nMaxCount);

        [DllImport("user32", EntryPoint = "GetClassNameW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetClassNameW(IntPtr hWnd, IntPtr lpClassName, int nMaxCount);

        [DllImport("user32", EntryPoint = "GetWindowDC", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowDC(IntPtr hWnd);

        [DllImport("user32", EntryPoint = "GetWindowRect", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("gdi32", EntryPoint = "CreateRectRgnIndirect", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int CreateRectRgnIndirect(ref RECT lpRect);

        [DllImport("user32", EntryPoint = "WindowFromPoint", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int WindowFromPoint(int xPoint, int yPoint);

        [DllImport("user32", EntryPoint = "GetWindowRgn", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowRgn(IntPtr hWnd, IntPtr hRgn);

        [DllImport("gdi32", EntryPoint = "SetROP2", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int SetROP2(IntPtr hdc, int nDrawMode);

        [DllImport("gdi32", EntryPoint = "FrameRgn", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int FrameRgn(IntPtr hdc, IntPtr hRgn, IntPtr hBrush, int nWidth, int nHeight);

        [DllImport("gdi32", EntryPoint = "GetStockObject", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetStockObject(int nIndex);

        [DllImport("user32", EntryPoint = "GetWindowThreadProcessId", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, ref int lpdwProcessId);

        [DllImport("gdi32", EntryPoint = "DeleteObject", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int DeleteObject(IntPtr hObject);

        [DllImport("user32", EntryPoint = "ReleaseDC", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hdc);

        // Binary raster ops
        public const int R2_NOT = unchecked((int)(6));//  Dn

        // Stock Logical Objects
        public const int WHITE_BRUSH = unchecked((int)(0));

        /// <summary>
        ///     Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null
        /// </summary>
        /// <param name="idHook">The id of the event you want to hook</param>
        /// <param name="callback">The callback.</param>
        /// <param name="hInstance">The handle you want to attach the event to, can be null</param>
        /// <param name="threadId">The thread you want to attach the event to, can be null</param>
        /// <returns>a handle to the desired hook</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, keyboardHookProc callback, IntPtr hInstance, uint threadId);

        /// <summary>
        ///     Unhooks the windows hook.
        /// </summary>
        /// <param name="hInstance">The hook handle that was returned from SetWindowsHookEx</param>
        /// <returns>True if successful, false otherwise</returns>
        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        /// <summary>
        ///     Calls the next hook.
        /// </summary>
        /// <param name="idHook">The hook id</param>
        /// <param name="nCode">The hook code</param>
        /// <param name="wParam">The wparam.</param>
        /// <param name="lParam">The lparam.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam);

        /// <summary>
        ///     Loads the library.
        /// </summary>
        /// <param name="lpFileName">Name of the library</param>
        /// <returns>A handle to the library</returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string lpFileName);

    }

    public delegate int keyboardHookProc(int code, int wParam, ref keyboardHookStruct lParam);

    [Serializable]
    public struct keyboardHookStruct {
        public int vkCode;
        public int scanCode;
        public int flags;
        public int dwExtraInfo;
        public int time;
    }


}
