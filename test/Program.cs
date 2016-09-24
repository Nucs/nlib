using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nucs.Startup;

namespace test
{
    class Program
    {
        static void Main(string[] args) {
            var o = CurrentAppStartup.IsAttached;
            Console.ReadLine();
        }
    }
}
