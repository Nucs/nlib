using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace nucs.Html {
    public static class HttpFile {
        /// <summary>
        ///     Checks the target url mime type and if it contains 'audio' then returns true. if timedout or not audio, returns false.
        ///     timeout = -1 for no timeout
        /// </summary>
        public static bool IsSongDownloadable(string url, int timeout=5000) {
            var request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null) return false;
            if (timeout != -1)
                request.Timeout = timeout;
            HttpWebResponse response;
            try {
                response = request.GetResponse() as HttpWebResponse;
            } catch (WebException e) {
                if (e.Message.ContainsAny("could not be resolved", "timed out", "server returned an error", "Unable to connect to the remote server", "The underlying connection was closed"))
                    return false; //automatically assume its a dead link.
#if DEBUG
                throw e;
#endif
                return false;
            }

            return response != null && response.ContentType.Contains("audio");
        }
    }
}