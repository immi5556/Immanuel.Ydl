using Immanuel.Yt.Cmments.Feedback.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Immanuel.Yt.Cmments.Feedback.App_Start
{
    public class ConfigContext
    {
        static public RankList Ranks { get; set; }

        public static void Start()
        {
            string pPath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data");
            Ranks = Newtonsoft.Json.JsonConvert.DeserializeObject<RankList>(File.ReadAllText(Path.Combine(pPath, "config.json")));
        }
    }
}