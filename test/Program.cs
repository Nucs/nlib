using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EventHook;

using nucs.Emailing;
using nucs.Emailing.Templating;
using nucs.Toaster;

namespace test
{
    class Program
    {
        static void Main(string[] args) {
            ClipboardWatcher.OnClipboardModified += (sender, eventArgs) => Console.WriteLine(eventArgs.Data);
            ClipboardWatcher.Start();
            Console.ReadLine();



        }
    }
}
