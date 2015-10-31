using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
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

        public static string CorrectHtmlDefacts(this string txt) {
            return CorrectMap.Aggregate(txt, (current, kv) => current.Replace(kv.Value, kv.Key));
        }

        public static HtmlDocument RetriveDocument(Uri site, int timeout = 10000) {
            return RetriveDocument(site.ToString(), timeout);
        }

        public static HtmlDocument RetriveDocument(string site, int timeout = 10000, int tries=3) {
            site = site.Replace("https", "http");
            if (!site.StartsWith("http://") && !site.StartsWith("https://"))
                site = "http://" + site;
            _retry:
/*            var web = new HtmlWeb(); //.Load(site);
            web.PreRequest += request => {
                request.Timeout = timeout;
                request.CookieContainer = new CookieContainer();
                return true;
            };*/

            try {
                    WebRequest req = HttpWebRequest.Create(site);
                    req.Timeout = timeout;
                    req.Method = "GET";

                    string source;
                    using (var reader = new StreamReader(req.GetResponse().GetResponseStream()))
                        source = reader.ReadToEnd();

                    var doc = new HtmlDocument();
                    doc.LoadHtml(source);
                    return doc;

                /*          var t = Task.Run(() => {
                    WebRequest req = HttpWebRequest.Create(site);
                    req.Method = "GET";

                    string source;
                    using (var reader = new StreamReader(req.GetResponse().GetResponseStream()))
                        source = reader.ReadToEnd();
                    
                    var doc = new HtmlDocument();
                    doc.LoadHtml(source);
                    return doc;
                });
                try {
                    if (Task.WaitAll(new Task[] {t}, timeout) == false)
                        throw new TimeoutException("Timedout");
                } catch (AggregateException e) {}
                return t.Result;*/
            } catch (WebException e) {
                if (e.Message.ContainsAny("timed out", "The request was aborted",
                    "The remote name could not be resolved")) {
                    if (tries-- != 0) {
                        if (timeout > 200)
                            timeout = (int) Math.Round(timeout*0.55);
                        else
                            timeout = 200;

                        goto _retry;
                    }

                    return null;

                }

                throw e;
            } catch (TimeoutException) {
                if (tries-- != 0) {
                    if (timeout > 200)
                        timeout = (int) Math.Round(timeout*0.55);
                    else
                        timeout = 200;

                    goto _retry;
                }

                return null;
            }
        }

        public static List<HtmlNode> RetriveNodes(string site, DocumentExecuter executer) {
            if (site == null) return null;
            return (executer.Invoke(RetriveDocument(site).DocumentNode) ?? new List<HtmlNode>()).ToList();
        } 

        public static List<HtmlNode> RetriveNodes(this HtmlDocument document, DocumentExecuter executer) {
            if (document == null) return null;
            return (executer.Invoke(document.DocumentNode) ?? new List<HtmlNode>()).ToList();
        } 

        public static HtmlNode RetriveNode(string site, DocumentExecuterSingle executer) {
            if (site == null) return null;

            return executer.Invoke(RetriveDocument(site).DocumentNode);
        } 

        public static HtmlNode RetriveNode(this HtmlDocument document, DocumentExecuterSingle executer) {
            if (document == null) return null;
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
#if NET_4_5 || NET_4_51
                response.Dispose();
#endif
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
