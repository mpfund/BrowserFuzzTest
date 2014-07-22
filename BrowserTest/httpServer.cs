using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrowserTest
{
    public class SimpleHttpServer
    {

        public event Action Started;

        public static void Start(string content)
        {
            var t = new Thread(() => SimpleListenerExample(new string[] { "http://localhost:2000/" }, content));
            t.IsBackground = true;
            t.Start();

        }

        // This example requires the System and System.Net namespaces. 
        public static void SimpleListenerExample(string[] prefixes, string content)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required, 
            // for example "http://contoso.com:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            var listener = new HttpListener();
            // Add the prefixes. 
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            Console.WriteLine("Listening...");

            while (true)
            {
                // Note: The GetContext method blocks while waiting for a request. 
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                // Obtain a response object.
                HttpListenerResponse response = context.Response;
                // Construct a response. 
                string responseString = content;
                response.AddHeader("Content-Type", "text/html; charset=utf-8");
                if (request.Url.PathAndQuery.StartsWith("/test"))
                {
                    responseString = "<!DOCTYPE html><html><body><script>a = alert;var k = '" + request.RawUrl + "';</script>mm " + request.RawUrl + "</body></html>";
                }
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                // You must close the output stream.
                output.Close();
            }
            listener.Stop();
        }
    }
}
