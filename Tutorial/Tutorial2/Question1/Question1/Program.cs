using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


/*
 * Question 1: Create a list of non-sensitive web server addresses.
 * Create an application that makes a request per server, retrieves the response 
 * from each server and provides statistics on the popularity of the different types 
 * of servers used (ex. Apache, IIS, ...) 
 * 
 * - request per server
 * - retrieve types server
 * - statistics on the popularity of the different types of servers used
 * 
 */

namespace Question1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            List<string> serverUrls = new List<string>()
            {
                // error = no server header
                "https://www.mozilla.org/", // apache
                "http://www.apache.org/", // apache
                    // "https://www.bing.com/", IIS, error
                "https://www.hp.com/", // IIS
                "https://www.nginx.com/", // nginx
                "https://www.dropbox.com/", // nginx
                    //  "https://www.twitch.tv/", // nginx , error
                    // "https://www.apt-get.eu/", // lighttpd error
                "https://www.google.com/", // gws
                "https://www.youtube.com/", // gws 
                "https://www.springframework.org/",// cloudfare
                    // "https://www.cisco.com/", // Zeus error
                "https://www.amazon.com/",
                "https://www.twitter.com/",
                "https://www.github.com/",
                "https://www.stackoverflow.com/",
                "https://www.reddit.com/",
              "https://www.apachehaus.com/",
              "https://www.digitalocean.com/",
              "https://www.openoffice.org/",
            };

            

            // Dictionary to store server type counts
            Dictionary<string, int> serverTypeCounts = new Dictionary<string, int>();
            HttpClient httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(5); // be faster

            foreach (string serverUrl in serverUrls)
            {
                try
                {
                    // Make a request to the server
                    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverUrl);
                    //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    HttpResponseMessage response = await httpClient.GetAsync(serverUrl);

                    // Get the server type from the response
                    // Apache, IIS,NGINX, lighttpd, gws, etc.
                    //string serverType = response.Server;
                    string serverType = response.Headers.Server.ToString(); // server header


                    Console.WriteLine($"{serverUrl}: {serverType}");
                    // If the server type is already in the dictionary, increment the count

                
                    if (serverTypeCounts.ContainsKey(serverType))
                    {
                        serverTypeCounts[serverType]++;
                    }
                    // Otherwise, add the server type to the dictionary with a count of 1
                    else
                    {
                        serverTypeCounts.Add(serverType, 1);
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Failed to get server type from {serverUrl}: {e.Message}");
                }
            }


            Console.WriteLine("");
            // Print the server type counts
            foreach (KeyValuePair<string, int> serverTypeCount in serverTypeCounts)
            {
                Console.WriteLine($"{serverTypeCount.Key}: {serverTypeCount.Value}");
            }

            Console.ReadLine();


        }
    }
}
