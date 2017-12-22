using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using VideoLibrary;
using YoutubeExtractor;

namespace Immanuel.Yl.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class YlController : ApiController
    {
        static int GlobalCnt = 0;
        string pPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Dlds");
        [HttpPost]
        public HttpResponseMessage DlVid()
        {
            ++GlobalCnt;
            string yurl = System.Web.HttpContext.Current.Request.Form["yurl"];
            string lurl = yurl;
            if ((yurl ?? "").ToLower().LastIndexOf("?start=") > -1)
            {
                yurl = (yurl ?? "").Remove((yurl ?? "").ToLower().LastIndexOf("?start="));
            }
            if ((yurl ?? "").ToLower().LastIndexOf("?end=") > -1)
            {
                yurl = (yurl ?? "").Remove((yurl ?? "").ToLower().LastIndexOf("?end="));
            }
            bool owrite = Convert.ToBoolean(System.Web.HttpContext.Current.Request.Form["overwrite"] ?? "false");
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(yurl, false);
            VideoInfo video = videoInfos
                .First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);
            var pth = Path.Combine(pPath, RemoveIllegalPathCharacters(video.Title) + video.VideoExtension);
            var splitVid = Path.Combine(pPath, RemoveIllegalPathCharacters(video.Title) + "-Split" + video.VideoExtension);
            if (owrite && File.Exists(pth))
            {
                File.Delete(pth);
            }
            else if (!owrite && File.Exists(pth))
            {

            }
            else
            {
                DownloadVideo(video, pth);
            }
            Uri myUri = new Uri(lurl);
            string qry = myUri.Query.Substring(myUri.Query.LastIndexOf("?"));
            string strstart = HttpUtility.ParseQueryString(qry).Get("start");
            string strend = HttpUtility.ParseQueryString(qry).Get("end");
            int start = 0, end = 0;
            int.TryParse(strstart, out start);
            int.TryParse(strend, out end);
            if (start > 0 || end > 0)
            {
                var inputFile = new MediaFile { Filename = pth };
                var outputFile = new MediaFile { Filename = splitVid };
                using (var engine = new Engine(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/ffmpeg/ffmpeg.exe")))
                {
                    engine.GetMetadata(inputFile);
                    if (start > 0 || end > 0)
                    {
                        if (start > 0 && end < start)
                            end = inputFile.Metadata.Duration.Seconds;
                        var options = new ConversionOptions();
                        options.CutMedia(TimeSpan.FromSeconds(start), TimeSpan.FromSeconds(end));
                        engine.Convert(inputFile, outputFile, options);
                    }
                    else
                    {
                        engine.Convert(inputFile, outputFile);
                    }
                    return new HttpResponseMessage()
                    {
                        Content = new JsonContent(new
                        {
                            Path = System.Web.HttpUtility.UrlEncode(@"/Content/Dlds/" + Path.GetFileName(splitVid)),
                            FileName = Path.GetFileName(splitVid),
                            TotCnt = GlobalCnt
                        })
                    };
                }
            }
            return new HttpResponseMessage()
            {
                Content = new JsonContent(new
                {
                    Path = System.Web.HttpUtility.UrlEncode(@"/Content/Dlds/" + Path.GetFileName(pth)),
                    FileName = Path.GetFileName(pth),
                    TotCnt = GlobalCnt
                })
            };
        }

        [HttpPost]
        public HttpResponseMessage Dlv()
        {
            ++GlobalCnt;
            string yurl = System.Web.HttpContext.Current.Request.Form["yurl"];
            bool owrite = Convert.ToBoolean(System.Web.HttpContext.Current.Request.Form["overwrite"] ?? "false");
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(yurl, false);
            VideoInfo video = videoInfos
                .First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);
            var pth = Path.Combine(pPath, RemoveIllegalPathCharacters(video.Title) + video.VideoExtension);
            if (owrite && File.Exists(pth))
            {
                File.Delete(pth);
            }
            else if (!owrite && File.Exists(pth))
            {

            }
            else
            {
                DownloadVideo(video, pth);
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            byte[] arr = File.ReadAllBytes(pth);
            response.Content = new StreamContent(new MemoryStream(arr));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(GetMimeTypes(Path.GetExtension(pth)));
            response.Content.Headers.ContentLength = arr.Length;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = Path.GetFileName(pth)
            };

            return response;
        }

        string GetMimeTypes(string ext)
        {
            ext = ext.StartsWith(".") ? ext.Split('.')[1] : ext;
            string mime = "";
            switch (ext)
            {
                case "flv":
                    mime = "video/x-flv";
                    break;
                case "mp4":
                    mime = "video/mp4";
                    break;
                case "avi":
                    mime = "video/avi";
                    break;
                case "wmv":
                    mime = "video/x-ms-wmv";
                    break;
                case "mpg":
                    mime = "video/mpeg";
                    break;
                case "mov":
                    mime = "video/quicktime";
                    break;
                case "m4v":
                    mime = "video/x-m4v";
                    break;
                case "mkv":
                    mime = "video/x-matroska";
                    break;
                case "webm":
                    mime = "video/webm";
                    break;
                case "3gp":
                    mime = "video/3gpp";
                    break;
            }
            return mime;
        }

        [HttpPost]
        public HttpResponseMessage DlAud()
        {
            ++GlobalCnt;
            try
            {
                string yurl = System.Web.HttpContext.Current.Request.Form["yurl"];
                string lurl = yurl;
                if((yurl ?? "").ToLower().LastIndexOf("?start=") > -1)
                {
                    yurl = (yurl ?? "").Remove((yurl ?? "").ToLower().LastIndexOf("?start="));
                }
                if ((yurl ?? "").ToLower().LastIndexOf("?end=") > -1)
                {
                    yurl = (yurl ?? "").Remove((yurl ?? "").ToLower().LastIndexOf("?end="));
                }
                bool owrite = Convert.ToBoolean(System.Web.HttpContext.Current.Request.Form["overwrite"] ?? "false");
                IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(yurl, false);
                VideoInfo video = videoInfos
                    .First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);
                var pth = Path.Combine(pPath, RemoveIllegalPathCharacters(video.Title) + video.VideoExtension);
                string audfil = Path.ChangeExtension(pth, "mp3");

                if (owrite && File.Exists(audfil))
                {
                    File.Delete(audfil);
                }
                else if (!owrite && File.Exists(audfil))
                {

                }
                else
                {
                    if (File.Exists(pth))
                    {
                        //File.Delete(pth);
                    }
                    else
                    {
                        DownloadVideo(video, pth);
                    }
                    Uri myUri = new Uri(lurl);
                    string strstart = HttpUtility.ParseQueryString(myUri.Query).Get("start");
                    string strend = HttpUtility.ParseQueryString(myUri.Query).Get("end");
                    long start = 0, end = 0;
                    long.TryParse(strstart, out start);
                    long.TryParse(strend, out end);
                    var inputFile = new MediaFile { Filename = pth };
                    var outputFile = new MediaFile { Filename = audfil };
                    using (var engine = new Engine(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/ffmpeg/ffmpeg.exe")))
                    {
                        engine.GetMetadata(inputFile);
                        if (start > 0 || end > 0)
                        {
                            if (start > 0 && end < start)
                                end = inputFile.Metadata.Duration.Ticks;
                            var options = new ConversionOptions();
                            options.CutMedia(TimeSpan.FromSeconds(start), TimeSpan.FromSeconds(end));
                            engine.Convert(inputFile, outputFile, options);
                        }
                        else
                        {
                            engine.Convert(inputFile, outputFile);
                        }
                    }
                }
                return new HttpResponseMessage()
                {
                    Content = new JsonContent(new
                    {
                        Path = System.Web.HttpUtility.UrlEncode(@"/Content/Dlds/" + Path.GetFileName(audfil)),
                        FileName = Path.GetFileName(audfil),
                        TotCnt = GlobalCnt
                    })
                };
            }
            catch (Exception exp)
            {
                return new HttpResponseMessage()
                {
                    Content = new JsonContent(new
                    {
                        Path = System.Web.HttpUtility.UrlEncode(@"/Content/Dlds/"),
                        FileName = "Error Occured",
                        TotCnt = GlobalCnt,
                        Excp = exp.ToString()
                    })
                };
            }
        }

        [HttpPost]
        public HttpResponseMessage Dla()
        {
            ++GlobalCnt;
            string err = "";
            string yurl = System.Web.HttpContext.Current.Request.Form["yurl"];
            bool owrite = Convert.ToBoolean(System.Web.HttpContext.Current.Request.Form["overwrite"] ?? "false");
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(yurl, false);
            VideoInfo video = videoInfos
                .First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);
            var pth = Path.Combine(pPath, RemoveIllegalPathCharacters(video.Title) + video.VideoExtension);
            string audfil = Path.ChangeExtension(pth, "mp3");
            try
            {
                if (owrite && File.Exists(audfil))
                {
                    File.Delete(audfil);
                }
                else if (!owrite && File.Exists(audfil))
                {

                }
                else
                {
                    if (File.Exists(pth))
                    {
                        //File.Delete(pth);
                    }
                    else
                    {
                        DownloadVideo(video, pth);
                    }
                    var inputFile = new MediaFile { Filename = pth };
                    var outputFile = new MediaFile { Filename = audfil };
                    using (var engine = new Engine(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/ffmpeg/ffmpeg.exe")))
                    {
                        engine.GetMetadata(inputFile);
                        engine.Convert(inputFile, outputFile);
                    }
                }
            }
            catch (Exception ex)
            {
                err = ex.ToString();
            }

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            byte[] arr = string.IsNullOrEmpty(err) ? File.ReadAllBytes(audfil) : System.Text.Encoding.UTF8.GetBytes(err);
            response.Content = new StreamContent(new MemoryStream(arr));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");
            response.Content.Headers.ContentLength = arr.Length;
            response.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = Path.GetFileName(audfil)
            };

            return response;
        }

        private void DownloadVideo(VideoInfo video, string pth)
        {
            /*
             * Select the first .mp4 video with 360p resolution
             */
            //VideoInfo video = videoInfos
            //    .First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);

            /*
             * If the video has a decrypted signature, decipher it
             */
            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            /*
             * Create the video downloader.
             * The first argument is the video to download.
             * The second argument is the path to save the video file.
             */
            var videoDownloader = new VideoDownloader(video, pth);

            // Register the ProgressChanged event and print the current progress
            videoDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage);

            /*
             * Execute the video downloader.
             * For GUI applications note, that this method runs synchronously.
             */
            videoDownloader.Execute();
        }

        private static void DownloadAudio(VideoInfo video, string pth)
        {
            /*
             * We want the first extractable video with the highest audio quality.
             */

            /*
             * If the video has a decrypted signature, decipher it
             */
            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            /*
             * Create the audio downloader.
             * The first argument is the video where the audio should be extracted from.
             * The second argument is the path to save the audio file.
             */

            var audioDownloader = new AudioDownloader(video, pth);

            // Register the progress events. We treat the download progress as 85% of the progress
            // and the extraction progress only as 15% of the progress, because the download will
            // take much longer than the audio extraction.
            audioDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage * 0.85);
            audioDownloader.AudioExtractionProgressChanged += (sender, args) => Console.WriteLine(85 + args.ProgressPercentage * 0.15);

            /*
             * Execute the audio downloader.
             * For GUI applications note, that this method runs synchronously.
             */
            audioDownloader.Execute();
        }

        private static string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }
    }

    public class JsonContent : HttpContent
    {

        private readonly MemoryStream _Stream = new MemoryStream();
        public JsonContent(object value)
        {

            Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var jw = new JsonTextWriter(new StreamWriter(_Stream));
            jw.Formatting = Formatting.Indented;
            var serializer = new JsonSerializer();
            serializer.Serialize(jw, value);
            jw.Flush();
            _Stream.Position = 0;

        }
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return _Stream.CopyToAsync(stream);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _Stream.Length;
            return true;
        }
    }
}