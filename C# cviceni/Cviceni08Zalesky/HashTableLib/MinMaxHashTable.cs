using System;
using System.Collections.Generic;
using System.Linq;

namespace HashTableLib
{
    public class MinMaxHashTable<K, V> where K : IComparable<K>
    {
        public class Cell
        {
            public K Key { get; set; }
            public V Value { get; set; }
        }
        public List<Cell>[] Table { get; set; }
        private bool MinKeySet { get; set; }
        private K _minKey;
        public K MinKey
        {
            get
            {
                if (MinKeySet)
                    return _minKey;
                throw new InvalidOperationException("MinKey not initialized");
            }
            private set {
                _minKey = value;
                MinKeySet = true; }
        }
        private bool MaxKeySet { get; set; }
        private K _maxKey;
        public K MaxKey
        {
            get
            {
                if (MaxKeySet)
                    return _maxKey;
                throw new InvalidOperationException("MinKey not initialized");
            }
            private set { _maxKey = value; MaxKeySet = true; }
        }
        private const int DEFAULT_SIZE = 20;
        public int Count
        {
            get { return Table.Sum(x => x.Count); }
            private set { Count = value; }
        }

        public IEnumerable<Cell> this[K min, K max]
        {
            get
            {
                return Range(min, max);
            }
        }

        public MinMaxHashTable()
        {
            Table = new List<Cell>[DEFAULT_SIZE];
            MinKeySet = false;
            MaxKeySet = false;
            for (int i = 0; i < DEFAULT_SIZE; i++)
            {
                Table[i] = new List<Cell>();
            }
        }

        public MinMaxHashTable(int initSize)
        {
            Table = new List<Cell>[initSize];
            MinKeySet = false;
            MaxKeySet = false;
            for (int i = 0; i < initSize; i++)
            {
                Table[i] = new List<Cell>();
            }
        }

        public IEnumerable<Cell> Range(K min, K max)
        {
            return Table.Select(x => x.Where(y => y.Key.GetHashCode() >= min.GetHashCode() && y.Key.GetHashCode() <= max.GetHashCode())).SelectMany(t => t);
        }

        public IEnumerable<Cell> SortedRange(K min, K max)
        {
            var range = Range(min, max);
            return range.OrderBy(x => x.Key);
        }

        public void Add(K key, V value)
        {
            if (Contains(key))
                throw new ArgumentException("Key already allocated");
            CheckMinMax(key);
            Table[GetHash(key)].Add(new Cell { Key = key, Value = value });
        }

        public V Get(K key)
        {
            if (!Contains(key))
                throw new KeyNotFoundException("Key not found");
            foreach (var item in Table)
            {
                var search = item.Find(x => x.Key.Equals(key));
                if (search != null)
                    return search.Value;
            }
            return default(V);
        }

        public V Remove(K key)
        {
            foreach (var item in Table)
            {
                var search = item.Find(x => x.Key.Equals(key));
                if (search != null)
                {
                    item.Remove(search);
                    return search.Value;
                }
            }
            throw new KeyNotFoundException("Key not found");
        }

        private void CheckMinMax(K key)
        {
            var hash = key.GetHashCode();
            if (!MinKeySet)
                MinKey = key;
            if (!MaxKeySet)
                MaxKey = key;
            if (hash < MinKey.GetHashCode())
                MinKey = key;
            if (hash > MaxKey.GetHashCode())
                MaxKey = key;
        }

        private int GetHash(K key)
        {
            return Math.Abs(key.GetHashCode() % Table.Length);
        }
        public bool Contains(K key)
        {
            foreach (var field in Table)
            {
                if (field.Find(x => x.Key.Equals(key)) != null)
                    return true;
            }
            return false;
        }
    }
}
