using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLibrary;
using YoutubeExtractor;

namespace Test.Cons
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(@"https://www.youtube.com/watch?v=3owqvmMf6No", false);
            VideoInfo video = videoInfos
                .First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);
            string source = @"C:\Immi\Personal\MyProjects\you-dl\Immanuel.Ydl\Immanuel.Yl\Immanuel.Yl\Content\Dlds\";
            var inputFile = new MediaFile { Filename = source + "xsFood - Explain.mp4" };
            var outputFile = new MediaFile { Filename = source + "xsFood - Explain - Slit.mp4" };

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);

                var options = new ConversionOptions();
                options.CutMedia(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));

                engine.Convert(inputFile, outputFile, options);
            }
        }

        static void Mp4ToMp3()
        {
            YouTube youtube = YouTube.Default;
            Video vid = youtube.GetVideo("https://www.youtube.com/watch?v=3owqvmMf6No");
            string source = @"C:\Immi\Personal\MyProjects\you-dl\Immanuel.Ydl\Immanuel.Yl\Test.Cons\";
            System.IO.File.WriteAllBytes(source + vid.FullName, vid.GetBytes());

            var inputFile = new MediaFile { Filename = source + vid.FullName };
            var outputFile = new MediaFile { Filename = $"{source + vid.FullName}.mp3" };

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);

                engine.Convert(inputFile, outputFile);
            }
        }
    }
}
