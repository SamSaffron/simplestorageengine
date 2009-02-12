using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.ActiveRecord {
    public class ActiveRecord <TClass,TKey>  where TClass : new() {

        public static TClass Build() 
        {
            return new TClass(); 
        }

        public static TClass Find(TKey key) {
            return new TClass();
        }

        public void Save()
        {
        }
    }
}
