using System;
using YOLOv4MLNet;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using YOLOv4MLNet.DataStructures;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.ML.Transforms.Image.ImageResizingEstimator;
using System.Collections.ObjectModel;

namespace ConsoleApp
{
    class Program
    {
        const string imageFolder = @"Assets\Images";
        static async Task Main()
        {
            ParMLcs mlo = new ParMLcs();
            Console.WriteLine("Start");

            var jpegs = Directory.EnumerateFiles(imageFolder, "*.jpg");

            var coll = new ObservableCollection<imageRes>();
            var cancelTS = new CancellationTokenSource();
            var cancelT = cancelTS.Token;


            await mlo.ProcessFolder(imageFolder, coll, cancelT);

            foreach (var item in coll)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
