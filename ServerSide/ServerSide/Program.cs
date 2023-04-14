using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            // Set up the HTTP listener

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();

            Console.WriteLine("Listening for requests on http://localhost:8080/...");

            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                
                // Allow all origins to access the server CORS Policy disable
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                HttpListenerRequest request = context.Request;

                // Determine which question to respond to based on the request path
                string question = request.Url.LocalPath.ToLower(); // put the same letters
                string response = "";

                if (question == "/question1")
                {
                    response = await Question1.GetResponse();
                }
                else if (question == "/question2")
                {
                    response = Question2.GetResponse();
                }
                else if (question == "/question3")
                {
                    response = Question3.GetResponse();
                }
                else if (question == "/")
                {
                    response ="This is the home page.";
                }
                else
                {
                    // Return a 404 error for unknown paths
                    context.Response.StatusCode = 404;
                    context.Response.Close();
                    continue;
                }


                // Write the response to the output stream
                HttpListenerResponse httpResponse = context.Response;
                httpResponse.ContentType = "text/plain"; 
                httpResponse.ContentEncoding = System.Text.Encoding.UTF8;
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(response);
                httpResponse.ContentLength64 = buffer.Length;
                httpResponse.OutputStream.Write(buffer, 0, buffer.Length);
                httpResponse.OutputStream.Flush();
                httpResponse.Close();
            }
        }
    }
}

class Question1
{
    public static async Task<string> GetResponse()
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

        string output = "";
        output += "Question 1\n";

        foreach (string serverUrl in serverUrls)
        {
            try
            {
                // Make a request to the server
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverUrl);
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Make a request to the server
                HttpResponseMessage response = await httpClient.GetAsync(serverUrl);

                // Get the server type from the response
                string serverType = response.Headers.Server.ToString(); // server header

                Console.WriteLine($"{serverUrl}: {serverType}");
                output += $"{serverUrl}: {serverType}\n";

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
                output += $"Failed to get server type from {serverUrl}: {e.Message}\n";
            }
        }

        output += "\n";
        // Print the server type counts
        foreach (KeyValuePair<string, int> serverTypeCount in serverTypeCounts)
        {
            Console.WriteLine($"{serverTypeCount.Key}: {serverTypeCount.Value}");
            output += $"{serverTypeCount.Key}: {serverTypeCount.Value}\n";
        }

        return output;
    }

}


class Question2
{
    public static string GetResponse()
    {
        List<string> urls = new List<string>() {
            "https://www.w3.org/",
            "https://www.nytimes.com/",
            "https://www.wikipedia.org/",
        };

        List<double> ages = new List<double>();

        string output = "";
        output += "Question 2\n";

        foreach (string url in urls)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                // Get the Last-Modified header
                string lastModified = response.Headers["Last-Modified"];

                Console.WriteLine($"{url} age: {lastModified} seconds");

                if (DateTime.TryParse(lastModified, out DateTime lastModifiedDate))
                {
                    // Calculate the age of the resource
                    // today - last modified date = age
                    TimeSpan age = DateTime.Now - lastModifiedDate;

                    Console.WriteLine($"{url} age: {age.TotalSeconds} seconds");
                    output += $"{url} age: {age.TotalSeconds} seconds\n";
                    ages.Add(age.TotalSeconds); // retrieve the age with lastmodifier in seconds
                }
            }
        }

        if (ages.Count > 0)
        {
            double mean = ages.Average(); // average is the function for mean
            double stdev = Math.Sqrt(ages.Select(x => Math.Pow(x - mean, 2)).Average()); // formule for standard deviation

            Console.WriteLine($"Mean age: {mean} seconds\nStandard deviation: {stdev} seconds\n");
            output += $"Mean age: {mean} seconds\nStandard deviation: {stdev} seconds\n";
        }
        else
        {
            Console.WriteLine("No valid Last-Modified headers found.");
            output += "No valid Last-Modified headers found.";
        }

        return output;
    }
}

class Question3
{
   public static string GetResponse()
   {
        // Set up the test parameters
        List<string> urls = new List<string> {
            "https://example.com/",
            "https://google.com/",
            "https://github.com/"
        };

        string output = "";
        output += "Question 3\n";

        // Loop through each URL and measure the response time
        foreach (string url in urls)
        {
            // Create a stopwatch to measure the elapsed time
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Make the HTTP request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            response.Close();

            // Stop the stopwatch and display the result
            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;

            // Concatenate the URL and the response time into a single string
            Console.WriteLine($"Response time for {url}: {elapsedTime}");
            string urlResponse = $"Response time for {url}: {elapsedTime}\n";

            // Append the URL and response time to the response string
            output += urlResponse;
        }

        // Return the concatenated response string
        return output;
    }
}
