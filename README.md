# Question 4

In the previous exercise (Exercise 5), I learned how to access the header fields and do HTTP response. For Question 4, I created a dynamic web page with three buttons that make a call to my server to test Questions 1, 2, and 3. It displays the response on the web page. Additionally, I have implemented Tutorial 1 and Tutorial 2 (Question 1, 2, and 3 separated) in the repository.

## What I did?

### Question 1: Popularity of different types of servers used
- Created a list of servers that I am going to call. Some servers do not have the header server, so I implemented some that work.
- Used HttpClient and HttpResponse to request and respond.
- Created a dictionary to retrieve the server type and the number of times it was used.
- Each time I encountered a repeated server header, I updated the dictionary.
- Finally, displayed the dictionary. It is slow because I have to wait for the response of each server, so please be patient.

### Question 2: Mean + Standard deviation age of pages
- Used HTTPwebrequest, which made it easier to get the headers. Although it is older than HTTP client, I could still do it with it.
- To calculate the age of the page, I used the header Last-Modified in seconds.
- When I retrieved it, the age = time now - last modified and saved in a list called ages.
- Used the formula to calculate the standard deviation and the mean.

### Question 3: Test scenarios
- The test scenario is just a simple one. I did not have time to implement a more interesting one.
- Used a stopwatch to calculate the time that a request and response take with a URL and calculate the time.
- Used HTTPweb request again.

### Question 4: Dynamic Web Pages
- I created a server with HttpListener.
- I implemented each question in a different method.
- I quit the CORS policy to work with the web page.
- you can use postman, run the code and try it:

- http://localhost:8080/question1
- http://localhost:8080/question2
- http://localhost:8080/question3 

## How to use it
- Open the server in Visual Studio Community and run it.
- Open the web page in Visual Studio and run it.
- Each time you interact with a button, you can see the response in the web page, and the server is working.
- The server is working on port 8080.
- Question 1 is slow, so please be patient.
- I did not work on the CSS of the webpage. I just worked on the functionality and tried to implement all the questions.
