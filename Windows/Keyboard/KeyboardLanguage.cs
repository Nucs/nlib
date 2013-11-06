using System;

namespace nucs.Windows.Keyboard {
    public static class KeyboardLanguage {
        public static string VKCodeToUnicode(uint VKCode){
            var sbString = new System.Text.StringBuilder();

            var bKeyState = new byte[255];
            bool bKeyStateStatus = NativeWin32.GetKeyboardState(bKeyState);
            if (!bKeyStateStatus)
                return "";
            uint lScanCode = NativeWin32.MapVirtualKey(VKCode, 0);
            var HKL = GetCurrentKeyboardLayout();

            NativeWin32.ToUnicodeEx(VKCode, lScanCode, bKeyState, sbString, 5, 0, HKL);
            return sbString.ToString();
        }
        
        public static IntPtr GetCurrentKeyboardLayout() {
            return NativeWin32.GetKeyboardLayout(new UIntPtr(NativeWin32.GetTopMostThreadId()));
        }
    }
}
