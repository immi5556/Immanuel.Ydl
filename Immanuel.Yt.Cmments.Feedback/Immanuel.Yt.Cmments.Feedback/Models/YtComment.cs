using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Immanuel.Yt.Cmments.Feedback.Models
{
    public class YtComment
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string nextPageToken { get; set; }
        public PageInfo pageInfo { get; set; }
        public List<Item> items { get; set; }
    }

    public class PageInfo
    {
        public int totalResults { get; set; }
        public int resultsPerPage { get; set; }
    }

    public class AuthorChannelId
    {
        public string value { get; set; }
    }

    public class TopLevelComment
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public Snippet snippet { get; set; }
    }

    public class Snippet
    {
        public string videoId { get; set; }
        public TopLevelComment topLevelComment { get; set; }
        public bool canReply { get; set; }
        public int totalReplyCount { get; set; }
        public bool isPublic { get; set; }
        public string authorDisplayName { get; set; }
        public string authorProfileImageUrl { get; set; }
        public string authorChannelUrl { get; set; }
        public AuthorChannelId authorChannelId { get; set; }
        public string textDisplay { get; set; }
        public string textOriginal { get; set; }
        public bool canRate { get; set; }
        public string viewerRating { get; set; }
        public int likeCount { get; set; }
        public DateTime publishedAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class Item
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public Snippet snippet { get; set; }
        public Statistics statistics { get; set; }
    }
}