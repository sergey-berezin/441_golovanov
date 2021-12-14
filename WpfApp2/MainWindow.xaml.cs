using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System.Collections.ObjectModel;
using YOLOv4MLNet;
using System.Threading;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Threading;
using System.Drawing.Imaging;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Elements test = new Elements();
        private static CancellationToken token;
        private static CancellationTokenSource ts;
        private static SemaphoreSlim sm = new SemaphoreSlim(1);
        private static ImageDB db;

        public MainWindow()
        {
            InitializeComponent();
            db = new ImageDB();
            Console.WriteLine("Start");
            foreach(var item in db.images) {
                db.Entry(item).Reference(x => x.BLOB).Load();

                Element el = new Element();
                el.Name = item.ImageId.ToString();
                el.Path = item.ImageId.ToString();
                MemoryStream blb = new MemoryStream(item.BLOB.Img);
                var btmp1 = new Bitmap(System.Drawing.Image.FromStream(blb));
                el.bitmap = btmp1;
                test.AddEl(el);
                Console.WriteLine("Add image");
            }
            DataContext = test;
        }

        void Open_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog folder = new VistaFolderBrowserDialog();
            bool result = (bool)folder.ShowDialog();

            if (result == true)
            {
                textBlock.Text = folder.SelectedPath;
                var jpegs = Directory.EnumerateFiles(folder.SelectedPath, "*.jpg");

                foreach(var jpeg in jpegs)
                {
                    test.Add(jpeg);
                }
            }
        }

        static public int Hash(string x)
        {
            int res = 0;
            foreach (var k in x)
            {
                res = (res + (int)k) % 3688765;
            }
            return 0;
        }

        private async void btnRun_Click(object sender, RoutedEventArgs e)
        {
            button1.IsEnabled = false;

            test.Clear();
            var imageOutputFolder = textBlock.Text + "\\Output";
            Directory.CreateDirectory(imageOutputFolder);
            var coll = new ObservableCollection<imageRes>();
            coll.CollectionChanged += handler;
            
            ts = new CancellationTokenSource();
            token = ts.Token;

            var parser = new ParMLcs();

            await parser.ProcessFolder(textBlock.Text, coll, token);

            foreach (var item in coll)
            {
                 var file_name = System.IO.Path.GetFileName(item.imgName);
                 var res_file_name = System.IO.Path.Combine(imageOutputFolder, file_name);
                var bitmap = new Bitmap(System.Drawing.Image.FromFile(res_file_name));
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Jpeg);
                byte[] blob = ms.ToArray();
                //test.Add(res_file_name);

                var hash = Hash(Convert.ToBase64String(blob));

                bool add = true;

                foreach (var img in db.images)
                {
                    if (hash == img.ImageHash)
                    {
                        db.Entry(img).Reference(x => x.BLOB).Load();
                        var res = Convert.ToBase64String(img.BLOB.Img) == Convert.ToBase64String(blob);
                        if (res)
                        {
                            add = false;

                        }
                    }
                }

                if (add)
                {
                    var imageblb = new ImageBlob { Img = blob };
                    var image = new Image { ImageHash = hash, BLOB = imageblb };
                    image.boxes = new List<Box>();

                    foreach(var box in item.results)
                    {
                        image.boxes.Add(new Box() { Label = item.imgName, Confidence = box.confidence, x1 = box.box[0],
                            x2 = box.box[1], x3 = box.box[2], x4 = box.box[3]});
                    }

                    //db.Add(imageblb);
                    db.Add(image);
                    db.SaveChanges();
                }
            }

            foreach (var item in db.images)
            {
                db.Entry(item).Reference(x => x.BLOB).Load();

                Element el = new Element();
                el.Name = item.ImageId.ToString();
                el.Path = item.ImageId.ToString();
                MemoryStream blb = new MemoryStream(item.BLOB.Img);
                var btmp1 = new Bitmap(System.Drawing.Image.FromStream(blb));
                el.bitmap = btmp1;
                test.AddEl(el);
            }

            button1.IsEnabled = true;
        }

        void handler(object sender, NotifyCollectionChangedEventArgs e)
        {
            var coll = (ObservableCollection<imageRes>)sender;

            foreach(var item in e.NewItems.Cast<imageRes>())
            {
                    var bitmap = new Bitmap(System.Drawing.Image.FromFile(item.imgName));

                    using (var g = Graphics.FromImage(bitmap))
                    {
                        foreach (var res in item.results)
                        {
                            // draw predictions
                            var x1 = res.box[0];
                            var y1 = res.box[1];
                            var x2 = res.box[2];
                            var y2 = res.box[3];
                            g.DrawRectangle(Pens.Red, x1, y1, x2 - x1, y2 - y1);
                            using (var brushes = new SolidBrush(System.Drawing.Color.FromArgb(50, System.Drawing.Color.Red)))
                            {
                                g.FillRectangle(brushes, x1, y1, x2 - x1, y2 - y1);
                            }

                            g.DrawString(res.label + " " + res.confidence.ToString("0.00"),
                                         new Font("Arial", 12), System.Drawing.Brushes.Blue, new PointF(x1, y1));
                        }
                        var imageOutputFolder = System.IO.Path.GetDirectoryName(item.imgName) + "\\Output";
                        var file_name = System.IO.Path.GetFileName(item.imgName);
                        var res_file_name = System.IO.Path.Combine(imageOutputFolder, file_name);

                        sm.Wait();
                        bitmap.Save(res_file_name);
                        //test.collection[index].Path = res_file_name;
                        sm.Release();
                    }
                
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ts.Cancel();
            button1.IsEnabled = true;
        }
    } 
}
