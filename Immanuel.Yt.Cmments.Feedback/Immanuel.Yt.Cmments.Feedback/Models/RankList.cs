using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Immanuel.Yt.Cmments.Feedback.Models
{
    public class CountRank
    {
        public int CountLess { get; set; }
        public string Rank { get; set; }
    }

    public class RankList
    {
        public List<CountRank> ViewCountRank { get; set; }
        public List<CountRank> CommentCountRank { get; set; }
        public List<CountRank> LikeCountRank { get; set; }
        public List<CountRank> DisLikeCountRank { get; set; }
        public List<CountRank> FavouriteCountRank { get; set; }
        public string Disclaimer { get; set; }
    }

    public class TextRank
    {
        public int WordPercentage { get; set; }
        public string ProfaneRate { get; set; }
        public int Count { get; set; }
    }
}