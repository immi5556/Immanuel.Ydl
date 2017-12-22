using Immanuel.Yt.Cmments.Feedback.Models;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Immanuel.Yt.Cmments.Feedback.Logic
{
    public class YtValid
    {
        public static string Validate(string clientid, string url)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
            //hubContext.Clients.User(clientid).progress("Called from Controller");
            hubContext.Clients.Client(clientid).progress("getting video info..");
            string uid = GetYid(url);
            WebClient client = new WebClient();
            string json = client.DownloadString("https://www.googleapis.com/youtube/v3/videos?part=statistics&key=AIzaSyBRhULlCbQcdBk_HLFrpP-UU6VlyXInAPQ&id=" + uid);
            YtVideo jo1 = JsonConvert.DeserializeObject<YtVideo>(json);
            hubContext.Clients.Client(clientid).progress("getting video comments...");
            json = client.DownloadString("https://www.googleapis.com/youtube/v3/commentThreads?key=AIzaSyBRhULlCbQcdBk_HLFrpP-UU6VlyXInAPQ&textFormat=plainText&part=snippet&maxResults=100&videoId=" + uid);
            YtComment jo = JsonConvert.DeserializeObject<YtComment>(json);
            hubContext.Clients.Client(clientid).progress("validating the feed...");
            var rest = GetWordCheck(jo);
            hubContext.Clients.Client(clientid).progress("Completed...");
            return GetRating(rest, jo1);
        }

        static string GetRating(string str, YtVideo vid)
        {
            StringBuilder repp = new StringBuilder();
            List<Models.TextRank> lst = JsonConvert.DeserializeObject<List<Models.TextRank>>(str);
            var ll = from l in lst
                     group l by new
                     {
                         l.ProfaneRate,
                         l.WordPercentage
                     } into gcs
                     //select gcs;
                     select new TextRank()
                     {
                         ProfaneRate = gcs.Key.ProfaneRate,
                         WordPercentage = gcs.Key.WordPercentage,
                         Count = gcs.Count()
                     };
            var lw = ll.ToList();
            TextRank tot0 = null, tot3 =null, tot10 = null, tot20 = null, tot30 = null, tot40 = null,
                tot50 = null, tot60 = null, tot70 = null, tot80 = null, tot90 = null, tot100 = null;
            lw.ForEach(t1 =>
            {
                Console.WriteLine(t1.Count);
                if (t1.WordPercentage == 0)
                    tot0 = t1;
                else if (t1.WordPercentage == 3)
                    tot3 = t1;
                else if (t1.WordPercentage == 10)
                    tot10 = t1;
                else if (t1.WordPercentage == 20)
                    tot20 = t1;
                else if (t1.WordPercentage == 30)
                    tot30 = t1;
                else if (t1.WordPercentage == 40)
                    tot40 = t1;
                else if (t1.WordPercentage == 50)
                    tot50 = t1;
                else if (t1.WordPercentage == 60)
                    tot60 = t1;
                else if (t1.WordPercentage == 70)
                    tot70 = t1;
                else if (t1.WordPercentage == 80)
                    tot80 = t1;
                else if (t1.WordPercentage == 90)
                    tot90 = t1;
                else if (t1.WordPercentage == 100)
                    tot100 = t1;
            });
            Item itm = null;
            if (vid.items != null && vid.items.Count > 0)
            {
                itm = vid.items[0];
            }
            if (tot0 != null)
            {
                if (itm != null)
                {
                    //int iu = Convert.ToInt32(itm.statistics.commentCount);
                    int iu = 100;
                    double pcnt = (double)tot0.Count / iu * 100;
                    //repp += tot0.ProfaneRate + " Comments recieved, " + Math.Round(pcnt, 2).ToString() + "% - Of Initial 100 Comments assessed." + Environment.NewLine;
                    repp.AppendLine(tot0.ProfaneRate + " Comments recieved, " + Math.Round(pcnt, 2).ToString() + "% - Of Initial 100 Comments assessed.");
                }
                else
                {
                    //repp += tot0.ProfaneRate + Environment.NewLine;
                    repp.AppendLine(tot0.ProfaneRate);
                }
            }
            if (tot3 != null)
            {
                if (itm != null)
                {
                    //int iu = Convert.ToInt32(itm.statistics.commentCount);
                    int iu = 100;
                    double pcnt = (double)tot3.Count / iu * 100;
                    //repp += tot3.ProfaneRate + " Comments recieved, " + Math.Round(pcnt, 2).ToString() + "% - Of Initial 100 Comments assessed." + Environment.NewLine;
                    repp.AppendLine(tot3.ProfaneRate + " Comments recieved, " + Math.Round(pcnt, 2).ToString() + "% - Of Initial 100 Comments assessed.");
                }
                else
                {
                    //repp += tot3.ProfaneRate + Environment.NewLine;
                    repp.AppendLine(tot3.ProfaneRate);
                }
            }
            if (tot10 != null)
            {
                if (itm != null)
                {
                    //int iu = Convert.ToInt32(itm.statistics.commentCount);
                    int iu = 100;
                    double pcnt = (double)tot10.Count / iu * 100;
                    repp.AppendLine(tot10.ProfaneRate + " Comments recieved, " + Math.Round(pcnt, 2).ToString() + "% - Of Initial 100 Comments assessed.");
                }
                else
                {
                    repp.AppendLine(tot10.ProfaneRate);
                }
            }
            if (tot20 != null)
            {
                if (itm != null)
                {
                    //int iu = Convert.ToInt32(itm.statistics.commentCount);
                    int iu = 100;
                    double pcnt = (double)tot20.Count / iu * 100;
                    repp.AppendLine(tot20.ProfaneRate + " Comments recieved, " + Math.Round(pcnt, 2).ToString() + "% - Of Initial 100 Comments assessed.");
                }
                else
                {
                    repp.AppendLine(tot20.ProfaneRate);
                }
            }
            if (tot30 != null)
            {
                if (itm != null)
                {
                    //int iu = Convert.ToInt32(itm.statistics.commentCount);
                    int iu = 100;
                    double pcnt = (double)tot30.Count / iu * 100;
                    repp.AppendLine(tot30.ProfaneRate + " Comments recieved, " + Math.Round(pcnt, 2).ToString() + "% - Of Initial 100 Comments assessed.");
                }
                else
                {
                    repp.AppendLine(tot30.ProfaneRate);
                }
            }
            if (tot40 != null)
            {
                if (itm != null)
                {
                    //int iu = Convert.ToInt32(itm.statistics.commentCount);
                    int iu = 100;
                    double pcnt = (double)tot40.Count / iu * 100;
                    repp.AppendLine(tot40.ProfaneRate + " Comments recieved, " + Math.Round(pcnt, 2).ToString() + "% - Of Initial 100 Comments assessed.");
                }
                else
                {
                    repp.AppendLine(tot40.ProfaneRate);
                }
            }
            if (tot50 != null)
            {
                if (itm != null)
                {
                    //int iu = Convert.ToInt32(itm.statistics.commentCount);
                    int iu = 100;
                    double pcnt = (double)tot50.Count / iu * 100;
                    repp.AppendLine(tot50.ProfaneRate + " Comments recieved, " + Math.Round(pcnt, 2).ToString() + "% - Of Initial 100 Comments assessed.");
                }
                else
                {
                    repp.AppendLine(tot50.ProfaneRate);
                }
            }
            if (tot60 != null)
            {
                if (itm != null)
                {
                    //int iu = Convert.ToInt32(itm.statistics.commentCount);
                    int iu = 100;
                    double pcnt = (double)tot60.Count / iu * 100;
                    repp.AppendLine(tot60.ProfaneRate + " Comments recieved, " + Math.Round(pcnt, 2).ToString() + "% - Of Initial 100 Comments assessed.");
                }
                else
                {
                    repp.AppendLine(tot60.ProfaneRate);
                }
            }
            if (tot70 != null)
            {
                if (itm != null)
                {
                    //int iu = Convert.ToInt32(itm.statistics.commentCount);
                    int iu = 100;
                    double pcnt = (double)tot70.Count / iu * 100;
                    repp.AppendLine(tot70.ProfaneRate + " Comments recieved, " + Math.Round(pcnt, 2).ToString() + "% - Of Initial 100 Comments assessed.");
                }
                else
                {
                    repp.AppendLine(tot70.ProfaneRate);
                }
            }
            if (tot80 != null)
            {
                if (itm != null)
                {
                    //int iu = Convert.ToInt32(itm.statistics.commentCount);
                    int iu = 100;
                    double pcnt = (double)tot80.Count / iu * 100;
                    repp.AppendLine(tot80.ProfaneRate + " Comments recieved, " + Math.Round(pcnt, 2).ToString() + "% - Of Initial 100 Comments assessed.");
                }
                else
                {
                    repp.AppendLine(tot80.ProfaneRate);
                }
            }
            if (tot90 != null)
            {
                if (itm != null)
                {
                    //int iu = Convert.ToInt32(itm.statistics.commentCount);
                    int iu = 100;
                    double pcnt = (double)tot90.Count / iu * 100;
                    repp.AppendLine(tot90.ProfaneRate + " Comments recieved, " + Math.Round(pcnt, 2).ToString() + "% - Of Initial 100 Comments assessed.");
                }
                else
                {
                    repp.AppendLine(tot90.ProfaneRate);
                }
            }
            if (tot100 != null)
            {
                if (itm != null)
                {
                    //int iu = Convert.ToInt32(itm.statistics.commentCount);
                    int iu = 100;
                    double pcnt = (double)tot100.Count / iu * 100;
                    repp.AppendLine(tot100.ProfaneRate + " Comments recieved, " + Math.Round(pcnt, 2).ToString() + "% - Of Initial 100 Comments assessed.");
                }
                else
                {
                    repp.AppendLine(tot100.ProfaneRate);
                }
            }

            return repp.ToString();
        }

        static string GetYid(string url)
        {
            var regexString = @"^.*(youtu\.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=)([^#\&\?]*).*";
            string uid3 = Regex.Match(url, regexString).Groups[2].Value;
            return string.IsNullOrEmpty(uid3) ? url : uid3;
        }

        static void AddCmts(Snippet t, List<string> lst)
        {
            if (!string.IsNullOrEmpty(t.textOriginal))
                lst.Add(t.textOriginal);
            if (t.topLevelComment != null && t.topLevelComment.snippet != null)
                lst.Add(t.topLevelComment.snippet.textOriginal);
        }

        static string GetWordCheck(YtComment jo)
        {
            List<string> lst = new List<string>();
            jo.items.ForEach(t =>
            {
                if (t.snippet != null)
                    AddCmts(t.snippet, lst);
            });
            WebClient client = new WebClient();
            System.Collections.Specialized.NameValueCollection formData = new System.Collections.Specialized.NameValueCollection();
            formData["comments"] = JsonConvert.SerializeObject(lst);
            byte[] resp = client.UploadValues(@"http://vulgarity-validator.immanuel.co/Profane/Comments", formData);
            return System.Text.Encoding.UTF8.GetString(resp);
        }
    }
}