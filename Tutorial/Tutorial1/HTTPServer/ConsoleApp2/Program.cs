using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Diagnostics;

/*
 * question 3 to be submitted: Make a Dynamic Web page (Cf. Middleware and Service oriented Computing course of the S7) 
 * allowing to generate test sequences and to display the statistics of questions 1 and 2. 
 *
 *  - a dynamic web page is a web page that display different content each time it is accessed
 *
 */

/*
 * run it 
 * the on postma call an localhost 8080
 * implement the index.html + enviromental variables
 */

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // 1. Create a HttpListener instance
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();
            Console.WriteLine("Listening for incoming requests in http://localhost:8080/");

            // 2. main server loop that listens for incoming requests
            // when a client connects, it reads the incoming request and sends a response

            while (true)
            {
                HttpListenerContext context = listener.GetContext();

                // 3. Display interesting header fields in hte console
                Console.WriteLine("HTTP Method: " + context.Request.HttpMethod);
                Console.WriteLine("User Agent: " + context.Request.UserAgent);
                Console.WriteLine("Accept-Language: " + context.Request.Headers["Accept-Language"]);
                Console.WriteLine("Accept-Encoding: " + context.Request.Headers["Accept-Encoding"]);
                Console.WriteLine("Accept-Charset: " + context.Request.Headers["Accept-Charset"]);
                
                HttpListenerResponse response = context.Response;
                response.ContentType = "text/html";

              
                string filePath = context.Request.Url.LocalPath;
                Console.WriteLine("Request URL: " + filePath);

                // Check if the requested file exists
                filePath = Environment.CurrentDirectory + filePath;
                Console.WriteLine("FILEPATH: " + filePath);

                // read the index.hmtl and display it 
                if (File.Exists(filePath))
                {

                    // here we need to change to display the code 
                    byte[] buffer = File.ReadAllBytes(filePath);
                    response.ContentLength64 = buffer.Length;
                    Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();

                }
                else
                {
                    Console.WriteLine("STATUS CODE 404 ERROR NOT FOUND");
                    response.StatusCode = 404;
                    response.Close();
                }

                Console.WriteLine(" ---------------------------- ");
            }


        }
    }
}
