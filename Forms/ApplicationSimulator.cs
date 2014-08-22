using System;
using System.Threading;
using System.Windows.Forms;
using ApartmentState = System.Threading.ApartmentState;
using ManualResetEventSlim = System.Threading.ManualResetEventSlim;
using Thread = System.Threading.Thread;

namespace nucs.Forms {
    /// <summary>
    /// Creates an application to run an invisible form on a background thread, 
    /// allowing use of wndProc through event <see cref="MessageArrived"/> and even use hooks for console projects.
    /// </summary>
    public class ApplicationSimulator : InvisibleForm {


        public delegate void MessageArrivalHandler(ref Message msg);

        /// <summary>
        /// Provides access to the messages that arrives through the application, unfiltered.
        /// </summary>
        public event MessageArrivalHandler MessageArrived; 

        private ApplicationSimulator() { }

        private ApplicationSimulator(Action<InvisibleForm> post_created) : base(post_created) {}


        public delegate T ActionWithReturn<out T>();

        /// <summary>
        /// Invokes an action that returns T on the app thread."/>
        /// </summary>
        /// <typeparam name="T">The return type</typeparam>
        /// <param name="act">the action that returns the type</param>
        /// <returns>value from the invoke.</returns>
        public T InvokeReturn<T>(ActionWithReturn<T> act) {
            var item = default(T);
            Invoke(new MethodInvoker(() => { item = act();}));
            return item;
        }

        /// <summary>
        /// Invokes a regular action on the app thread
        /// </summary>
        /// <param name="act">The action to invoke</param>
        public void Invoke(Action act) {
            base.Invoke(act);
        }

        protected override void WndProc(ref Message m) {
            if (MessageArrived != null)
                MessageArrived(ref m);
            base.WndProc(ref m);
        }

        /// <summary>
        /// Creates the application and starts it, returning the an instance (child of <see cref="Form"/>).
        /// </summary>
        public static ApplicationSimulator Create() {
            ApplicationSimulator instace_holder = null;

            var holder = new ManualResetEventSlim(false);
            var t = new Thread(()=> Application.Run(
                instace_holder = new ApplicationSimulator(inst => {
                                                              instace_holder = (ApplicationSimulator)inst; holder.Set();
                                                          }
                    )));

            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
            holder.Wait(); //wait for init.
            return instace_holder;
        }
    }
}