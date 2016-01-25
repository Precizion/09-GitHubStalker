using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;


namespace GitHubStalker
{
    class Program
    {
        static void Main(string[] args)
        {
            // getting input
            Console.WriteLine("Enter a username");

            // downloading content forom github
            string username = Console.ReadLine();
            //string jsonRepo;

            // tried to read JSON Repo Array
            /*if(File.Exists("githubrepotest.json"))
            {
                jsonRepo = File.ReadAllText("githubrepotest.json");
                jsonRepo = JsonConvert.DeserializeObject<
            }*/

            //webclient = a class that simulates a browser
            WebClient wc = new WebClient();
            WebClient otherwc = new WebClient();
            WebClient commitWC = new WebClient();
            WebClient issuesWC = new WebClient();

            wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            otherwc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            commitWC.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            issuesWC.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            //create a json string
            string json = wc.DownloadString("https://api.github.com/users/" + username);
            string jsonRepo = otherwc.DownloadString("https://api.github.com/users/" + username + "/repos");


            //parse json into something I can display
            var gitHubObject = JObject.Parse(json);
            var gitHubRepo = JArray.Parse(jsonRepo);

            Console.WriteLine("Name: " + gitHubObject["name"].ToString());

            Console.WriteLine("URL: " + gitHubObject["url"].ToString());

            Console.WriteLine("Followers: " + gitHubObject["followers"].ToString());

            Console.WriteLine("Repositories: " + gitHubObject["public_repos"].ToString());

            Console.WriteLine("------------");

            for ( var i = 0; i < gitHubRepo.Count; i ++)
            {
                Console.WriteLine(gitHubRepo[i]["name"].ToString() + ", " + gitHubRepo[i]["stargazers_count"] + " stars, " + gitHubRepo[i]["watchers_count"] + " watchers");

                var repo = gitHubRepo[i]["name"];
                string jsonRepoCommit = commitWC.DownloadString("https://api.github.com/repos/" + username + "/" + repo + "/commits");
                var gitHubRepoCommit = JArray.Parse(jsonRepoCommit);

                for (var j = 0; j < gitHubRepoCommit.Count; j++)
                {
                    Console.WriteLine("------------");
                    Console.WriteLine("------------");
                    Console.WriteLine("Commit #" + (j+1) + ":" + gitHubRepoCommit[j]["commit"].ToString());
                    
                    // Attempt at getting more specific information from commits
                    //" Date: " + gitHubRepoCommit[j]["commit.committer.date"].ToString() + " Commit Message: " + gitHubRepoCommit[j]["commit.message"].ToString());
                    Console.WriteLine("------------");
                    Console.WriteLine("------------");
                }


                string jsonRepoIssue = issuesWC.DownloadString("https://api.github.com/repos/" + username + "/" + repo + "/issues");
                var gitHubRepoIssues = JArray.Parse(jsonRepoIssue);

                for (var k = 0; k < gitHubRepoIssues.Count; k++)
                {
                    Console.WriteLine("------------");
                    Console.WriteLine("Issue #" + (k+1));
                    Console.WriteLine("Title: " + gitHubRepoIssues[k]["title"].ToString());
                    Console.WriteLine("Body: " + gitHubRepoIssues[k]["body"].ToString());
                    Console.WriteLine("------------");

                    //" Date: " + gitHubRepoCommit[j]["commit.committer.date"].ToString() + " Commit Message: " + gitHubRepoCommit[j]["commit.message"].ToString());
                }
            }

            Console.ReadLine();

        }
    }
}
