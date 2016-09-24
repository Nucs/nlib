using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nucs.Net.Discovery {
    public interface IDiscovery {
        List<ShortIP> Discover(ShortIP ip);
        void Listen();
    }
}
