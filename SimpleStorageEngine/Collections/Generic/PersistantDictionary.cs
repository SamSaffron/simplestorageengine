using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.Collections.Generic {
    public class PersistantDictionary<TKey, TValue> : IDictionary<TKey, TValue> {

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value) {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key) {
            throw new NotImplementedException();
        }

        public ICollection<TKey> Keys {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(TKey key) {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value) {
            throw new NotImplementedException();
        }

        public ICollection<TValue> Values {
            get { throw new NotImplementedException(); }
        }

        public TValue this[TKey key] {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        public void Add(KeyValuePair<TKey, TValue> item) {
            throw new NotImplementedException();
        }

        public void Clear() {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public int Count {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }

        #endregion
    }
}
