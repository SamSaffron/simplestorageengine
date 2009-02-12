using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.ActiveRecord {
    public class HasMany<T> where T : new() {
        public T Build()
        {
            return new T();
        }

        public int Count { get { throw new NotImplementedException();  } }
    }
}
