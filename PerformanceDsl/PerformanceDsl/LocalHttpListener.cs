using System;
using System.Configuration;
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

        /// <summary>
        ///     This method realllllly needs sorting out now.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public WebRequestInfo Read(HttpListenerRequest request)
        {
            var info = new WebRequestInfo {HttpMethod = request.HttpMethod, Url = request.Url};
            //check to see if this is a json post, which indicates a test run rather than a file upload.
            if (request.ContentType == "application/json")
            {
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
            string fileName = request.Headers["FileName"];
            if (fileName != null)
            {
                SaveFile(request.ContentEncoding, GetBoundary(request.ContentType), request.InputStream, fileName);
                info.Body = "File Uploaded";
            }
            return info;
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

        private String GetBoundary(String ctype)
        {
            return "--" + ctype.Split(';')[1].Split('=')[1];
        }

        private void SaveFile(Encoding enc, String boundary, Stream input, string fileName)
        {
            string path = ConfigurationManager.AppSettings["DllPath"];
            string pathToSave = string.Format("{0}\\{1}", path, fileName);
            Byte[] boundaryBytes = enc.GetBytes(boundary);
            Int32 boundaryLen = boundaryBytes.Length;

            using (var output = new FileStream(pathToSave, FileMode.Create, FileAccess.Write))
            {
                var buffer = new Byte[1024];
                Int32 len = input.Read(buffer, 0, 1024);
                Int32 startPos = -1;

                // Find start boundary
                while (true)
                {
                    if (len == 0)
                    {
                        throw new Exception("Start Boundaray Not Found");
                    }

                    startPos = IndexOf(buffer, len, boundaryBytes);
                    if (startPos >= 0)
                    {
                        break;
                    }
                    Array.Copy(buffer, len - boundaryLen, buffer, 0, boundaryLen);
                    len = input.Read(buffer, boundaryLen, 1024 - boundaryLen);
                }

                // Skip four lines (Boundary, Content-Disposition, Content-Type, and a blank)
                for (Int32 i = 0; i < 4; i++)
                {
                    while (true)
                    {
                        if (len == 0)
                        {
                            throw new Exception("Preamble not Found.");
                        }

                        startPos = Array.IndexOf(buffer, enc.GetBytes("\n")[0], startPos);
                        if (startPos >= 0)
                        {
                            startPos++;
                            break;
                        }
                        len = input.Read(buffer, 0, 1024);
                    }
                }

                Array.Copy(buffer, startPos, buffer, 0, len - startPos);
                len = len - startPos;

                while (true)
                {
                    Int32 endPos = IndexOf(buffer, len, boundaryBytes);
                    if (endPos >= 0)
                    {
                        if (endPos > 0) output.Write(buffer, 0, endPos);
                        break;
                    }
                    if (len <= boundaryLen)
                    {
                        throw new Exception("End Boundaray Not Found");
                    }
                    output.Write(buffer, 0, len - boundaryLen);
                    Array.Copy(buffer, len - boundaryLen, buffer, 0, boundaryLen);
                    len = input.Read(buffer, boundaryLen, 1024 - boundaryLen) + boundaryLen;
                }
            }
        }

        private static Int32 IndexOf(Byte[] buffer, Int32 len, Byte[] boundaryBytes)
        {
            for (Int32 i = 0; i <= len - boundaryBytes.Length; i++)
            {
                Boolean match = true;
                for (Int32 j = 0; j < boundaryBytes.Length && match; j++)
                {
                    match = buffer[i + j] == boundaryBytes[j];
                }

                if (match)
                {
                    return i;
                }
            }

            return -1;
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