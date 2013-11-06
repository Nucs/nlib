using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nucs.SystemCore.String;

namespace nucs.SystemCore {
    public static class UriExtensions {

        public static string CssValidHost(this Uri uri) {
            return uri.Host.Replace("www.", "").Replace('.', '-'); 
        }

    }
}
