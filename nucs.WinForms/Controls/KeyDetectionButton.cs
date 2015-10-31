#if !AV_SAFE

using System;
using System.Windows.Forms;
using nucs.SystemCore.Boolean;
using nucs.Windows.Keyboard;

namespace nucs.WinForms.Controls {

    public delegate void KeyDetectedHadler(KeyDetectionButton sender, KeyEventArgs args, AKey key);

    /// <summary>
    /// Used to detect a what key is being pressed
    /// </summary>
    public class KeyDetectionButton : Button {
        /// <summary>
        /// Invoked when the button detects a key press
        /// </summary>
        public event KeyDetectedHadler KeyDetected;

        public KeyDetectionButton() {
            IsDetecting = false;
            detectKeyFunc = (o, args) => btnDetect_PreviewKeyDown(o as Button, args);
            Click += OnClick;
            Disposed += (sender, args) => onDisposing();
        }

        /// <summary>
        /// Is the button detecting keypress? true after being clicked.
        /// </summary>
        public bool IsDetecting { get; private set; }

        private readonly PreviewKeyDownEventHandler detectKeyFunc;

        private void OnClick(object sender, EventArgs eventArgs) {
            if (IsDetecting)
                return;
            IsDetecting = true;
            Text = "Press Any Key";
            PreviewKeyDown += detectKeyFunc;
        }

        /// <summary>
        /// Once a key detected, The key and it's details can be obtained here through <see cref="AKey"/> instance.
        /// </summary>
        public AKey AKey { get; private set; }


        private void btnDetect_PreviewKeyDown(Button sender, PreviewKeyDownEventArgs e) {
            //Break logic
            if (IsDetecting == false) { //not suppose to happen. but you know..
                PreviewKeyDown -= detectKeyFunc;
                return;
            }

            if (Bool.EqualsAny((KeyCode)e.KeyValue, KeyCode.Shift, KeyCode.Alt, KeyCode.Control))
                return;

            if (KeyDetected == null) {
                Text = "Detect";
                PreviewKeyDown -= detectKeyFunc;
                IsDetecting = false;
                return;
            }
            //Event preperation
            var args = new KeyEventArgs(e.KeyData) { Handled = true };
            AKey = AKey.Produce(false);
            
            KeyDetected(this, args, AKey);
            if (args.Handled) {
                Text = "Detect";
                PreviewKeyDown -= detectKeyFunc;
                IsDetecting = false;
            }
        }

        private void onDisposing() {
            
        }
        /* // difference in the event args.
         * Yes the various ways have minor subtle differences as far as determining when to react to a keystroke
         * but ultimately it probably doesn't matter a whole lot.  Now the differences.
         * KeyCode is an enumeration that represents all the possible keys on the keyboard
         * and is the best way to handle specific keys.
         * KeyData is the KeyCode combined with the modifiers (Ctrl, Alt and/or Shift).
         * Finally KeyValue is the raw numeric value.
         * Normally you use KeyCode to determine what key was pressed when you don't care about 
         * the state of the modifiers. Use KeyData when you do.
         * Note that normally you are reacting to a key press so you get the KeyEventArgs
         * argument that actually contains booleans for the modifiers so you can use KeyCode
         * in combination with the other properties to get the same information. 
         * Honestly this probably more readable anyway.  Thus in general you will probably look at KeyCode.
         * BTW please post WinForms related questions in the WinForms forum. 
         * Please reserve the C# forum for questions specifically about C#. 
         * I'm moving your posting to the WinForms forum.  Thanks.
         */

    }
}
#endif