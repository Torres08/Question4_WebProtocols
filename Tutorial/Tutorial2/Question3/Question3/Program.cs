using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

/*
 * Question 3: Add programs that implement test scenarios and display statistical results that seem relevant to you (at least 3 that will be reused in the next question). 
 * Your originality and creativity is an important factor in this matter. You must then write in the README that will accompany question 4 to be submitted, the technical 
 * use cases that motivate these test scenarios. 
 * 
 */


/*
 * test scenarios, ideas:
 * 
 * - response the measure time of  HTTP request of a URL
 * - quickly check the response time of a web page
 * - use a stopwatch to calculate the time it takes to load a web page
 * 
 */


namespace Question3
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // Set up the test parameters
            List<string> urls = new List<string> { 
                "https://example.com/", 
                "https://google.com/", 
                "https://github.com/"
            };


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
                Console.WriteLine($"Response time for {url}: {elapsedTime}");

               
            }
            Console.ReadKey();
        }
    }
}
