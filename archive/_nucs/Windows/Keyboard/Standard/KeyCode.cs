using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using nucs.SystemCore;
using nucs.Windows;
using nucs.Windows.Keyboard;

public static class KeyCodeExtensions {
    #region Differentiating

        /// <summary>
        /// Is the given KeyCode is a modifier (regular or sided)
        /// </summary>
        public static bool IsModifier(this KeyCode kc) {
            var i = (ushort)kc;
            return (i >= 16 && i <= 18) || (i >= 160 && i <= 165);
        }

        /// <summary>
        /// Is the given KeyCode is a modifier, but contains a side indicator e.g. LControl or RShift
        /// </summary>
        public static bool IsSidedModifier(this KeyCode kc) {
            var i = (ushort)kc;
            return (i >= 160 && i <= 165);
        }

        /// <summary>
        /// Is the given KeyCode is a key
        /// </summary>
        public static bool IsKey(this KeyCode kc) {
            return !IsModifier(kc);
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Converts a <see cref="KeyCode"/> Enum item to <see cref="Keys"/> of WinForm item. incase of missfit in conversion, <see cref="Keys.None"/> is returned
        /// </summary>
        /// <param name="kc"></param>
        /// <returns></returns>
        public static Keys ToKeys(this KeyCode kc) {
            switch (kc) {
                case KeyCode.LControl:
                    return Keys.LControlKey;
                case KeyCode.RControl:
                    return Keys.RControlKey;
                case KeyCode.LMenu:
                    return Keys.LMenu;
                case KeyCode.RMenu:
                    return Keys.RMenu;
                case KeyCode.LShift:
                    return Keys.LShiftKey;
                case KeyCode.RShift:
                    return Keys.RShiftKey;
                case KeyCode.Control:
                    return Keys.Control;
                case KeyCode.Menu:
                    return Keys.Alt;
                case KeyCode.Shift:
                    return Keys.Shift;
            }
            try {
                return (Keys) kc;
            } catch (InvalidCastException) {
                return Keys.None;
            }
        }
    /// <summary>
    /// Compares between two modifiers, where sided always equal to regular.
    /// </summary>
    public static bool CompareModifiers(this Keys key, Keys to) {
        if (key == Keys.Control && (to == Keys.LControlKey || to == Keys.RControlKey)) return true;
        if (key == Keys.Alt && (to == Keys.LMenu || to == Keys.RMenu)) return true;
        if (key == Keys.Shift && (to == Keys.LShiftKey || to == Keys.RShiftKey)) return true;
        return key.Equals(to);
    }

        /// <summary>
        /// Converts a <see cref="Keys"/> Enum item to <see cref="KeyCode"/> of WinForm item. incase of missfit in conversion, <see cref="KeyCode.None"/> is returned
        /// </summary>
        /// <param name="kc"></param>
        /// <returns></returns>
        public static KeyCode ToKeyCode(this Keys kc) {
            try {
                return (KeyCode)kc;
            } catch {
                return KeyCode.None;
            }
        }

        #endregion
        
        #region Status

        /// <summary>
        /// Determines whether a key is up or down at the time the function is called by calling the GetAsyncKeyState function. (See: http://msdn.microsoft.com/en-us/library/ms646293(VS.85).aspx)
        /// 
        /// </summary>
        /// <param name="keyCode">The key code.</param>
        /// <returns>
        /// <c>true</c> if the key is down; otherwise, <c>false</c>.
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
        public static bool IsKeyDownAsync(this KeyCode keyCode) {
            return NativeWin32.GetAsyncKeyState((ushort)keyCode) < 0;
        }

        /// <summary>
        /// Determines whether the specified key is up or down by calling the GetKeyState function. (See: http://msdn.microsoft.com/en-us/library/ms646301(VS.85).aspx)
        /// 
        /// </summary>
        /// <param name="keyCode">The <see cref="T:WindowsInput.KeyCode"/> for the key.</param>
        /// <returns>
        /// <c>true</c> if the key is down; otherwise, <c>false</c>.
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
        public static bool IsKeyDown(this KeyCode keyCode) {
            return NativeWin32.GetKeyState((ushort)keyCode) < 0;
        }

        /// <summary>
        /// Determines whether the toggling key is toggled on (in-effect) or not by calling the GetKeyState function.  (See: http://msdn.microsoft.com/en-us/library/ms646301(VS.85).aspx)
        /// 
        /// </summary>
        /// <param name="keyCode">The <see cref="T:WindowsInput.KeyCode"/> for the key.</param>
        /// <returns>
        /// <c>true</c> if the toggling key is toggled on (in-effect); otherwise, <c>false</c>.
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
        public static bool IsTogglingKeyInEffect(this KeyCode keyCode) {
            return (NativeWin32.GetKeyState((ushort)keyCode) & 1) == 1;
        }

        #endregion

}


namespace nucs.Windows.Keyboard {

    /// <summary>
    /// Provides a range of tools from simulating inputs to testing if key is down.
    /// </summary>
    public static class KeyCodeUtils {


        #region Simulation
        /// <summary>
        /// Calls the Win32 SendInput method to simulate a Key DOWN.
        /// 
        /// </summary>
        /// <param name="keyCode">The KeyCode to press</param>
        public static void SimulateKeyDown(this KeyCode keyCode)
        {
            var input = new INPUT();
            input.Type = 1U;
            input.Data.Keyboard = new KEYBDINPUT();
            input.Data.Keyboard.Vk = (ushort)keyCode;
            input.Data.Keyboard.Scan = 0;
            input.Data.Keyboard.Flags = 0U;
            input.Data.Keyboard.Time = 0U;
            input.Data.Keyboard.ExtraInfo = IntPtr.Zero;
            if ((int)NativeWin32.SendInput(1U, new INPUT[1] {
                                                     input
                                                 }, Marshal.SizeOf(typeof(INPUT))) == 0)
                throw new Exception(string.Format("The key down simulation for {0} was not successful.", keyCode));
        }

        /// <summary>
        /// Calls the Win32 SendInput method to simulate a Key UP.
        /// 
        /// </summary>
        /// <param name="keyCode">The KeyCode to lift up</param>
        public static void SimulateKeyUp(this KeyCode keyCode)
        {
            var input = new INPUT();
            input.Type = 1U;
            input.Data.Keyboard = new KEYBDINPUT();
            input.Data.Keyboard.Vk = (ushort)keyCode;
            input.Data.Keyboard.Scan = 0;
            input.Data.Keyboard.Flags = 2U;
            input.Data.Keyboard.Time = 0U;
            input.Data.Keyboard.ExtraInfo = IntPtr.Zero;
            if ((int)NativeWin32.SendInput(1U, new INPUT[1] {
                                                     input
                                                 }, Marshal.SizeOf(typeof(INPUT))) == 0)
                throw new Exception(string.Format("The key up simulation for {0} was not successful.", keyCode));
        }

        /// <summary>
        /// Calls the Win32 SendInput method with a KeyDown and KeyUp message in the same input sequence in order to simulate a Key PRESS.
        /// 
        /// </summary>
        /// <param name="keyCode">The KeyCode to press</param>
        public static void SimulateKeyPress(this KeyCode keyCode)
        {
            var input1 = new INPUT();
            input1.Type = 1U;
            input1.Data.Keyboard = new KEYBDINPUT();
            input1.Data.Keyboard.Vk = (ushort)keyCode;
            input1.Data.Keyboard.Scan = 0;
            input1.Data.Keyboard.Flags = 0U;
            input1.Data.Keyboard.Time = 0U;
            input1.Data.Keyboard.ExtraInfo = IntPtr.Zero;
            var input2 = new INPUT();
            input2.Type = 1U;
            input2.Data.Keyboard = new KEYBDINPUT();
            input2.Data.Keyboard.Vk = (ushort)keyCode;
            input2.Data.Keyboard.Scan = 0;
            input2.Data.Keyboard.Flags = 2U;
            input2.Data.Keyboard.Time = 0U;
            input2.Data.Keyboard.ExtraInfo = IntPtr.Zero;
            if ((int)NativeWin32.SendInput(2U, new INPUT[2] {
                                                     input1,
                                                     input2
                                                 }, Marshal.SizeOf(typeof(INPUT))) == 0)
                throw new Exception(string.Format("The key press simulation for {0} was not successful.", keyCode));
        }

        /// <summary>
        /// Calls the Win32 SendInput method with a stream of KeyDown and KeyUp messages in order to simulate uninterrupted text entry via the keyboard.
        /// 
        /// </summary>
        /// <param name="text">The text to be simulated.</param>
        public static void SimulateTextEntry(string text)
        {
            if (text.Length > (long)int.MaxValue)
            {
                throw new ArgumentException(
                    string.Format("The text parameter is too long. It must be less than {0} characters.",
                                  (uint)int.MaxValue), "text");
            }
            byte[] bytes = Encoding.ASCII.GetBytes(text);
            int length = bytes.Length;
            var inputs = new INPUT[length * 2];
            for (int index = 0; index < length; ++index)
            {
                ushort num = bytes[index];
                var input1 = new INPUT();
                input1.Type = 1U;
                input1.Data.Keyboard = new KEYBDINPUT();
                input1.Data.Keyboard.Vk = 0;
                input1.Data.Keyboard.Scan = num;
                input1.Data.Keyboard.Flags = 4U;
                input1.Data.Keyboard.Time = 0U;
                input1.Data.Keyboard.ExtraInfo = IntPtr.Zero;
                var input2 = new INPUT();
                input2.Type = 1U;
                input2.Data.Keyboard = new KEYBDINPUT();
                input2.Data.Keyboard.Vk = 0;
                input2.Data.Keyboard.Scan = num;
                input2.Data.Keyboard.Flags = 6U;
                input2.Data.Keyboard.Time = 0U;
                input2.Data.Keyboard.ExtraInfo = IntPtr.Zero;
                if ((num & 65280) == 57344)
                {
                    input1.Data.Keyboard.Flags |= 1U;
                    input2.Data.Keyboard.Flags |= 1U;
                }
                inputs[2 * index] = input1;
                inputs[2 * index + 1] = input2;
            }
            var num1 = (int)NativeWin32.SendInput((uint)(length * 2), inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// Performs a simple modified keystroke like CTRL-C where CTRL is the modifierKey and C is the key.
        ///             The flow is Modifier KEYDOWN, Key PRESS, Modifier KEYUP.
        /// 
        /// </summary>
        /// <param name="modifierKeyCode">The modifier key</param><param name="keyCode">The key to simulate</param>
        public static void SimulateModifiedKeyStroke(KeyCode modifierKeyCode, KeyCode keyCode)
        {
            SimulateKeyDown(modifierKeyCode);
            SimulateKeyPress(keyCode);
            SimulateKeyUp(modifierKeyCode);
        }

        /// <summary>
        /// Performs a modified keystroke where there are multiple modifiers and one key like CTRL-ALT-C where CTRL and ALT are the modifierKeys and C is the key.
        ///             The flow is Modifiers KEYDOWN in order, Key PRESS, Modifiers KEYUP in reverse order.
        /// 
        /// </summary>
        /// <param name="modifierKeyCodes">The list of modifier keys</param><param name="keyCode">The key to simulate</param>
        public static void SimulateModifiedKeyStroke(IEnumerable<KeyCode> modifierKeyCodes,
                                                     KeyCode keyCode)
        {
            if (modifierKeyCodes != null)
                modifierKeyCodes.ToList().ForEach((x => SimulateKeyDown(x)));
            SimulateKeyPress(keyCode);
            if (modifierKeyCodes == null)
                return;
            modifierKeyCodes.Reverse().ToList().ForEach((x => SimulateKeyUp(x)));
        }

        /// <summary>
        /// Performs a modified keystroke where there is one modifier and multiple keys like CTRL-K-C where CTRL is the modifierKey and K and C are the keys.
        ///             The flow is Modifier KEYDOWN, Keys PRESS in order, Modifier KEYUP.
        /// 
        /// </summary>
        /// <param name="modifierKey">The modifier key</param><param name="keyCodes">The list of keys to simulate</param>
        public static void SimulateModifiedKeyStroke(KeyCode modifierKey, IEnumerable<KeyCode> keyCodes)
        {
            SimulateKeyDown(modifierKey);
            if (keyCodes != null)
                keyCodes.ToList().ForEach((x => SimulateKeyPress(x)));
            SimulateKeyUp(modifierKey);
        }

        /// <summary>
        /// Performs a modified keystroke where there are multiple modifiers and multiple keys like CTRL-ALT-K-C where CTRL and ALT are the modifierKeys and K and C are the keys.
        ///             The flow is Modifiers KEYDOWN in order, Keys PRESS in order, Modifiers KEYUP in reverse order.
        /// 
        /// </summary>
        /// <param name="modifierKeyCodes">The list of modifier keys</param><param name="keyCodes">The list of keys to simulate</param>
        public static void SimulateModifiedKeyStroke(IEnumerable<KeyCode> modifierKeyCodes,
                                                     IEnumerable<KeyCode> keyCodes)
        {
            if (modifierKeyCodes != null)
                modifierKeyCodes.ToList().ForEach((x => SimulateKeyDown(x)));
            if (keyCodes != null)
                keyCodes.ToList().ForEach((x => SimulateKeyPress(x)));
            if (modifierKeyCodes == null)
                return;
            modifierKeyCodes.Reverse().ToList().ForEach((x => SimulateKeyUp(x)));
        }
        #endregion

        

    }

    /// <summary>
    /// A list of the modifiers available in the keyboard
    /// </summary>
    [Serializable]
    public enum KeyCodeModifiers : ushort {
        Shift = (ushort)16,
        Control = (ushort)17,
        /// <summary>
        /// Equivilant to Alt
        /// </summary>
        Menu = (ushort)18,
        /// <summary>
        /// Equivilant to Menu
        /// </summary>
        Alt = (ushort)18,
        LShift = (ushort)160,
        RShift = (ushort)161,
        LControl = (ushort)162,
        RControl = (ushort)163,
        /// <summary>
        /// Equivalent to LMenu
        /// </summary>
        LAlt = (ushort)164,
        /// <summary>
        /// Equivalent to RMenu
        /// </summary>
        RAlt = (ushort)165,
        /// <summary>
        /// Equivalent to LAlt
        /// </summary>
        LMenu = (ushort)164,
        /// <summary>
        /// Equivalent to RAlt
        /// </summary>
        RMenu = (ushort)165
    }

    /// <summary>
    /// A list of only keys available on the keyboard
    /// </summary>
    [Serializable]
    public enum KeyCodeKeys : ushort {
        None = 0,
        LButton = (ushort)1,
        RButton = (ushort)2,
        Cancel = (ushort)3,
        MButton = (ushort)4,
        XButton1 = (ushort)5,
        XButton2 = (ushort)6,
        Back = (ushort)8,
        Tab = (ushort)9,
        Clear = (ushort)12,
        Return = (ushort)13,
        Pause = (ushort)19,
        Capital = (ushort)20,
        HANGEUL = (ushort)21,
        HANGUL = (ushort)21,
        KANA = (ushort)21,
        JUNJA = (ushort)23,
        Final = (ushort)24,
        HANJA = (ushort)25,
        KANJI = (ushort)25,
        Escape = (ushort)27,
        Convert = (ushort)28,
        NonConvert = (ushort)29,
        Accept = (ushort)30,
        ModeChange = (ushort)31,
        Space = (ushort)32,
        Prior = (ushort)33,
        Next = (ushort)34,
        End = (ushort)35,
        Home = (ushort)36,
        Left = (ushort)37,
        Up = (ushort)38,
        Right = (ushort)39,
        Down = (ushort)40,
        Select = (ushort)41,
        Print = (ushort)42,
        Execute = (ushort)43,
        Snapshot = (ushort)44,
        Insert = (ushort)45,
        Delete = (ushort)46,
        Help = (ushort)47,
        D0 = (ushort)48,
        D1 = (ushort)49,
        D2 = (ushort)50,
        D3 = (ushort)51,
        D4 = (ushort)52,
        D5 = (ushort)53,
        D6 = (ushort)54,
        D7 = (ushort)55,
        D8 = (ushort)56,
        D9 = (ushort)57,
        A = (ushort)65,
        B = (ushort)66,
        C = (ushort)67,
        D = (ushort)68,
        E = (ushort)69,
        F = (ushort)70,
        G = (ushort)71,
        H = (ushort)72,
        I = (ushort)73,
        J = (ushort)74,
        K = (ushort)75,
        L = (ushort)76,
        M = (ushort)77,
        N = (ushort)78,
        O = (ushort)79,
        P = (ushort)80,
        Q = (ushort)81,
        R = (ushort)82,
        S = (ushort)83,
        T = (ushort)84,
        U = (ushort)85,
        V = (ushort)86,
        W = (ushort)87,
        X = (ushort)88,
        Y = (ushort)89,
        Z = (ushort)90,
        /// <summary>
        /// Do not mistake, this button is not a modifier.
        /// </summary>
        LWIN = (ushort)91,
        /// <summary>
        /// Do not mistake, this button is not a modifier.
        /// </summary>
        RWIN = (ushort)92,
        Apps = (ushort)93,
        Sleep = (ushort)95,
        NUMPAD0 = (ushort)96,
        NUMPAD1 = (ushort)97,
        NUMPAD2 = (ushort)98,
        NUMPAD3 = (ushort)99,
        NUMPAD4 = (ushort)100,
        NUMPAD5 = (ushort)101,
        NUMPAD6 = (ushort)102,
        NUMPAD7 = (ushort)103,
        NUMPAD8 = (ushort)104,
        NUMPAD9 = (ushort)105,
        Multiply = (ushort)106,
        Add = (ushort)107,
        Seperator = (ushort)108,
        Substract = (ushort)109,
        Decimal = (ushort)110,
        Divide = (ushort)111,
        F1 = (ushort)112,
        F2 = (ushort)113,
        F3 = (ushort)114,
        F4 = (ushort)115,
        F5 = (ushort)116,
        F6 = (ushort)117,
        F7 = (ushort)118,
        F8 = (ushort)119,
        F9 = (ushort)120,
        F10 = (ushort)121,
        F11 = (ushort)122,
        F12 = (ushort)123,
        F13 = (ushort)124,
        F14 = (ushort)125,
        F15 = (ushort)126,
        F16 = (ushort)127,
        F17 = (ushort)128,
        F18 = (ushort)129,
        F19 = (ushort)130,
        F20 = (ushort)131,
        F21 = (ushort)132,
        F22 = (ushort)133,
        F23 = (ushort)134,
        F24 = (ushort)135,
        Numlock = (ushort)144,
        Scroll = (ushort)145,
        Browser_Back = (ushort)166,
        Browser_Forward = (ushort)167,
        Browser_Refresh = (ushort)168,
        Browser_Stop = (ushort)169,
        Browser_Search = (ushort)170,
        Browser_Favorites = (ushort)171,
        Browser_Home = (ushort)172,
        Volume_Mute = (ushort)173,
        Volume_Down = (ushort)174,
        Volume_Up = (ushort)175,
        Media_Next_Track = (ushort)176,
        Media_Prev_Track = (ushort)177,
        Media_Stop = (ushort)178,
        Media_Play_Pause = (ushort)179,
        Launch_Mail = (ushort)180,
        Launch_Media_Select = (ushort)181,
        Launch_App1 = (ushort)182,
        Launch_App2 = (ushort)183,
        OEM_1 = (ushort)186,
        OEM_Plus = (ushort)187,
        OEM_Comma = (ushort)188,
        OEM_Minus = (ushort)189,
        OEM_Period = (ushort)190,
        OEM_2 = (ushort)191,
        OEM_3 = (ushort)192,
        OEM_4 = (ushort)219,
        OEM_5 = (ushort)220,
        OEM_6 = (ushort)221,
        OEM_7 = (ushort)222,
        OEM_8 = (ushort)223,
        OEM_102 = (ushort)226,
        ProcessKey = (ushort)229,
        Packet = (ushort)231,
        ATTN = (ushort)246,
        CRSEL = (ushort)247,
        EXSEL = (ushort)248,
        EREOF = (ushort)249,
        Play = (ushort)250,
        Zoom = (ushort)251,
        NoName = (ushort)252,
        PA1 = (ushort)253,
        OEM_Clear = (ushort)254,
    }

    /// <summary>
    /// A full list of keys available on the keyboard.
    /// </summary>
    [Serializable]
    public enum KeyCode : ushort {
        None = 0,
        LButton = (ushort)1,
        RButton = (ushort)2,
        Cancel = (ushort)3,
        MButton = (ushort)4,
        XButton1 = (ushort)5,
        XButton2 = (ushort)6,
        Back = (ushort)8,
        Tab = (ushort)9,
        Clear = (ushort)12,
        Return = (ushort)13,
        /// <summary>
        /// Equivalent to Return
        /// </summary>
        Enter = Return,
        Shift = (ushort)16,
        Control = (ushort)17,
        /// <summary>
        /// Equivilant to Menu
        /// </summary>
        Alt = (ushort)18,
        /// <summary>
        /// Equivilant to Alt
        /// </summary>
        Menu = (ushort)18,
        Pause = (ushort)19,
        Capital = (ushort)20,
        HANGEUL = (ushort)21,
        HANGUL = (ushort)21,
        KANA = (ushort)21,
        JUNJA = (ushort)23,
        Final = (ushort)24,
        HANJA = (ushort)25,
        KANJI = (ushort)25,
        Escape = (ushort)27,
        Convert = (ushort)28,
        NonConvert= (ushort)29,
        Accept = (ushort)30,
        ModeChange = (ushort)31,
        Space = (ushort)32,
        Prior = (ushort)33,
        Next = (ushort)34,
        End = (ushort)35,
        Home = (ushort)36,
        Left = (ushort)37,
        Up = (ushort)38,
        Right = (ushort)39,
        Down = (ushort)40,
        Select = (ushort)41,
        Print = (ushort)42,
        Execute = (ushort)43,
        Snapshot = (ushort)44,
        Insert = (ushort)45,
        Delete = (ushort)46,
        Help = (ushort)47,
        D0 = (ushort)48,
        D1 = (ushort)49,
        D2 = (ushort)50,
        D3 = (ushort)51,
        D4 = (ushort)52,
        D5 = (ushort)53,
        D6 = (ushort)54,
        D7 = (ushort)55,
        D8 = (ushort)56,
        D9 = (ushort)57,
        A = (ushort)65,
        B = (ushort)66,
        C = (ushort)67,
        D = (ushort)68,
        E = (ushort)69,
        F = (ushort)70,
        G = (ushort)71,
        H = (ushort)72,
        I = (ushort)73,
        J = (ushort)74,
        K = (ushort)75,
        L = (ushort)76,
        M = (ushort)77,
        N = (ushort)78,
        O = (ushort)79,
        P = (ushort)80,
        Q = (ushort)81,
        R = (ushort)82,
        S = (ushort)83,
        T = (ushort)84,
        U = (ushort)85,
        V = (ushort)86,
        W = (ushort)87,
        X = (ushort)88,
        Y = (ushort)89,
        Z = (ushort)90,
        /// <summary>
        /// Do not mistake, this button is not a modifier.
        /// </summary>
        LWIN = (ushort)91,
        /// <summary>
        /// Do not mistake, this button is not a modifier.
        /// </summary>
        RWIN = (ushort)92,
        Apps = (ushort)93,
        Sleep = (ushort)95,
        NUMPAD0 = (ushort)96,
        NUMPAD1 = (ushort)97,
        NUMPAD2 = (ushort)98,
        NUMPAD3 = (ushort)99,
        NUMPAD4 = (ushort)100,
        NUMPAD5 = (ushort)101,
        NUMPAD6 = (ushort)102,
        NUMPAD7 = (ushort)103,
        NUMPAD8 = (ushort)104,
        NUMPAD9 = (ushort)105,
        Multiply = (ushort)106,
        Add = (ushort)107,
        Seperator = (ushort)108,
        Substract = (ushort)109,
        Decimal = (ushort)110,
        Divide = (ushort)111,
        F1 = (ushort)112,
        F2 = (ushort)113,
        F3 = (ushort)114,
        F4 = (ushort)115,
        F5 = (ushort)116,
        F6 = (ushort)117,
        F7 = (ushort)118,
        F8 = (ushort)119,
        F9 = (ushort)120,
        F10 = (ushort)121,
        F11 = (ushort)122,
        F12 = (ushort)123,
        F13 = (ushort)124,
        F14 = (ushort)125,
        F15 = (ushort)126,
        F16 = (ushort)127,
        F17 = (ushort)128,
        F18 = (ushort)129,
        F19 = (ushort)130,
        F20 = (ushort)131,
        F21 = (ushort)132,
        F22 = (ushort)133,
        F23 = (ushort)134,
        F24 = (ushort)135,
        Numlock = (ushort)144,
        Scroll = (ushort)145,
        LShift = (ushort)160,
        RShift = (ushort)161,
        LControl = (ushort)162,
        RControl = (ushort)163,
        LAlt = (ushort)164,
        RAlt = (ushort)165,
        LMenu = (ushort)164,
        RMenu = (ushort)165,
        Browser_Back = (ushort)166,
        Browser_Forward = (ushort)167,
        Browser_Refresh = (ushort)168,
        Browser_Stop = (ushort)169,
        Browser_Search = (ushort)170,
        Browser_Favorites = (ushort)171,
        Browser_Home = (ushort)172,
        Volume_Mute = (ushort)173,
        Volume_Down = (ushort)174,
        Volume_Up = (ushort)175,
        Media_Next_Track = (ushort)176,
        Media_Prev_Track = (ushort)177,
        Media_Stop = (ushort)178,
        Media_Play_Pause = (ushort)179,
        Launch_Mail = (ushort)180,
        Launch_Media_Select = (ushort)181,
        Launch_App1 = (ushort)182,
        Launch_App2 = (ushort)183,
        OEM_1 = (ushort)186,
        OEM_Plus = (ushort)187,
        OEM_Comma = (ushort)188,
        OEM_Minus = (ushort)189,
        OEM_Period = (ushort)190,
        OEM_2 = (ushort)191,
        OEM_3 = (ushort)192,
        OEM_4 = (ushort)219,
        OEM_5 = (ushort)220,
        OEM_6 = (ushort)221,
        OEM_7 = (ushort)222,
        OEM_8 = (ushort)223,
        OEM_102 = (ushort)226,
        ProcessKey = (ushort)229,
        Packet = (ushort)231,
        ATTN = (ushort)246,
        CRSEL = (ushort)247,
        EXSEL = (ushort)248,
        EREOF = (ushort)249,
        Play = (ushort)250,
        Zoom = (ushort)251,
        NoName = (ushort)252,
        PA1 = (ushort)253,
        OEM_Clear = (ushort)254
    }
    
    #region Native implementation

    public struct INPUT {
        /// <summary>
        /// The data structure that contains information about the simulated Mouse, Keyboard or Hardware event.
        /// 
        /// </summary>
        public MOUSEKEYBDHARDWAREINPUT Data;

        /// <summary>
        /// Specifies the type of the input event. This member can be one of the following values.
        ///             InputType.MOUSE - The event is a mouse event. Use the mi structure of the union.
        ///             InputType.KEYBOARD - The event is a keyboard event. Use the ki structure of the union.
        ///             InputType.HARDWARE - Windows 95/98/Me: The event is from input hardware other than a keyboard or mouse. Use the hi structure of the union.
        /// 
        /// </summary>
        public uint Type;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct MOUSEKEYBDHARDWAREINPUT {
        [FieldOffset(0)] public MOUSEINPUT Mouse;
        [FieldOffset(0)] public KEYBDINPUT Keyboard;
        [FieldOffset(0)] public HARDWAREINPUT Hardware;
    }

    /// <summary>
    /// The MOUSEINPUT structure contains information about a simulated mouse event. (see: http://msdn.microsoft.com/en-us/library/ms646273(VS.85).aspx)
    ///             Declared in Winuser.h, include Windows.h
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// If the mouse has moved, indicated by MOUSEEVENTF_MOVE, dxand dy specify information about that movement. The information is specified as absolute or relative integer values.
    ///             If MOUSEEVENTF_ABSOLUTE value is specified, dx and dy contain normalized absolute coordinates between 0 and 65,535. The event procedure maps these coordinates onto the display surface. Coordinate (0,0) maps onto the upper-left corner of the display surface; coordinate (65535,65535) maps onto the lower-right corner. In a multimonitor system, the coordinates map to the primary monitor.
    ///             Windows 2000/XP: If MOUSEEVENTF_VIRTUALDESK is specified, the coordinates map to the entire virtual desktop.
    ///             If the MOUSEEVENTF_ABSOLUTE value is not specified, dxand dy specify movement relative to the previous mouse event (the last reported position). Positive values mean the mouse moved right (or down); negative values mean the mouse moved left (or up).
    ///             Relative mouse motion is subject to the effects of the mouse speed and the two-mouse threshold values. A user sets these three values with the Pointer Speed slider of the Control Panel's Mouse Properties sheet. You can obtain and set these values using the SystemParametersInfo function.
    ///             The system applies two tests to the specified relative mouse movement. If the specified distance along either the x or y axis is greater than the first mouse threshold value, and the mouse speed is not zero, the system doubles the distance. If the specified distance along either the x or y axis is greater than the second mouse threshold value, and the mouse speed is equal to two, the system doubles the distance that resulted from applying the first threshold test. It is thus possible for the system to multiply specified relative mouse movement along the x or y axis by up to four times.
    /// 
    /// </remarks>
    public struct MOUSEINPUT {
        /// <summary>
        /// Specifies an additional value associated with the mouse event. An application calls GetMessageExtraInfo to obtain this extra information.
        /// 
        /// </summary>
        public IntPtr ExtraInfo;

        /// <summary>
        /// A set of bit flags that specify various aspects of mouse motion and button clicks. The bits in this member can be any reasonable combination of the following values.
        ///             The bit flags that specify mouse button status are set to indicate changes in status, not ongoing conditions. For example, if the left mouse button is pressed and held down, MOUSEEVENTF_LEFTDOWN is set when the left button is first pressed, but not for subsequent motions. Similarly, MOUSEEVENTF_LEFTUP is set only when the button is first released.
        ///             You cannot specify both the MOUSEEVENTF_WHEEL flag and either MOUSEEVENTF_XDOWN or MOUSEEVENTF_XUP flags simultaneously in the dwFlags parameter, because they both require use of the mouseData field.
        /// 
        /// </summary>
        public uint Flags;

        /// <summary>
        /// If dwFlags contains MOUSEEVENTF_WHEEL, then mouseData specifies the amount of wheel movement. A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel was rotated backward, toward the user. One wheel click is defined as WHEEL_DELTA, which is 120.
        ///             Windows Vista: If dwFlags contains MOUSEEVENTF_HWHEEL, then dwData specifies the amount of wheel movement. A positive value indicates that the wheel was rotated to the right; a negative value indicates that the wheel was rotated to the left. One wheel click is defined as WHEEL_DELTA, which is 120.
        ///             Windows 2000/XP: IfdwFlags does not contain MOUSEEVENTF_WHEEL, MOUSEEVENTF_XDOWN, or MOUSEEVENTF_XUP, then mouseData should be zero.
        ///             If dwFlags contains MOUSEEVENTF_XDOWN or MOUSEEVENTF_XUP, then mouseData specifies which X buttons were pressed or released. This value may be any combination of the following flags.
        /// 
        /// </summary>
        public uint MouseData;

        /// <summary>
        /// Time stamp for the event, in milliseconds. If this parameter is 0, the system will provide its own time stamp.
        /// 
        /// </summary>
        public uint Time;

        /// <summary>
        /// Specifies the absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value of the dwFlags member. Absolute data is specified as the x coordinate of the mouse; relative data is specified as the number of pixels moved.
        /// 
        /// </summary>
        public int X;

        /// <summary>
        /// Specifies the absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value of the dwFlags member. Absolute data is specified as the y coordinate of the mouse; relative data is specified as the number of pixels moved.
        /// 
        /// </summary>
        public int Y;
    }

    /// <summary>
    /// The KEYBDINPUT structure contains information about a simulated keyboard event.  (see: http://msdn.microsoft.com/en-us/library/ms646271(VS.85).aspx)
    ///             Declared in Winuser.h, include Windows.h
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// Windows 2000/XP: INPUT_KEYBOARD supports nonkeyboard-input methodssuch as handwriting recognition or voice recognitionas if it were text input by using the KEYEVENTF_UNICODE flag. If KEYEVENTF_UNICODE is specified, SendInput sends a WM_KEYDOWN or WM_KEYUP message to the foreground thread's message queue with wParam equal to VK_PACKET. Once GetMessage or PeekMessage obtains this message, passing the message to TranslateMessage posts a WM_CHAR message with the Unicode character originally specified by wScan. This Unicode character will automatically be converted to the appropriate ANSI value if it is posted to an ANSI window.
    ///             Windows 2000/XP: Set the KEYEVENTF_SCANCODE flag to define keyboard input in terms of the scan code. This is useful to simulate a physical keystroke regardless of which keyboard is currently being used. The virtual key value of a key may alter depending on the current keyboard layout or what other keys were pressed, but the scan code will always be the same.
    /// 
    /// </remarks>
    public struct KEYBDINPUT {
        /// <summary>
        /// Specifies an additional value associated with the keystroke. Use the GetMessageExtraInfo function to obtain this information.
        /// 
        /// </summary>
        public IntPtr ExtraInfo;

        /// <summary>
        /// Specifies various aspects of a keystroke. This member can be certain combinations of the following values.
        ///             KEYEVENTF_EXTENDEDKEY - If specified, the scan code was preceded by a prefix byte that has the value 0xE0 (224).
        ///             KEYEVENTF_KEYUP - If specified, the key is being released. If not specified, the key is being pressed.
        ///             KEYEVENTF_SCANCODE - If specified, wScan identifies the key and wVk is ignored.
        ///             KEYEVENTF_UNICODE - Windows 2000/XP: If specified, the system synthesizes a VK_PACKET keystroke. The wVk parameter must be zero. This flag can only be combined with the KEYEVENTF_KEYUP flag. For more information, see the Remarks section.
        /// 
        /// </summary>
        public uint Flags;

        /// <summary>
        /// Specifies a hardware scan code for the key. If dwFlags specifies KEYEVENTF_UNICODE, wScan specifies a Unicode character which is to be sent to the foreground application.
        /// 
        /// </summary>
        public ushort Scan;

        /// <summary>
        /// Time stamp for the event, in milliseconds. If this parameter is zero, the system will provide its own time stamp.
        /// 
        /// </summary>
        public uint Time;

        /// <summary>
        /// Specifies a virtual-key code. The code must be a value in the range 1 to 254. The Winuser.h header file provides macro definitions (VK_*) for each value. If the dwFlags member specifies KEYEVENTF_UNICODE, wVk must be 0.
        /// 
        /// </summary>
        public ushort Vk;
    }

    /// <summary>
    /// The HARDWAREINPUT structure contains information about a simulated message generated by an input device other than a keyboard or mouse.  (see: http://msdn.microsoft.com/en-us/library/ms646269(VS.85).aspx)
    ///             Declared in Winuser.h, include Windows.h
    /// 
    /// </summary>
    public struct HARDWAREINPUT {
        /// <summary>
        /// Value specifying the message generated by the input hardware.
        /// 
        /// </summary>
        public uint Msg;

        /// <summary>
        /// Specifies the high-order word of the lParam parameter for uMsg.
        /// 
        /// </summary>
        public ushort ParamH;

        /// <summary>
        /// Specifies the low-order word of the lParam parameter for uMsg.
        /// 
        /// </summary>
        public ushort ParamL;
    }
    #endregion


}