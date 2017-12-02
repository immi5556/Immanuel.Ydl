using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLibrary;

namespace Test.Cons
{
    class Program
    {
        static void Main(string[] args)
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
