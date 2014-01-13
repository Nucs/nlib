using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace nucs.Windows.Mouse {
    /// <summary>
    ///     Operations that simulate mouse events
    /// </summary>
    public static class MouseSimulator {
        #region Windows API Code

        private const int MOUSEEVENTF_MOVE = 0x1;
        private const int MOUSEEVENTF_LEFTDOWN = 0x2;
        private const int MOUSEEVENTF_LEFTUP = 0x4;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x8;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        private const int MOUSEEVENTF_MIDDLEUP = 0x40;
        private const int MOUSEEVENTF_WHEEL = 0x800;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets a structure that represents both X and Y mouse coordinates
        /// </summary>
        public static Point Position {
            get { return new Point(Cursor.Position.X, Cursor.Position.Y); }
            set { Cursor.Position = value; }
        }

        /// <summary>
        ///     Gets or sets only the mouse's x coordinate
        /// </summary>
        public static int X {
            get { return Cursor.Position.X; }
            set { Cursor.Position = new Point(value, Y); }
        }

        /// <summary>
        ///     Gets or sets only the mouse's y coordinate
        /// </summary>
        public static int Y {
            get { return Cursor.Position.Y; }
            set { Cursor.Position = new Point(X, value); }
        }

        #endregion

        #region Methods

        #region Global Mouse Press

/*
        /// <summary>
        ///     Press a mouse button down
        /// </summary>
        /// <param name="button"></param>
        public static void MouseDown(MouseButton button) {
            NativeWin32.mouse_event(((int) button), 0, 0, 0, 0);
        }

        /// <summary>
        ///     Press a mouse button down
        /// </summary>
        /// <param name="button"></param>
        public static void MouseDown(MouseButtons button) {
            switch (button) {
                case MouseButtons.Left:
                    MouseDown(MouseButton.Left);
                    break;
                case MouseButtons.Middle:
                    MouseDown(MouseButton.Middle);
                    break;
                case MouseButtons.Right:
                    MouseDown(MouseButton.Right);
                    break;
            }
        }

        /// <summary>
        ///     Let a mouse button up
        /// </summary>
        /// <param name="button">What kind of press to perfrom for a mouseup</param>
        public static void MouseUp(MouseButton button) {
            NativeWin32.mouse_event(((int)button) * 2, 0, 0, 0, 0);
        }

        /// <summary>
        ///     Double click a mouse button (down then up twice)
        /// </summary>
        /// <param name="button"></param>
        public static void DoubleClick(MouseButton button, int delay = 10) {
            Click(button);
            if (delay > 0)
                Task.Delay(delay);
            Click(button);
        }

        /// <summary>
        ///     Double click a mouse button (down then up twice)
        /// </summary>
        /// <param name="button"></param>
        public static void DoubleClick(MouseButtons button) {
            switch (button) {
                case MouseButtons.Left:
                    DoubleClick(MouseButton.Left);
                    break;
                case MouseButtons.Middle:
                    DoubleClick(MouseButton.Middle);
                    break;
                case MouseButtons.Right:
                    DoubleClick(MouseButton.Right);
                    break;
            }
        }

        /// <summary>
        ///     Let a mouse button up
        /// </summary>
        /// <param name="button"></param>
        public static void MouseUp(MouseButtons button) {
            switch (button) {
                case MouseButtons.Left:
                    MouseUp(MouseButton.Left);
                    break;
                case MouseButtons.Middle:
                    MouseUp(MouseButton.Middle);
                    break;
                case MouseButtons.Right:
                    MouseUp(MouseButton.Right);
                    break;
            }
        }

        /// <summary>
        ///     Click a mouse button (down then up)
        /// </summary>
        /// <param name="button">What type of button to simulate</param>
        /// <param name="delay">Put 0 or below to skip delay, uses <see cref="Task.Delay(int)"/></param>
        public static void Click(MouseButton button, int delay = 20) {
            MouseDown(button);
            if (delay > 0)
                Task.Delay(delay);
            MouseUp(button);
        }

        /// <summary>
        ///     Click a mouse button (down then up)
        /// </summary>
        /// <param name="button">What type of button to simulate</param>
        /// <param name="delay">Put 0 or below to skip delay, uses <see cref="Task.Delay(int)"/></param>
        public static void Click(MouseButtons button, int delay = 20) {
            switch (button) {
                case MouseButtons.Left:
                    Click(MouseButton.Left, delay);
                    break;
                case MouseButtons.Middle:
                    Click(MouseButton.Middle, delay);
                    break;
                case MouseButtons.Right:
                    Click(MouseButton.Right, delay);
                    break;
            }
        }

        /// <summary>
        ///     Roll the mouse wheel. Delta of 120 wheels up once normally, -120 wheels down once normally
        /// </summary>
        /// <param name="delta"></param>
        public static void MouseWheel(int delta) {
            NativeWin32.mouse_event(MOUSEEVENTF_WHEEL, 0, 0, delta, 0);
        }
*/

        #endregion

        #region DirectX Mouse Press - Handle based press

        private static readonly Random rand;

        static MouseSimulator() { rand = new Random(); }

        public static void MouseUp(MouseButton button) {
            switch (button) {
                case MouseButton.Left:
                    NativeWin32.mouse_event((int) MouseEventFlags.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                    break;
                case MouseButton.Right:
                    NativeWin32.mouse_event((int) MouseEventFlags.MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                    break;
                case MouseButton.Middle:
                    NativeWin32.mouse_event((int) MouseEventFlags.MOUSEEVENTF_MIDDLEUP, 0, 0, 0, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("button");
            }
        }

        public static void MouseDown(MouseButton button) {
            switch (button) {
                case MouseButton.Left:
                    NativeWin32.mouse_event((int) MouseEventFlags.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                    break;
                case MouseButton.Right:
                    NativeWin32.mouse_event((int) MouseEventFlags.MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                    break;
                case MouseButton.Middle:
                    NativeWin32.mouse_event((int) MouseEventFlags.MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("button");
            }
        }

        public static void Click(MouseButton button) {
            MouseDown(button);
            Thread.Sleep(rand.Next(10, 60));
            MouseUp(button);
        }

        public static void DoubleClick(MouseButton button) {
            Click(button);
            Thread.Sleep(rand.Next(10, 60));
            Click(button);
        }
        #endregion

        /// <summary>
        ///     Show a hidden current on currently application
        /// </summary>
        public static void ShowCursor() { NativeWin32.ShowCursor(true); }

        /// <summary>
        ///     Hide mouse cursor only on current application's forms
        /// </summary>
        public static void HideCursor() { NativeWin32.ShowCursor(false); }

        #endregion
    }
    #region Enums

    /// <summary>
    ///     Mouse buttons that can be pressed
    /// </summary>
    public enum MouseButton {
        Left = 0x2,
        Right = 0x8,
        Middle = 0x20,
    }

    [Flags]
    internal enum WM_MESSAGE : uint {
        /// <summary>
        ///     The WM_LBUTTONDOWN message is posted when the user presses the left mouse button
        /// </summary>
        LBUTTONDOWN = 0x201,

        /// <summary>
        ///     The RBUTTONDOWN message is posted when the user presses the right mouse button
        /// </summary>
        RBUTTONDOWN = 0x204,

        /// <summary>
        ///     The MBUTTONDOWN message is posted when the user presses the middle mouse button
        /// </summary>
        MBUTTONDOWN = 0x207,

        /// <summary>
        ///     The LBUTTONUP message is posted when the user releases the left mouse button
        /// </summary>
        LBUTTONUP = 0x202,

        /// <summary>
        ///     The RBUTTONUP message is posted when the user releases the right mouse button
        /// </summary>
        RBUTTONUP = 0x205,

        /// <summary>
        ///     The MBUTTONUP message is posted when the user releases the middle mouse button
        /// </summary>
        MBUTTONUP = 0x208,

        /// <summary>
        ///     The LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button
        /// </summary>
        LBUTTONDBLCLK = 0x203,

        /// <summary>
        ///     The RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button
        /// </summary>
        RBUTTONDBLCLK = 0x206,

        /// <summary>
        ///     The RBUTTONDOWN message is posted when the user presses the right mouse button
        /// </summary>
        MBUTTONDBLCLK = 0x209,

        /// <summary>
        ///     The MOUSEWHEEL message is posted when the user presses the mouse wheel.
        /// </summary>
        MOUSEWHEEL = 0x020A
    }

    internal enum MouseEventFlags {
        MOUSEEVENTF_MOVE = 0x0001,
        MOUSEEVENTF_LEFTDOWN = 0x0002,
        MOUSEEVENTF_LEFTUP = 0x0004,
        MOUSEEVENTF_RIGHTDOWN = 0x0008,
        MOUSEEVENTF_RIGHTUP = 0x0010,
        MOUSEEVENTF_MIDDLEDOWN = 0x0020,
        MOUSEEVENTF_MIDDLEUP = 0x0040,
        MOUSEEVENTF_XDOWN = 0x0080,
        MOUSEEVENTF_XUP = 0x0100,
        MOUSEEVENTF_WHEEL = 0x0800,
        MOUSEEVENTF_VIRTUALDESK = 0x4000,
        MOUSEEVENTF_ABSOLUTE = 0x8000
    }

    internal enum MK_MESSAGE {
        CONTROL = 0x0008,
        LBUTTON = 0x0001,
        MBUTTON = 0x0010,
        RBUTTON = 0x0002,
        SHIFT = 0x0004,
        XBUTTON1 = 0x0020,
        XBUTTON2 = 0x0040
    }
    #endregion
/*
    public class MouseSimulateForm {
        [Flags]
        public enum MouseEventFlags {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }

        

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        
        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT {
            public readonly int uMsg;
            public readonly short wParamL;
            public readonly short wParamH;
        }

        /// <summary>
        ///     The Data passed to SendInput in an array.
        /// </summary>
        /// <remarks>Contains a union field type specifies what it contains </remarks>
        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT {
            /// <summary>
            ///     The actual data type contained in the union Field
            /// </summary>
            public SendInputEventType type;

            public MouseKeybdhardwareInputUnion mkhi;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT {
            public readonly ushort wVk;
            public readonly ushort wScan;
            public readonly uint dwFlags;
            public readonly uint time;
            public readonly IntPtr dwExtraInfo;
        }

        /// <summary>
        ///     The mouse data structure
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct MouseInputData {
            /// <summary>
            ///     The x value, if ABSOLUTE is passed in the flag then this is an actual X and Y value
            ///     otherwise it is a delta from the last position
            /// </summary>
            public int dx;

            /// <summary>
            ///     The y value, if ABSOLUTE is passed in the flag then this is an actual X and Y value
            ///     otherwise it is a delta from the last position
            /// </summary>
            public int dy;

            /// <summary>
            ///     Wheel event data, X buttons
            /// </summary>
            public uint mouseData;

            /// <summary>
            ///     ORable field with the various flags about buttons and nature of event
            /// </summary>
            public MouseEventFlags dwFlags;

            /// <summary>
            ///     The timestamp for the event, if zero then the system will provide
            /// </summary>
            public readonly uint time;

            /// <summary>
            ///     Additional data obtained by calling app via GetMessageExtraInfo
            /// </summary>
            public readonly IntPtr dwExtraInfo;
        }

        /// <summary>
        ///     Captures the union of the three three structures.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        private struct MouseKeybdhardwareInputUnion {
            /// <summary>
            ///     The Mouse Input Data
            /// </summary>
            [FieldOffset(0)] public MouseInputData mi;

            /// <summary>
            ///     The Keyboard input data
            /// </summary>
            [FieldOffset(0)] public readonly KEYBDINPUT ki;

            /// <summary>
            ///     The hardware input data
            /// </summary>
            [FieldOffset(0)] public readonly HARDWAREINPUT hi;
        }

        /// <summary>
        ///     The event type contained in the union field
        /// </summary>
        private enum SendInputEventType {
            /// <summary>
            ///     Contains Mouse event data
            /// </summary>
            InputMouse,

            /// <summary>
            ///     Contains Keyboard event data
            /// </summary>
            InputKeyboard,

            /// <summary>
            ///     Contains Hardware event data
            /// </summary>
            InputHardware
        }

        public static void ClickLeftMouseButton()
        {
            /*var mouseInput = new INPUT {type = SendInputEventType.InputMouse};
            mouseInput.mkhi.mi.dx = -1;//CalculateAbsoluteCoordinateX(x);
            mouseInput.mkhi.mi.dy = -1;//CalculateAbsoluteCoordinateY(y);
            mouseInput.mkhi.mi.mouseData = 0;#1#

            var structInput = new INPUT();
            structInput.type = SendInputEventType.InputMouse;
            structInput.mkhi.mi.dx = Cursor.Position.X;
            structInput.mkhi.mi.dy = Cursor.Position.Y;

            structInput.mkhi.mi.dwFlags = MouseEventFlags.LEFTDOWN;
            SendInput(1, ref structInput, Marshal.SizeOf(new INPUT()));

            Task.Delay(100);

            structInput.mkhi.mi.dwFlags = MouseEventFlags.LEFTUP;
            SendInput(1, ref structInput, Marshal.SizeOf(new INPUT()));
        }

        public static void clicker() {
            var whandle = GetHandleByProccessName("main");
            int lparam = Cursor.Position.X & 0xFFFF | (Cursor.Position.Y & 0xFFFF) << 16;
            int wparam = 0;
            NativeWin32.PostMessage(whandle, WM_MESSAGE.LBUTTONDOWN, wparam, lparam);
            Thread.Sleep(10);
            NativeWin32.PostMessage(whandle, WM_MESSAGE.LBUTTONUP, wparam, lparam);
        }

        public static IntPtr GetHandleByProccessName(string name) {
            IntPtr hWnd = IntPtr.Zero;
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.ProcessName.Contains(name)) //MainWindowTitle MU ISRAEL //ProcessName main
                {
                    hWnd = pList.MainWindowHandle;
                }
            }
            return hWnd;
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        /*[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
#1#
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        

        

// int MouseX
// int MouseY
// public static readonly uint WM_LBUTTONUP = 0x202;
// public static readonly uint WM_LBUTTONDOWN = 0x201;

    }*/
}