using System;
using System.Collections;
using System.Collections.Generic;

namespace Glitch9.Collections
{
    public class EnumSelection<T> : ICollection<T> where T : Enum
    {
        private readonly List<T> _items;
        public int Count => _items.Count;
        public bool IsReadOnly => false;
        public bool None => _items.Count == 0;
        public bool All => _items.Count == _count;

        private readonly int _count;

        public EnumSelection()
        {
            _items = new();
            _count = Enum.GetValues(typeof(T)).Length;
            SelectAll();
        }
        
        public void SelectAll()
        {
            _items.Clear();
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                _items.Add(value);
            }
        }

        public void SelectNone()
        {
            _items.Clear();
        }

        public void Select(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _items.Add(item);
        }

        public void Deselect(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _items.Remove(item);
        }

        public bool IsSelected(T item)
        {
            return _items.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return _items.Remove(item);
        }
    }
}