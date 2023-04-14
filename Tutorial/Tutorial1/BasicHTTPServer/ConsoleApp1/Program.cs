using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.NetworkInformation;
using System.Collections;

/*
    Implementation of a basic HTTP server
    Start by creating a server that takes into account a simple HTTP request of the type GET <chemin relatif d’un fichier html par rapport à la racine du site web> (example : GET /index.htmlcorresponds for the server to the sending of the file /www/pub/index.html).

    You will make sure to declare an environment variable HTTP_ROOT which allows you to specify the root directory of the tree of documents accessible from the WEB server.

    Don't forget to respect the protocol by returning a header line ( http/1.0 200 OK) in the HTTP response followed by a line break before the HTML content. 

 */

/*
 * To use it
 * - run the program
 * - go to POSTMAN or a browser and http://localhost:8080/index.html to see the html
 */

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. Set the HTTP_ROOT environment variable to the root directory of your web site 
            // @"C:\path\to\your\root\directory";
            string rootDirectory = @"C:\Users\juanl\Desktop\Web Protocols\Labs\ConsoleApp\MyWebDocs";
            Environment.SetEnvironmentVariable("HTTP_ROOT", rootDirectory);

            
            string httpRoot = Environment.GetEnvironmentVariable("HTTP_ROOT");
            if (httpRoot == null)
            {
                Console.WriteLine("HTTP_ROOT environment variable not set.");
                return;
            }

            //2. Create a TCP listener on port 8080 and start listening for incoming connections
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
            listener.Start();

            Console.WriteLine("HTTP server listening on port 8080...");

            // 3. Main server loop that listen for incoming connection from clients
            while (true) {
               
                // client
                TcpClient client = listener.AcceptTcpClient();

                //4. read the request fron the client
                NetworkStream stream = client.GetStream();
                StreamReader reader = new StreamReader(stream);
                string request = reader.ReadLine();
                Console.WriteLine("Received request: {0}", request);

                string[] parts = request.Split(' ');

                // bad response
                if (parts.Length != 3 || parts[0] != "GET")
                {
                    SendBadRequestResponse(stream);
                    client.Close();
                    continue;
                }

                string filePath = Path.Combine(httpRoot, parts[1].Substring(1));
                if (!File.Exists(filePath))
                {
                    SendNotFoundResponse(stream);
                    client.Close();
                    continue;
                }


                // 5. Send the response to the client
                byte[] content = File.ReadAllBytes(filePath);

                SendOkResponse(stream, content);

                client.Close();

                

            }

        }
        static void SendOkResponse(Stream stream, byte[] content)
        {
            string headers = "HTTP/1.0 200 OK\r\nContent-Type: text/html\r\nContent-Length: " + content.Length + "\r\n\r\n";
            byte[] headersBytes = Encoding.ASCII.GetBytes(headers);
            stream.Write(headersBytes, 0, headersBytes.Length);
            stream.Write(content, 0, content.Length);
        }

        static void SendBadRequestResponse(Stream stream)
        {
            string response = "HTTP/1.0 400 Bad Request\r\n\r\n";
            byte[] responseBytes = Encoding.ASCII.GetBytes(response);
            stream.Write(responseBytes, 0, responseBytes.Length);
        }

        static void SendNotFoundResponse(Stream stream)
        {
            string response = "HTTP/1.0 404 Not Found\r\n\r\n";
            byte[] responseBytes = Encoding.ASCII.GetBytes(response);
            stream.Write(responseBytes, 0, responseBytes.Length);
        }

    }
}
