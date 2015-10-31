using System;

namespace nucs.Windows.Keyboard {
    public static class KeyboardLanguage {
        /// <summary>
        /// Takes a Virtual Key from a key press event and translates it into a char from the current keyboard language.
        /// </summary>
        public static string VKCodeToUnicode(uint VKCode, IntPtr? keyboardLayout = null) {
            var sbString = new System.Text.StringBuilder();

            var bKeyState = new byte[255];
            bool bKeyStateStatus = NativeWin32.GetKeyboardState(bKeyState);
            if (!bKeyStateStatus)
                return "";
            uint lScanCode = NativeWin32.MapVirtualKey(VKCode, 0);
            var HKL = keyboardLayout ?? GetCurrentKeyboardLayout();

            NativeWin32.ToUnicodeEx(VKCode, lScanCode, bKeyState, sbString, 5, 0, HKL);
            return sbString.ToString();
        }
        
        /// <summary>
        /// Gives the keyboard current language.
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetCurrentKeyboardLayout() {
            return NativeWin32.GetKeyboardLayout(new UIntPtr(NativeWin32.GetTopMostThreadId()));
        }
    }
}
