using System;
using System.Collections;
using System.Collections.Generic;

namespace Catodo.Core {
    public class TodoList: IEnumerable<string>, ICollection<string> {
        public TodoList()
        {
            this.list = new List<string>();
        }

        public void Add(string item)
        {
            this.list.Add( item );
        }

        public void Clear()
        {
            this.list.Clear();
        }

        public bool Contains(string item)
        {
            return this.list.Contains( item );
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            this.list.CopyTo( array, arrayIndex );
        }

        public bool Remove(string item)
        {
            return this.list.Remove( item );
        }

        public void RemoveAt(int i)
        {
            this.list.RemoveAt( i );
        }

        public int Count {
            get { return this.list.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public void MoveUp(int i)
        {
            if ( i > 0 ) {
                string swap = this.list[ i - 1 ];
                this.list[ i - 1 ] = this.list[ i ];
                this.list[ i ] = swap;
            }
        }

        public void MoveDown(int i)
        {
            if ( i < ( this.Count - 1 ) ) {
                string swap = this.list[ i + 1 ];
                this.list[ i + 1 ] = this.list[ i ];
                this.list[ i ] = swap;
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        private List<string> list;
    }
}

