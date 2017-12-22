using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Immanuel.Yt.Cmments.Feedback.Models
{
    public class YtVideo
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public PageInfo pageInfo { get; set; }
        public List<Item> items { get; set; }
    }

    public class Statistics
    {
        public string viewCount { get; set; }
        public string likeCount { get; set; }
        public string dislikeCount { get; set; }
        public string favoriteCount { get; set; }
        public string commentCount { get; set; }
    }
}