using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using System.Windows.Forms;
using Z.ExtensionMethods.Object;

#if NET_4_5
using System.Threading;
using System.Threading.Tasks;
#else
using System.Threading;
using nucs.Mono.System.Threading;
#endif

namespace nucs.Forms {
    public static class ControlExtensions {
        /// <summary>
        /// Tests if the control is set RightToLeft
        /// </summary>
        /// <param name="c">Control to be tested</param>
        public static bool IsRightToLeft(this Control c) {
            return c.RightToLeft == RightToLeft.Inherit ? (c.Parent != null && IsRightToLeft(c.Parent)) : c.RightToLeft == RightToLeft.Yes;
        }

#if NET_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Invoke(this Control c, Action act) {
            c.Invoke(new MethodInvoker(act));
        }

        public static T Invoke<T>(this Control c, Func<T> act) {
            T res = default(T);
            c.Invoke(new MethodInvoker(()=>res = act()));
            return res;
        }

#if NET_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Invoke(this Form c, Action act) {
            if (c.IsNull())
                throw new ArgumentNullException("c", "cannot invoke to a form, since it is null.");
            if (c.IsHandleCreated == false)
                Task.Run(() => { c.WaitForHandleCreation(); c.Invoke(new MethodInvoker(act)); });
            else
                c.Invoke(new MethodInvoker(act));
        }

        public static bool WaitForHandleCreation(this Control control, int timeout = -1) {
            if (control.IsHandleCreated) return true;
            var holder = new ManualResetEvent(false);
            EventHandler s = (sender, args) => ControlOnHandleCreated(holder);
            control.HandleCreated += s;
            holder.WaitOne(timeout);
            control.HandleCreated -= s;
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            return control.IsHandleCreated;
        }

        public static bool WaitForHandleCreation(this Form control, int timeout = -1) {
            if (control.IsHandleCreated) return true;
            
            var holder = new ManualResetEvent(false);
            EventHandler s = (sender, args) => ControlOnHandleCreated(holder);
            control.HandleCreated += s;
            holder.WaitOne(timeout);
            control.HandleCreated -= s;
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            return control.IsHandleCreated;
        }

        private static void ControlOnHandleCreated(ManualResetEvent holder) {
            holder.Set();
        }

        /// <summary>
        ///     Iterates all children and children-childrens to collect all controls in the form.
        /// </summary>
        /// <param name="form">The form to search</param>
        /// <returns>All the controls in the form</returns>
        public static List<Control> GetChildControls(this Form form) {
            return GetControls(null, form).ToList();
        } 
        
        private static IEnumerable<Control> GetControls(Control c = null, Form form = null) {
            if (c != null) {
                if (c.Controls != null) {
                    foreach (var cont in c.Controls.Cast<Control>()) {
                        foreach (var co in GetControls(cont))
                            yield return co;
                        yield return cont;
                    }
                    
                }
                yield break;
            }

            foreach (Control control in form.Controls)
                foreach (var cont in GetControls(control))
                    yield return cont;
                
        }
    }
}
