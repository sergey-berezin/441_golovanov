using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;

namespace WpfApp2
{
    class Element: INotifyPropertyChanged
    {
        private string name;
        private string path;
        public Bitmap bitmap;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("change"));
            }
        }

        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("change"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    class Elements : IEnumerable<BitmapImage>, INotifyCollectionChanged
    {
        public List<Element> collection;

        public Elements()
        {
            collection = new List<Element>();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void PropChanged(object sender, PropertyChangedEventArgs arg)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Add(string filename)
        {
            var added = new Element();
            added.Name = filename;
            added.Path = filename;
            added.bitmap = new Bitmap(System.Drawing.Image.FromFile(filename));
            added.PropertyChanged += PropChanged;
            collection.Add(added);
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void AddEl(Element el)
        {
            el.PropertyChanged += PropChanged;
            collection.Add(el);
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Clear()
        {
            collection = new List<Element>();
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        public IEnumerator<BitmapImage> GetEnumerator()
        {

            var res = new List<BitmapImage>();
            foreach (var item in collection)
            {
                res.Add(Convert(item.bitmap));
            }
            return res.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var res = new List<BitmapImage>();
            foreach(var item in collection)
            {
                res.Add(Convert(item.bitmap));
            }
            return ((IEnumerable)res).GetEnumerator();
        }
    }
}
