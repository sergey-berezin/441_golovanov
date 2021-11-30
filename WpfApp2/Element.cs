using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace WpfApp2
{
    class Element: INotifyPropertyChanged
    {
        private string name;
        private string path;

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

    class Elements : IEnumerable<string>, INotifyCollectionChanged
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
            added.PropertyChanged += PropChanged;
            collection.Add(added);
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public IEnumerator<string> GetEnumerator()
        {

            var res = new List<string>();
            foreach (var item in collection)
            {
                res.Add(item.Path);
            }
            return ((IEnumerable<string>)res).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var res = new List<string>();
            foreach(var item in collection)
            {
                res.Add(item.Path);
            }
            return ((IEnumerable)res).GetEnumerator();
        }
    }
}
