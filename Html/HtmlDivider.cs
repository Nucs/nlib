using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using nucs.SystemCore.String;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace nucs.Html {
    public delegate IEnumerable<HtmlNode> DocumentExecuter(HtmlNode doc);
    public delegate HtmlNode DocumentExecuterSingle(HtmlNode doc);
    
    public static class HtmlDivider {
        private readonly static Dictionary<string, string> CorrectMap = new Dictionary<string, string>
            ( new Dictionary<string, string>{
                  {"!", "%21"},
                  {"$", "%24"},
                  {"&", "%26"},
                  {"'", "%27"},
                  {"(", "%28"},
                  {")", "%29"},
                  {"*", "%2A"},
                  {"+", "%2B"},
                  {",", "%2C"},
                  {"-", "%2D"},
                  {".", "%2E"},
                  {":", "%3A"},
                  {";", "%3B"},
                  {"=", "%3D"},
                  {"@", "%40"},
                  {"_", "%5F"},
                  {"~", "%7E"}
              }
            );
        
        public static HtmlDocument RetriveDocument(string site) {
            site = site.Replace("https", "http");
            if (!site.StartsWith("http://"))
                site = "http://" + site;
            return new HtmlWeb().Load(site);
        }

        public static List<HtmlNode> RetriveNodes(string site, DocumentExecuter executer) {
            return (executer.Invoke(RetriveDocument(site).DocumentNode) ?? new List<HtmlNode>()).ToList();
        } 

        public static List<HtmlNode> RetriveNodes(HtmlDocument document, DocumentExecuter executer) {
            return (executer.Invoke(document.DocumentNode) ?? new List<HtmlNode>()).ToList();
        } 

        public static HtmlNode RetriveNode(string site, DocumentExecuterSingle executer) {
            return executer.Invoke(RetriveDocument(site).DocumentNode);
        } 

        public static HtmlNode RetriveNode(HtmlDocument document, DocumentExecuterSingle executer) {
            return executer.Invoke(document.DocumentNode);
        } 

        /*public static string RetriveContent(string site)
        {
            var s = new HtmlWeb();
            
            
            int retries = 0;
            _retry: 
            try {
                using (var c = new MyWebClient()) {
                    c.Headers.Set("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:12.0) Gecko/20100101 Firefox/12.0");
                    using (var stream = c.OpenRead(new Uri(site))) {
                        if (stream == null)
                            goto _retry;
                        using (var textReader = new StreamReader(stream, Encoding.UTF8, true))
                            return textReader.ReadToEnd();
                    }
                }
            } catch (Exception e) {
                if (retries >= 3)
                    throw;
                retries++;
                goto _retry;
            }
        }*/

        public static string GetContentType(string url) {
            try {
                var request = WebRequest.Create(url);
                request.Method = "HEAD";
                WebResponse response = request.GetResponse();
                //TODO check status code
                string contentType = response.ContentType;
                response.Close();
                response.Dispose();
                return contentType;
            } catch {
                return null;
            }
        }

        class MyWebClient : WebClient {
            Uri _responseUri;

            public Uri ResponseUri {
                get { return _responseUri; }
            }

            public MyWebClient() {
                Proxy = null;
                
            }

            protected override WebRequest GetWebRequest(Uri uri){
                WebRequest w = base.GetWebRequest(uri);
                w.Timeout = 5 * 1000;
                return w;
            }

            protected override WebResponse GetWebResponse(WebRequest request) {
                WebResponse response = base.GetWebResponse(request);
                _responseUri = response.ResponseUri;
                return response;
            }
        }
    }
}
