using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using static System.Net.WebRequestMethods;


/*
 * Question 2: Create an application that makes multiple requests to different 
 * web pages from a non-responsive web server.  
 * 
 * Caluclate the meand and standard deviation of the age of these pages
 * 
 * create a list of URLs to request
 * make a Head Request and look for the header Last-Modified
 * save tin ages with seconds
 * with all data save, calculate mean and standard deviation
 *
 */

// I tried to use the same code as in question 1, but I hav eproblem swith retrieving the last modifier headr
// httpWebRequest is older than HttpClient 


namespace Question2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // not everyone have a last-modifier heaader
            List<string> urls = new List<string>() {

                "https://www.w3.org/",
                "https://www.nytimes.com/",
                "https://www.wikipedia.org/",
            };

            List<double> ages = new List<double>();

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
                        ages.Add(age.TotalSeconds); // retrieve the age with lastmodifier in seconds
                    }
                }
            }

            if (ages.Count > 0)
            {
                double mean = ages.Average(); // average is the function for mean
                double stdev = Math.Sqrt(ages.Select(x => Math.Pow(x - mean, 2)).Average()); // formule for standard deviation

                Console.WriteLine($"Mean age: {mean} seconds");
                Console.WriteLine($"Standard deviation: {stdev} seconds");
            }
            else
            {
                Console.WriteLine("No valid Last-Modified headers found.");
            }

            Console.ReadKey();

        }


    }
}
