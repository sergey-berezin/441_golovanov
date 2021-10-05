using System;
using YOLOv4MLNet;
using System.IO;

namespace YoloC
{
    class Program
    {
        const string imageFolder = @"Assets\Images";
        static void Main(string[] args)
        {
            ParMLcs mlo = new ParMLcs();
            Console.WriteLine("Start");

            var jpegs = Directory.EnumerateFiles(imageFolder, "*.jpg");

            var res = mlo.processFolder(imageFolder);

            foreach (var item in res)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
