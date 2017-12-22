using HtmlAgilityPack;
using Immanuel.Yt.Cmments.Feedback.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Test.Scrape
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://www.youtube.com/watch?v=rWSSSzM7Nbg";
            //url = "https://www.youtube.com/embed/BmOpD46eZoA?start=36&end=65";
            url = "https://www.youtube.com/watch?v=2Vv-BfVoq4g&list=PLx0sYbCqOb8TBPRdmBHs5Iftvv9TPboYG";
            //url = "http://immanuel.co/";
            //url = "";
            url = "rWSSSzM7Nbg";
            //var uid = Regex.Match(url, @"(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?)\/|.*[?&amp;]v=)|youtu\.be\/)([^""&amp;?\/ ]{11})").Groups[1].Value;

            //var uid1 = Regex.Match(url, @"(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?)\/|.*[?&amp;]v=)|youtu\.be\/)([^""&amp;?\/ ]{11})").Groups[1].Value;

            //Works fine
            var regexString = @"^.*(youtu\.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=)([^#\&\?]*).*";
            var vvvl = Regex.Match(url, regexString);
            var uid3 = vvvl.Groups[2].Value;
            uid3 = string.IsNullOrEmpty(uid3) ? url : uid3;

            WebClient client = new WebClient();
            string json = client.DownloadString("https://www.googleapis.com/youtube/v3/commentThreads?key=AIzaSyBRhULlCbQcdBk_HLFrpP-UU6VlyXInAPQ&textFormat=plainText&part=snippet&maxResults=100&videoId=" + uid3);
            YtComment jo = JsonConvert.DeserializeObject<YtComment>(json);
            json = client.DownloadString("https://www.googleapis.com/youtube/v3/videos?part=statistics&key=AIzaSyBRhULlCbQcdBk_HLFrpP-UU6VlyXInAPQ&id=" + uid3);
            YtVideo jo1 = JsonConvert.DeserializeObject<YtVideo>(json);
            Console.WriteLine(uid3);


            //HtmlDocument doc = new HtmlDocument();
            //WebClient client = new WebClient();
            //string html = client.DownloadString("https://www.googleapis.com/youtube/v3/commentThreads?key=AIzaSyBRhULlCbQcdBk_HLFrpP-UU6VlyXInAPQ&textFormat=plainText&part=snippet&videoId=9EVeC7jmdtc");
            //doc.LoadHtml(html);

            //HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div");

            //YtComment jo = JsonConvert.DeserializeObject<YtComment>(File.ReadAllText(@"C:\Immi\Personal\MyProjects\you-dl\Immanuel.Ydl\Immanuel.Yt.Cmments.Feedback\Test.Scrape\YT_Comment_1.json"));
            //YtVideo jo1 = JsonConvert.DeserializeObject<YtVideo>(File.ReadAllText(@"C:\Immi\Personal\MyProjects\you-dl\Immanuel.Ydl\Immanuel.Yt.Cmments.Feedback\Test.Scrape\YT_Vid.json"));

            //JObject dd = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(@"C:\Immi\Personal\MyProjects\you-dl\Immanuel.Ydl\Immanuel.Yt.Cmments.Feedback\Immanuel.Yt.Cmments.Feedback\App_Data\en-EN\Vulgar.json"));
            //List<string> vulgar = JsonConvert.DeserializeObject<List<string>>(dd.GetValue("vulgar").ToString());

            //dd = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(@"C:\Immi\Personal\MyProjects\you-dl\Immanuel.Ydl\Immanuel.Yt.Cmments.Feedback\Immanuel.Yt.Cmments.Feedback\App_Data\en-EN\Ugly.json"));
            //List<string> ugly = JsonConvert.DeserializeObject<List<string>>(dd.GetValue("ugly").ToString());

            //dd = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(@"C:\Immi\Personal\MyProjects\you-dl\Immanuel.Ydl\Immanuel.Yt.Cmments.Feedback\Immanuel.Yt.Cmments.Feedback\App_Data\en-EN\Positive.json"));
            //List<string> positive = JsonConvert.DeserializeObject<List<string>>(dd.GetValue("positive").ToString());

            //dd = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(@"C:\Immi\Personal\MyProjects\you-dl\Immanuel.Ydl\Immanuel.Yt.Cmments.Feedback\Immanuel.Yt.Cmments.Feedback\App_Data\en-EN\Offensive.json"));
            //List<string> offensive = JsonConvert.DeserializeObject<List<string>>(dd.GetValue("offensive").ToString());

            //dd = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(@"C:\Immi\Personal\MyProjects\you-dl\Immanuel.Ydl\Immanuel.Yt.Cmments.Feedback\Immanuel.Yt.Cmments.Feedback\App_Data\en-EN\Negative.json"));
            //List<string> negative = JsonConvert.DeserializeObject<List<string>>(dd.GetValue("negative").ToString());

            //dd = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(@"C:\Immi\Personal\MyProjects\you-dl\Immanuel.Ydl\Immanuel.Yt.Cmments.Feedback\Immanuel.Yt.Cmments.Feedback\App_Data\en-EN\Insult.json"));
            //List<string> insult = JsonConvert.DeserializeObject<List<string>>(dd.GetValue("insult").ToString());

            //dd = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(@"C:\Immi\Personal\MyProjects\you-dl\Immanuel.Ydl\Immanuel.Yt.Cmments.Feedback\Immanuel.Yt.Cmments.Feedback\App_Data\en-EN\Harsh.json"));
            //List<string> harsh = JsonConvert.DeserializeObject<List<string>>(dd.GetValue("harsh").ToString());

            ////dd = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(@"C:\Immi\Personal\MyProjects\you-dl\Immanuel.Ydl\Immanuel.Yt.Cmments.Feedback\Immanuel.Yt.Cmments.Feedback\App_Data\en-EN\Complaint.json"));
            ////List<string> complaint = JsonConvert.DeserializeObject<List<string>>(dd.GetValue("complaint").ToString());

            //dd = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(@"C:\Immi\Personal\MyProjects\you-dl\Immanuel.Ydl\Immanuel.Yt.Cmments.Feedback\Immanuel.Yt.Cmments.Feedback\App_Data\en-EN\Apologizing.json"));
            //List<string> apologize = JsonConvert.DeserializeObject<List<string>>(dd.GetValue("apologize").ToString());

            //string text = "Why pay 400 for 3 instruments when you can get all 3 for 400 each! oh wait....";
            //var sents = text.Split('.').ToList();
            //List<string> wrds = new List<string>();
            //sents.ForEach(t => wrds.AddRange(t.Split(' ')));

            //var vulgarinter = vulgar.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            //vulgarinter = vulgar.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            //var uglyinter = ugly.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            //uglyinter = ugly.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            //var positiveinter = positive.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            //positiveinter = positive.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            //var offensiveinter = offensive.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            //offensiveinter = offensive.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            //var negativeinter = negative.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            //negativeinter = negative.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            //var insultinter = insult.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            //insultinter = insult.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            //var harshinter = harsh.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            //harshinter = harsh.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            //var apologizeinter = apologize.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            //apologizeinter = apologize.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            Console.ReadKey();
        }
    }
}
