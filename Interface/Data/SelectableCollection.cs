using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceos.Interface.Data
{
    /// <summary>
    /// This class encapsulates an Enumerable throw the Source property and expose an SelectableItem collection.
    /// If you want this class to respond to Sources changes, it should implement the INotifyPropertyChanged interface.
    /// </summary>
    public class SelectableCollection<T> : IEnumerable<SelectableItem<T>>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private ObservableCollection<SelectableItem<T>> _internalCollection = new ObservableCollection<SelectableItem<T>>();
        private IEnumerable<T> _source;
        private bool _defaultSelected;
        
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public SelectableCollection()
        {
            _internalCollection.CollectionChanged += (sender,e) =>
            {
                if (CollectionChanged != null)
                    CollectionChanged(this, e);
            };
        }

        public bool DefaultSelectedValue
        {
            get { return _defaultSelected; }
            set
            {
                if (_defaultSelected != value)
                {
                    _defaultSelected = value;
                    OnPropertyChanged("DefaultSelectedValue");
                }
            }
        }
        public IEnumerable<T> Source
        {
            get { return _source; }
            set
            {
                if (_source != value)
                {
                    RemoveSourceHandlers();
                    _source = value;
                    ResetInternalCollection();
                    AddSourceHandlers();
                }
            }
        }

        public IEnumerable<T> SelectedItems()
        {
            var selecteds = from item in _internalCollection
                            where item.Selected
                            select item.Item;

            return selecteds.ToArray();
        }
        public IEnumerator<SelectableItem<T>> GetEnumerator()
        {
            return _internalCollection.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void AddSourceHandlers()
        {
            INotifyCollectionChanged collection = Source as INotifyCollectionChanged;
            if (collection != null)
            {
                collection.CollectionChanged += SourceCollectionChanged;
            }
        }
        private void RemoveSourceHandlers()
        {
            INotifyCollectionChanged collection = Source as INotifyCollectionChanged;
            if (collection != null)
            {
                collection.CollectionChanged -= SourceCollectionChanged;
            }
        }
        private void ResetInternalCollection()
        {
            var items = from item in Source
                        select new SelectableItem<T>(item, DefaultSelectedValue);

            _internalCollection.Clear();
            foreach (SelectableItem<T> item in items)
            {
                _internalCollection.Add(item);
            }
        }
        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                IEnumerable<T> removedItems = e.OldItems.Cast<T>();
                
                List<T> removeList = new List<T>();
                foreach (T removedItem in removedItems)
                {
                    SelectableItem<T> removedSelectableItem = (from item in _internalCollection
                                                               where item.Item.Equals(removedItem)
                                                               select item).First();
                    _internalCollection.Remove(removedSelectableItem);
                }
            }
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                IEnumerable<T> added = e.NewItems.Cast<T>();
                foreach (T addedItem in added)
                    _internalCollection.Add(new SelectableItem<T>(addedItem));
            }
        }

        internal void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
