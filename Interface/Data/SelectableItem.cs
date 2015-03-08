using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceos.Interface.Data
{
    /// <summary>
    /// This class holds a boolean to represent if the Item is selected.
    /// </summary>
    public class SelectableItem<T> : INotifyPropertyChanged
    {
        private T _item;
        private bool _selected;

        public event PropertyChangedEventHandler PropertyChanged;

        public SelectableItem(T item)
        {
            _item = item;
        }
        public SelectableItem(T item, bool selected)
            : this(item)
        {
            _selected = selected;
        }

        public T Item
        {
            get { return _item; }
            set
            {
                if (!_item.Equals(value))
                {
                    _item = value;
                    OnPropertyChanged("Item");
                }
            }
        }
        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    OnPropertyChanged("Selected");
                }
            }
        }

        internal void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
