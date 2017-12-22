using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Immanuel.Yt.Cmments.Feedback.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReviewController : ApiController
    {
        [HttpPost]
        [Route("review/youtube")]
        public string YoutubePost()
        {
            string youtubeid = System.Web.HttpContext.Current.Request.Form["youtubeid"];
            string clientid = System.Web.HttpContext.Current.Request.Form["clientid"];
            return Logic.YtValid.Validate(clientid, youtubeid);
        }

        [HttpGet]
        [Route("review/youtube/{url}/{clientid}")]
        public string YoutubeGet(string url, string clientid)
        {
            return Logic.YtValid.Validate(clientid, url);
        }
    }
}