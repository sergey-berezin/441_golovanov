using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using YOLOv4MLNet.DataStructures;
using static Microsoft.ML.Transforms.Image.ImageResizingEstimator;



namespace YOLOv4MLNet
{
    public class ParMLcs
    {
        const string modelPath = @"C:\Users\Nikita\Projects\Models\yolov4.onnx";

        //const string imageFolder = @"Assets\Images";

        //const string imageOutputFolder = @"Assets\Output";

        static readonly string[] classesNames = new string[] { "person", "bicycle", "car", "motorbike", "aeroplane", "bus", "train", "truck", "boat", "traffic light", "fire hydrant", "stop sign", "parking meter", "bench", "bird", "cat", "dog", "horse", "sheep", "cow", "elephant", "bear", "zebra", "giraffe", "backpack", "umbrella", "handbag", "tie", "suitcase", "frisbee", "skis", "snowboard", "sports ball", "kite", "baseball bat", "baseball glove", "skateboard", "surfboard", "tennis racket", "bottle", "wine glass", "cup", "fork", "knife", "spoon", "bowl", "banana", "apple", "sandwich", "orange", "broccoli", "carrot", "hot dog", "pizza", "donut", "cake", "chair", "sofa", "pottedplant", "bed", "diningtable", "toilet", "tvmonitor", "laptop", "mouse", "remote", "keyboard", "cell phone", "microwave", "oven", "toaster", "sink", "refrigerator", "book", "clock", "vase", "scissors", "teddy bear", "hair drier", "toothbrush" };

        private static SemaphoreSlim sm;


        public List<resClass> processImage(Bitmap image)
        {
            MLContext mlContext = new MLContext();

            var pipeline = mlContext.Transforms.ResizeImages(inputColumnName: "bitmap", outputColumnName: "input_1:0", imageWidth: 416, imageHeight: 416, resizing: ResizingKind.IsoPad)
                .Append(mlContext.Transforms.ExtractPixels(outputColumnName: "input_1:0", scaleImage: 1f / 255f, interleavePixelColors: true))
                .Append(mlContext.Transforms.ApplyOnnxModel(
                    shapeDictionary: new Dictionary<string, int[]>()
                    {
                        { "input_1:0", new[] { 1, 416, 416, 3 } },
                        { "Identity:0", new[] { 1, 52, 52, 3, 85 } },
                        { "Identity_1:0", new[] { 1, 26, 26, 3, 85 } },
                        { "Identity_2:0", new[] { 1, 13, 13, 3, 85 } },
                    },
                    inputColumnNames: new[]
                    {
                        "input_1:0"
                    },
                    outputColumnNames: new[]
                    {
                        "Identity:0",
                        "Identity_1:0",
                        "Identity_2:0"
                    },
                    modelFile: modelPath, recursionLimit: 100));

            // Fit on empty list to obtain input data schema
            var model = pipeline.Fit(mlContext.Data.LoadFromEnumerable(new List<YoloV4BitmapData>()));

            var predictionEngine = mlContext.Model.CreatePredictionEngine<YoloV4BitmapData, YoloV4Prediction>(model);

            var predict = predictionEngine.Predict(new YoloV4BitmapData() { Image = image });
            var results = predict.GetResults(classesNames, 0.3f, 0.7f);

            List<resClass> res = new List<resClass>();

            foreach(var item in results)
            {
                var resItem = new resClass(item.Label, item.BBox[0], item.BBox[1], item.BBox[2], item.BBox[3], item.Confidence);
                res.Add(resItem);
            }

            return res;
        }



        public List<imageRes> processFolder(string folder)
        {
            var jpegs = Directory.EnumerateFiles(folder, "*.jpg");
            int length = 0;
            foreach (var jpeg in jpegs)
                length++;

            sm = new SemaphoreSlim(1);
            int x = 0;
            int i = 0;

            var tasks = new Task<imageRes>[length];

            foreach (var jpeg in jpegs)
            {
                var ajpeg = jpeg;
                tasks[i] = Task<imageRes>.Factory.StartNew(() =>
                {
                    var bitmap = new Bitmap(Image.FromFile(ajpeg));
                    var res = processImage(bitmap);
                    sm.Wait();
                    Interlocked.Increment(ref x);
                    float procent = (float)x / (float)length * 100;
                    Console.WriteLine(procent.ToString());
                    sm.Release();
                    return new imageRes(ajpeg, res);
                });
                i++;
            }

            var task3 = Task.WhenAll<imageRes>(tasks).ContinueWith(combined => {
                var res = new List<imageRes>();
                foreach(var item in combined.Result)
                {
                    res.Add(item);
                }
                return res;
            });

            task3.Wait();

           return task3.Result;
        }
    }
}
