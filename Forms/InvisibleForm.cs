using System;
using System.Windows.Forms;

namespace nucs.Forms {
    /// <summary>
    /// An invisible form, can be used to capture wndProc or just hidden form.
    /// </summary>
    public class InvisibleForm : Form {

        /// <summary>
        /// Has the form finished loading and ready to be used? (after calling post creation)
        /// </summary>
        public bool Loaded { get; private set; }

        private Action<InvisibleForm> post_creation;
        /// <summary>
        /// Create a regular invisible form
        /// </summary>
        public InvisibleForm() {
            Shown += (sender, args) => Hide();
            if (!IsHandleCreated)
                CreateHandle();
        }

        /// <summary>
        /// Creates a regular invisible form and invokes <see cref="post_created"/> after creation and hiding.
        /// </summary>
        /// <param name="post_created">Invoked after creation and hiding</param>
        public InvisibleForm(Action<InvisibleForm> post_created) : this() {
            this.post_creation = post_created;
        }

        protected override void SetVisibleCore(bool value) {
            if (!IsHandleCreated) {
                CreateHandle();
                this.WaitForHandleCreation();
            }

            // Ensures the window never becomes visible
            try {
                base.SetVisibleCore(false);
            } catch { }
            if (post_creation!=null)
                post_creation(this);
            post_creation = null;
            Loaded = true;
        }

        #region partial
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion


    }
}
