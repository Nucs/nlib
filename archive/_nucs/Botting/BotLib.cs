/* BOTTING LIBRARY NAMESPACES */

using nucs.Windows;
using Thread = System.Threading.Thread;

#region NAMESPACES

using System;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#endregion

//############## IMPORTANT NOTICE - There is NO error handling in this class. You will have to do that yourself ##############\\

namespace nucs.Botting {
    public static class Robot {
        #region DLL IMPORTS

        public enum MouseEventFlags : uint {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010,
            WHEEL = 0x00000800,
            XDOWN = 0x00000080,
            XUP = 0x00000100
        }

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetFrontWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out NativeWin32.RECT lpRect);

        [DllImport("User32.dll")]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        /*[StructLayout(LayoutKind.Sequential)]
        public struct RECT {
            //This will contain the location of the 4 corners of our window
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }*/

        #endregion

        #region SCREENSHOTS

        /// <summary>
        ///     Takes a screenshot of the whole screen
        /// </summary>
        /// <returns>Returns a bitmap image of the screen</returns>
        public static Bitmap getScreenshot() {
            // Create new bitmap object the size of screen
            var screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            // Creates a new graphic object
            Graphics graphic = Graphics.FromImage(screenshot);

            //Takes the screenshot
            graphic.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);

            //Dispose the graphics object
            graphic.Dispose();

            // Return the bitmap to the user
            return screenshot;
        }

        /// <summary>
        ///     Takes a bitmap screenshot of a particular window
        /// </summary>
        /// <param name="processName">The process/window's title</param>
        /// <returns>Bitmap of the window</returns>
        public static Bitmap getScreenshotOfWindow(string windowTitle) {
            //This will hold our window's information
            var WINDOW = new NativeWin32.RECT();

            //Get the window information
            IntPtr window = getWindowHandle(windowTitle);

            //Get window details
            GetWindowRect(window, out WINDOW);

            //Width and height of window
            int winWidth = WINDOW.Right - WINDOW.Left;
            int winHeight = WINDOW.Bottom - WINDOW.Top;

            //Window size
            var winSize = new Size(winWidth, winHeight);

            //Our graphics variables
            var screenshot = new Bitmap(winWidth, winHeight);
            Graphics graphic = Graphics.FromImage(screenshot);

            //Takes the screenshot, starting where the top left corner of the window is, and takes only the size of the window
            graphic.CopyFromScreen(WINDOW.Left, WINDOW.Top, 0, 0, winSize, CopyPixelOperation.SourceCopy);

            //Clean up a bit
            graphic.Dispose();

            //Return the screenshot
            return screenshot;
        }

        #endregion

        #region IMAGE RECOGNITON

        /// <summary>
        ///     Searches for a bitmap inside a larger one
        /// </summary>
        /// <param name="small">Image (Bitmap) to look for </param>
        /// <param name="large">Image (Bitmap) to look in</param>
        /// <returns>
        ///     True if image was found, and changes the Point value passed to it to the top left co-ordinates of where the
        ///     image was found
        /// </returns>
        public static bool findImage(Bitmap small, Bitmap large, out Point location) {
            //Loop through large images width
            for (int largeX = 0; largeX < large.Width; largeX++) {
                //And height
                for (int largeY = 0; largeY < large.Height; largeY++) {
                    //Loop through the small width
                    for (int smallX = 0; smallX < small.Width; smallX++) {
                        //And height
                        for (int smallY = 0; smallY < small.Height; smallY++) {
                            //Get current pixels for both image
                            Color currentSmall = small.GetPixel(smallX, smallY);
                            Color currentLarge = large.GetPixel(largeX + smallX, largeY + smallY);
                            //If they dont match (i.e. the image is not there)
                            if (!colorsMatch(currentSmall, currentLarge))
                                //Goto the next pixel in the large image
                                goto nextLoop;
                        }
                    }
                    //If all the pixels match up, then return true and change Point location to the top left co-ordinates where it was found
                    location = new Point(largeX, largeY);
                    return true;
                    //Go to next pixel on large image
                    nextLoop:
                    ;
                }
            }
            //Return false if image is not found, and set an empty point
            location = Point.Empty;
            return false;
        }

        /// <summary>
        ///     Finds ALL occurences of a smaller image inside a bigger one
        /// </summary>
        /// <param name="small">Image (24 bit Bitmap) to find </param>
        /// <param name="large">Image (Bitmap) to look in</param>
        /// <returns>A list containing Points of the top left of where the image occured</returns>
        public static List<Point> findImages(Bitmap small, Bitmap large) {
            /* !!! WARNING !!! This can be very slow but can save time in the long term. For instance, you can take a picture of the screen,
             * find and store all "items" (thing you can do stuff to) then get the person/avatar to walk round and handle each money individualy, 
             * and you just acount for the changes in position he has made. 1 big screenshot is better than 20 small ones, 
             * (creates the impression of faster run speed)
             */

            //A new list of image locations
            var imageLocations = new List<Point>();

            //Loop through large images width
            for (int largeX = 0; largeX < large.Width; largeX++) {
                //And height
                for (int largeY = 0; largeY < large.Height; largeY++) {
                    //Loop through the small width
                    for (int smallX = 0; smallX < small.Width; smallX++) {
                        //And height
                        for (int smallY = 0; smallY < small.Height; smallY++) {
                            //Get current pixels for both image
                            Color currentSmall = small.GetPixel(smallX, smallY);
                            Color currentLarge = large.GetPixel(largeX + smallX, largeY + smallY);

                            //If they dont match (i.e. the image is not there)
                            if (!colorsMatch(currentSmall, currentLarge))
                                //Goto the next pixel in the large image
                                goto nextLoop;
                        }
                    }
                    //If all the pixels match up, add point to list
                    imageLocations.Add(new Point(largeX, largeY));
                    //Go to next pixel on large image
                    nextLoop:
                    ;
                }
            }

            return imageLocations;
        }

        /// <summary>
        ///     Finds the first occurence of a color. Starts from top left corner
        /// </summary>
        /// <param name="image">Image to look in</param>
        /// <param name="color">Color to look for</param>
        /// <param name="absolute">If the colors have to be an exact match or not</param>
        /// <param name="tolerance">How close the colors have to be. Read more in the colorsSimilar() method</param>
        /// <returns>True if color found, and changes value of the Point passed to it</returns>
        public static bool findColor(Bitmap image, Color color, out Point location, bool absolute = false, int tolerance = 5) {
            //Loop through image width
            for (int i = 0; i <= image.Width; i++) {
                //And height
                for (int j = 0; j <= image.Height; j++) {
                    // If absolute match wanted and their is an absolute match between color and current pixel, return true and change Point location
                    if (absolute && colorsMatch(color, image.GetPixel(i, j))) {
                        location = new Point(i, j);
                        return true;
                    }
                        // If colors similar otherwise, change locatio and return true
                    if (colorsSimilar(color, image.GetPixel(i, j), tolerance)) {
                        location = new Point(i, j);
                        return true;
                    }
                }
            }
            //Return false if color not found
            location = Point.Empty;
            return false;
        }

        /// <summary>
        ///     Searches through an image for a particular color and adds all occurences to a Point List
        /// </summary>
        /// <param name="image">Image (Bitmap) to look in</param>
        /// <param name="color">Color to search for</param>
        /// <param name="absolute">If the colors have to be an exact match or not</param>
        /// <param name="tolerance">How close the colors have to be. Read more in the colorsSimilar() method</param>
        /// <returns>A list of point where the color has occured</returns>
        public static List<Point> findColors(Bitmap image, Color color, bool absolute = false, int tolerance = 5) {
            //Create a new list
            var colorLocations = new List<Point>();
            //Loop through image width
            for (int i = 0; i <= image.Width; i++) {
                // And height
                for (int j = 0; j <= image.Height; j++) {
                    //If absolute match wanted and their is an absolute match, add point onto lis
                    if (absolute && colorsMatch(color, image.GetPixel(i, j)))
                        colorLocations.Add(new Point(i, j));
                    else {
                        //If colors are similar otherwise, add Point onto list
                        if (colorsSimilar(color, image.GetPixel(i, j), tolerance))
                            colorLocations.Add(new Point(i, j));
                    }
                }
            }
            //Return list of color locations
            return colorLocations;
        }

        /// <summary>
        ///     Sees if 2 colors are an EXACT  match
        /// </summary>
        /// <param name="one">First color to compare</param>
        /// <param name="two">Second color to compare</param>
        /// <returns>True or false depending on if there the same</returns>
        /// pect of the color
        public static bool colorsMatch(Color one, Color two) {
            //Compares the R,G & B as
            if (one.B == two.B && one.G == two.G && one.R == two.R) return true;
            return false;
        }

        /// <summary>
        ///     Compares 2 colors and sees if they are SIMILAR
        /// </summary>
        /// <param name="one">First color to compare</param>
        /// <param name="two">Second color to compare</param>
        /// <param name="tolerance">Differance allowed. Higher equals more different colors and vice versa</param>
        /// <returns>True or false on colors being similar</returns>
        public static bool colorsSimilar(Color one, Color two, int tolerance) {
            //If any colors aspects difference if bigger than the tolerance, return false
            if (Math.Abs(one.B - two.B) >= tolerance || Math.Abs(one.G - two.G) >= tolerance || Math.Abs(one.R - two.R) >= tolerance) return false;

            return true;
        }

        #endregion

        #region WINDOW HANDLING

        /// <summary>
        ///     Returns the main window handle for the given process
        /// </summary>
        /// <param name="windowName">String with window title</param>
        /// <param name="contains">
        ///     True/False does the window title have to match exactly. False means it will check if the title
        ///     contains the string passed to it
        /// </param>
        /// <returns>A window handle</returns>
        public static IntPtr getWindowHandle(string windowTitle) {
            IntPtr hWnd;
            hWnd = (IntPtr) 0;
            Process[] processes = Process.GetProcesses();
            if (processes.Length > 0) {
                Thread.Sleep(3000);
                foreach (Process process in processes) {
                    if (process.MainWindowTitle == windowTitle) {
                        hWnd = process.MainWindowHandle;
                        break;
                    }
                }
            }
            return hWnd;
        }

        /// <summary>
        ///     Sets a particular window to the front
        /// </summary>
        /// <param name="windowHandle">The window's handle</param>
        public static Boolean setFrontWindow(IntPtr windowHandle) {
            //Get the main window handle and set it to the foreground
            return SetFrontWindow(windowHandle);
        }

        /// <summary>
        ///     Shows a window
        /// </summary>
        /// <param name="windowHandle">The window's handle</param>
        public static void showWindow(IntPtr windowHandle) { ShowWindow(windowHandle, 9); }

        #endregion

        #region ANTI-BANS

        // Note that human mouse movement is in the OS CONTROL region

        /// <summary>
        ///     Pauses the thread for a random number of milliseconds in range. Randomly delays create a human like pause,
        ///     but you can also use the fact that some method like findImage and findColor can take several seconds so that counts
        ///     as a pause
        /// </summary>
        /// <param name="lower">Minimum delay time</param>
        /// <param name="upper">Maximum delay time</param>
        public static void delay(int lower = 10, int upper = 30) { Thread.Sleep(random(lower, upper)); }

        /// <summary>
        ///     Pauses the thread for the given time
        /// </summary>
        /// <param name="time">No of milliseconds to delay for.</param>
        public static void delay(int time = 10000) { Thread.Sleep(time); }

        /// <summary>
        ///     Generates a random number  between the ranges given. Doing stuff randomly makes the program different everytime
        ///     (more human like)
        /// </summary>
        /// <param name="lower">Minimum number possible</param>
        /// <param name="upper">Maxium number possible</param>
        /// <returns>A random number</returns>
        public static int random(int lower, int upper) { return new Random().Next(lower, upper); }

        #endregion

        #region AUTOMATION

        // Allows users to esily control mouse position
        public static Point mousePosition {
            get { return Cursor.Position; }
            set { moveMouse(value); }
        }

        #region LEFT CLICK

        /// <summary>
        ///     Causes the mouse to left click at its current position
        /// </summary>
        public static void leftClick() {
            // Mouse down
            leftDown();
            //Wait
            Thread.Sleep(new Random().Next(10, 30));
            //Mouse up
            leftUp();
        }

        /// <summary>
        ///     Makes the left mouse button go down
        /// </summary>
        public static void leftDown() {
            // Mouse down
            mouse_event((int) (MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
        }

        /// <summary>
        ///     Makes the left mouse button go up
        /// </summary>
        public static void leftUp() {
            //Mouse up
            mouse_event((int) (MouseEventFlags.LEFTUP), 0, 0, 0, 0);
        }

        #endregion

        #region RIGHT CLICK

        /// <summary>
        ///     Causes the mouse to right click at its current position
        /// </summary>
        public static void rightClick() {
            //Mouse down
            rightDown();
            //Wait a bit
            Thread.Sleep(new Random().Next(10, 30));
            //Mouse up
            rightUp();
        }

        /// <summary>
        ///     Makes the right mouse button go down
        /// </summary>
        public static void rightDown() {
            //Mouse down
            mouse_event((int) (MouseEventFlags.RIGHTDOWN), 0, 0, 0, 0);
        }

        /// <summary>
        ///     Makes the right mouse button go up
        /// </summary>
        public static void rightUp() {
            //Mouse up
            mouse_event((int) (MouseEventFlags.RIGHTUP), 0, 0, 0, 0);
        }

        #endregion

        #region MOVEMENT

        /// <summary>
        ///     Moves the mouse in a linear straight line to the target position
        /// </summary>
        /// <param name="targetX">X co-ordinate of target</param>
        /// <param name="targetY">Y co-ordinate of target</param>
        /// <param name="human">Human mouse movement or just assign position</param>
        /// <param name="steps">
        ///     Number of steps to take. More means a slower but smoother move, less means a faster but more jerky
        ///     movement
        /// </param>
        public static bool moveMouse(Point target, bool human = true, int steps = 100) {
            /* If the mouse moves to slowly when traveling over buttons/boxes/dropdowns (anything to click) it can stop, 
            and will then jump to the target due to the fail safe. You can change the delay time, but the best thing to do is to try out 
            different numbers of steps and get the right balance */

            //If mouse movement is not human
            if (!human) {
                // Set mouse to target position
                Cursor.Position = target;

                return true;
            }

            // Word out how far the cursor must move for each step. It will then change by this much each step
            int xStep = (target.X - Cursor.Position.X)/steps;
            int yStep = (target.Y - Cursor.Position.Y)/steps;

            // Loop through the steps
            for (int i = 0; i <= steps; i++) {
                // Work out the new mouse postion
                int xPosition = Cursor.Position.X + xStep;
                int yPosition = Cursor.Position.Y + yStep;

                //Move the mouse to its new position
                Cursor.Position = new Point(xPosition, yPosition);

                //Delay a few milliseconds
                Thread.Sleep(5);
            }
            // As a fail safe assigns the mouse the target position 
            Cursor.Position = target;

            //If position is right
            if (Cursor.Position == target)
                //return true
                return true;
            return false;
        }

        /// <summary>
        ///     Moves the mouse relative to the window. eg if you specified for it to move  to 10,10 over the window "Notepad"
        ///     it would move 10,10 into the top left of Notepad. A bit like local scope where 0,0 is the top left corner of the
        ///     window given.
        /// </summary>
        /// <param name="windowHandle">Window handle</param>
        /// <param name="targetX">Trget X position</param>
        /// <param name="targetY">Target Y position</param>
        /// <param name="human">Human or not mouse movement</param>
        /// <param name="steps">Number of steps to take</param>
        /// <returns>True or false depending on outcome</returns>
        public static bool moveMouseRelative(IntPtr windowHandle, Point target, bool human = true, int steps = 100) {
            //Will sore window details
            var WINDOW = new NativeWin32.RECT();

            //Get window details
            if (!GetWindowRect(windowHandle, out WINDOW))
                return false;

            //Move the mouse
            if (!moveMouse(new Point(WINDOW.Left + target.X, WINDOW.Top + target.Y), human, steps))
                return false;

            return true;
        }

        /// <summary>
        ///     Drags the mouse
        /// </summary>
        /// <param name="start">Drag start position</param>
        /// <param name="end">Drag end position</param>
        /// <param name="human">Human mouse movement or not</param>
        /// <param name="steps">Number of steps to take</param>
        /// <returns>True or false on outcome</returns>
        public static bool dragMouse(Point start, Point end, bool human = true, int steps = 100) {
            //Move mouse to start point
            moveMouse(start, human, steps);
            //Left button down
            leftDown();
            //Drag mouse to end point
            moveMouse(end, human, steps);
            // Left button up
            leftUp();

            // if mouse position is right 
            if (Cursor.Position == end)
                //Return true
                return true;
            return false;
        }

        /// <summary>
        ///     Drags the mouse relative to a window
        /// </summary>
        /// <param name="windowHandle">The window handle you want to move relative to</param>
        /// <param name="start">Drag start position</param>
        /// <param name="end">Drag end position</param>
        /// <param name="human">Human mouse movement or not</param>
        /// <param name="steps">Number of steps to take</param>
        /// <returns>True or false on outcome</returns>
        public static bool dragMouseRelative(IntPtr windowHandle, Point start, Point end, bool human = true, int steps = 100) {
            //Will sore window details
            var WINDOW = new NativeWin32.RECT();

            //Get window details
            if (!GetWindowRect(windowHandle, out WINDOW))
                return false;

            //Move mouse to start point
            moveMouse(new Point(WINDOW.Left + start.X, WINDOW.Top + start.Y), human, steps);
            //Left button down
            leftDown();
            //Drag mouse to end point
            moveMouse(new Point(WINDOW.Left + end.X, WINDOW.Top + end.Y), human, steps);
            // Left button up
            leftUp();

            // if mouse position is right 
            if (Cursor.Position == end)
                //Return true
                return true;
            return false;
        }

        #endregion

        /// <summary>
        ///     New little thing that simulates a slight "jiggle" of the mouse, like a humans unsteady hands. Be aware this has had
        ///     little testing
        /// </summary>
        public static void mouseJiggle() {
            //Work out a slight change in mouse position
            int xChange = new Random().Next(-10, 10);
            int yChange = new Random().Next(-10, 10);

            //Move mouse a few pixels
            moveMouse(new Point(Cursor.Position.X + xChange, Cursor.Position.Y + yChange), true, 5);
            //Wait a bit
            Thread.Sleep(new Random().Next(20, 60));
            // Move mouse back to original point
            moveMouse(new Point(Cursor.Position.X - xChange, Cursor.Position.Y - yChange), true, 5);
        }

        /// <summary>
        ///     Simulates key preses. Read about how to use here =>
        ///     www.msdn.microsoft.com/en-us/library/system.windows.forms.sendkeys.send.aspx
        /// </summary>
        /// <param name="key">String of keys to be presses</param>
        public static void sendKeys(string keys, int delay = 300) {
            // Loop through each key in keys
            foreach (char key in keys) {
                //Send keys to the system
                SendKeys.Send(key.ToString());

                Thread.Sleep(new Random().Next(delay - 30, delay + 30));
            }
        }

        #endregion

        #region HOTKEYS

        // Store the register hot keys, and the keys currently down at this time
        public static List<Keys> hotKeys, keysDown;

        // If hotKeys monitor is running or not
        private static bool running;

        /// <summary>
        ///     Starts monitoring
        /// </summary>
        public static void startMonitoring() { running = true; }

        /// <summary>
        ///     Stops monitoring
        /// </summary>
        public static void stopMonitoring() { running = false; }

        /// <summary>
        ///     Registers a hot key
        /// </summary>
        /// <param name="key">The key to register</param>
        public static void registerHotKey(Keys key) { hotKeys.Add(key); }

        /// <summary>
        ///     Checks all registered hotkeys, and adds any that are down to the keysDown list
        /// </summary>
        public static void checkHotKeys() {
            // Clears the keyDown list of all those before
            keysDown.Clear();

            // Loop through all the hotkeys
            foreach (Keys key in hotKeys) {
                // If keys is down
                if (keyDown(key))
                    hotKeyAlert(key);
            }

            // Stop if running == false
            if (!running)
                return;

            // Wait a bit
            delay(10);

            // Start again
            checkHotKeys();
        }

        /// <summary>
        ///     Called when a Hotkey is detected down
        /// </summary>
        /// <param name="key">Hotkey that is down</param>
        public static void hotKeyAlert(Keys key) {
            keysDown.Add(key); // Add it to the key down list
            // Do somethind else...
        }

        /// <summary>
        ///     Checks if the key given is currently down
        /// </summary>
        /// <param name="key">The key to check  READ => msdn.microsoft.com/en-us/library/system.windows.forms.keys.aspx</param>
        /// <returns>Tru/False</returns>
        public static bool keyDown(Keys key) {
            //Returns if the key is down
            return GetAsyncKeyState(key) == -32767;
        }

        #endregion

        /* MAIN METHODS */
    }
}