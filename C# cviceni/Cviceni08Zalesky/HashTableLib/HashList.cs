using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace HashTableLib
{
    public class HashList<K, V>
    {
        public class Cell
        {
            public K Key { get; set; }
            public V Value { get; set; }
            public Cell Next { get; set; }
            public Cell Previous { get; set; }



            public Cell(K key, V value)
            {
                Key = key;
                Value = value;
                Previous = null;
                Next = null;
            }

            // override object.Equals
            public override bool Equals(object obj)
            {
                //       
                // See the full list of guidelines at
                //   http://go.microsoft.com/fwlink/?LinkID=85237  
                // and also the guidance for operator== at
                //   http://go.microsoft.com/fwlink/?LinkId=85238
                //

                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                // TODO: write your implementation of Equals() here
                Cell cell = (Cell)obj;
                if (cell.Key.Equals(this.Key) && cell.Value.Equals(this.Value))
                    return true;
                return false;
            }

            public override int GetHashCode()
            {
                return Key.GetHashCode();
            }
        }
        public Cell First { get; set; }
        public Cell Last { get; set; }

        public void Add(K key, V value)
        {
            Cell cell = new Cell(key, value);
            if (First == null)
            {
                First = cell;
                Last = cell;
            }
            else
            {
                Last.Next = cell;
                cell.Previous = Last;
                Last = cell;

            }
        }

        public Cell Remove(Cell cell)
        {
            Cell current = First;
            if (current.Equals(First)) {
                First = First.Next;
                return current;
            }
            do
            {
                if (current.Equals(cell))
                {
                    current.Previous.Next = current.Next;
                    current.Next.Previous = current.Previous;
                    return current;
                }
            } while (current.Next != null);
            return null;
        }

        
    }
}
