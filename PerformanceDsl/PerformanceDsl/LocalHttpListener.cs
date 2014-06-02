using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PerformanceDsl.Logging;

namespace PerformanceDsl
{
    public class LocalHttpListener
    {
        public const string UriAddress = "http://*:9999/";
        private readonly HttpListener _httpListener;

        public LocalHttpListener()
        {
            Log4NetLogger.LogEntry(typeof (LocalHttpListener), "LocalHttpListener_Constructor",
                string.Format("Uri is {0}", UriAddress), LoggerLevel.Info);
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(UriAddress);
        }

        public void Start()
        {
            _httpListener.Start();
            Log4NetLogger.LogEntry(typeof (LocalHttpListener), "Start", "started server", LoggerLevel.Info);

            while (_httpListener.IsListening)
                ProcessRequest();
        }

        public void Stop()
        {
            _httpListener.Stop();
        }

        private void ProcessRequest()
        {
            Log4NetLogger.LogEntry(typeof (LocalHttpListener), "ProcessRequest", "processing request", LoggerLevel.Info);
            IAsyncResult result = _httpListener.BeginGetContext(ListenerCallback, _httpListener);
            result.AsyncWaitHandle.WaitOne();
        }

        private void ListenerCallback(IAsyncResult result)
        {
            HttpListenerContext context = _httpListener.EndGetContext(result);

            try
            {
                WebRequestInfo info = Read(context.Request);
                Log4NetLogger.LogEntry(typeof (LocalHttpListener), "ProcessRequest",
                    string.Format("Server received: {0}{1}",
                        Environment.NewLine,
                        info), LoggerLevel.Info);

                CreateResponse(context.Response, info.ToString());
            }
            catch (Exception exception)
            {
                CreateResponse(context.Response, exception.ToString());
            }
        }

        public static WebRequestInfo Read(HttpListenerRequest request)
        {
            var info = new WebRequestInfo {HttpMethod = request.HttpMethod, Url = request.Url};

            if (request.HasEntityBody)
            {
                Encoding encoding = request.ContentEncoding;
                using (Stream bodyStream = request.InputStream)
                using (var streamReader = new StreamReader(bodyStream, encoding))
                {
                    if (request.ContentType != null)
                        info.ContentType = request.ContentType;

                    info.ContentLength = request.ContentLength64;
                    info.Body = streamReader.ReadToEnd();
                }
            }
            Log4NetLogger.LogEntry(typeof (LocalHttpListener), "Read", info.Body, LoggerLevel.Info);
            var testRunner = new TestRunner();
            try
            {
                var testRun = JsonConvert.DeserializeObject<TestRun>(info.Body);
                Task task = testRunner.Begin(testRun);
                task.ContinueWith(x =>
                {
                    Console.WriteLine(x.Status.ToString());
                    Console.WriteLine("end");
                });
                task.Wait();
                return info;
            }
            catch (Exception exception)
            {
                Log4NetLogger.LogEntry(typeof (LocalHttpListener), "Read", info.Body, LoggerLevel.Error, exception);
                return null;
            }
        }

        public static WebResponseInfo Read(HttpWebResponse response)
        {
            var info = new WebResponseInfo
            {
                StatusCode = response.StatusCode,
                StatusDescription = response.StatusDescription,
                ContentEncoding = response.ContentEncoding,
                ContentLength = response.ContentLength,
                ContentType = response.ContentType
            };

            using (Stream bodyStream = response.GetResponseStream())
                if (bodyStream != null)
                {
                    using (var streamReader = new StreamReader(bodyStream, Encoding.UTF8))
                    {
                        info.Body = streamReader.ReadToEnd();
                    }
                }
            return info;
        }

        private static void CreateResponse(HttpListenerResponse response, string body)
        {
            response.StatusCode = (int) HttpStatusCode.OK;
            response.StatusDescription = HttpStatusCode.OK.ToString();
            byte[] buffer = Encoding.UTF8.GetBytes(body);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
    }

    public class WebRequestInfo
    {
        public string Body { get; set; }
        public long ContentLength { get; set; }
        public string ContentType { get; set; }
        public string HttpMethod { get; set; }
        public Uri Url { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("HttpMethod {0}", HttpMethod));
            sb.AppendLine(string.Format("Url {0}", Url));
            sb.AppendLine(string.Format("ContentType {0}", ContentType));
            sb.AppendLine(string.Format("ContentLength {0}", ContentLength));
            sb.AppendLine(string.Format("Body {0}", Body));
            return sb.ToString();
        }
    }

    public class WebResponseInfo
    {
        public string Body { get; set; }
        public string ContentEncoding { get; set; }
        public long ContentLength { get; set; }
        public string ContentType { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("StatusCode {0} StatusDescripton {1}", StatusCode, StatusDescription));
            sb.AppendLine(string.Format("ContentType {0} ContentEncoding {1} ContentLength {2}", ContentType,
                ContentEncoding, ContentLength));
            sb.AppendLine(string.Format("Body {0}", Body));
            return sb.ToString();
        }
    }
}