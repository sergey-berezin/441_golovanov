using System;
using YOLOv4MLNet;
using Microsoft.ML;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using YOLOv4MLNet.DataStructures;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.ML.Transforms.Image.ImageResizingEstimator;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;

namespace ConsoleApp
{
    public class Image
    {
        public int ImageId { get; set; }

        public int ImageHash { get; set; }

        public ImageBlob BLOB { get; set; }

        public List<Box> boxes { get; set; } = new List<Box>();

    }

    public class ImageBlob
    {
        public int ImageBlobId { get; set; }
        public byte[] Img { get; set; }
    }

    public class Box
    {
        public int BoxId { get; set; }

        public string Label { get; set; }
        public int x1 { get; set; }
        public int x2 { get; set; }
        public int x3 { get; set; }

        public int x4 { get; set; }
        public float Confidence { get; set; }

        public int ImageId { get; set; }

        public Image Image { get; set; }
    }

    public class ImageDB : DbContext
    {
        public DbSet<Image> images { get; set; }

        public DbSet<Box> boxes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder o) => o.UseSqlite("Data Source=images.db");
    }



    class Program
    {
        const string imageFolder = @"Assets\Images";

        static public int Hash(string x)
        {
            int res = 0;
            foreach(var k in x)
            {
                res = (res + (int)k) % 3688765;
            }
            return 0;
        }


        static void  Main()
        {
            ParMLcs mlo = new ParMLcs();
            Console.WriteLine("Start");

            var jpegs = Directory.EnumerateFiles(imageFolder, "*.jpg");

            //var bitmap = new Bitmap(System.Drawing.Image.FromFile(jpegs));

            foreach(var jpeg in jpegs)
            {
                var bitmap = new Bitmap(System.Drawing.Image.FromFile(jpeg));
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Jpeg);
                byte[] blob = ms.ToArray();

                int hash = Hash(Convert.ToBase64String(blob));


                using (var db = new ImageDB())
                {
                    /*var imageblb = new ImageBlob { Img = blob };
                    var image = new Image { ImageHash = hash, BLOB = imageblb };

                    //db.Add(imageblb);
                    db.Add(image);
                    db.SaveChanges();*/
                    bool add = true;

                    foreach(var img in db.images)
                    {
                        if(hash == img.ImageHash)
                        {
                            db.Entry(img).Reference(x => x.BLOB).Load();
                            //db.Entry(img).Collection(x => x.boxes).Load();
                            //Console.WriteLine(((List<Box>)img.boxes)[0].x1);
                            /*MemoryStream blb = new MemoryStream(img.BLOB.Img);
                            var btmp1 = new Bitmap(System.Drawing.Image.FromStream(blb));*/
                            var res = Convert.ToBase64String(img.BLOB.Img) == Convert.ToBase64String(blob);
                            if (res)
                            {
                                add = false;
                                
                            }
                        }
                    }

                    Console.WriteLine(add.ToString());

                    if(add)
                    {
                        var imageblb = new ImageBlob { Img = blob };
                        var image = new Image { ImageHash = hash, BLOB = imageblb };
                        image.boxes = new List<Box>();
                        image.boxes.Add(new Box());

                        //db.Add(imageblb);
                        db.Add(image);
                        db.SaveChanges();
                    }

                    /*foreach (var p in db.images)
                        db.images.Remove(p);
                    

                    db.SaveChanges();*/
                }

                var btmp = new Bitmap(System.Drawing.Image.FromStream(ms));

                Console.WriteLine("1");
            }

            /*var coll = new ObservableCollection<imageRes>();
            var cancelTS = new CancellationTokenSource();
            var cancelT = cancelTS.Token;

            

            await mlo.ProcessFolder(imageFolder, coll, cancelT);

            foreach (var item in coll)
            {
                Console.WriteLine(item.ToString());
            }*/
        }
    }
}
