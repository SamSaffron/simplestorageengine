using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SimpleStorageEngine.Persistance {
    public class Row {
        Dictionary<string, object> data = new Dictionary<string, object>();

        public Row SetValue(string key, object data) {
            this[key] = data; 
            return this;
        }

        public bool ContainsKey(string key) {
            return data.ContainsKey(key);
        }

        public bool TryGetValue(string key, out object o)
        {
            return data.TryGetValue(key, out o); 
        }

        public object this[string columnName]
        {
            get 
            {
                return data[columnName]; 
            }
            set 
            {
                data[columnName] = value;
            }
        }

        public override bool Equals(object obj) {
            Row other = obj as Row;
            if (other == null || other.data.Count != data.Count) return false;
            
            foreach (var item in data) {
                object val;
                if (!other.TryGetValue(item.Key, out val)) return false;
                if (val == null && item.Value == null) continue;
                if (!val.Equals(item.Value)) return false; 
            }
            return true;
        }

        public override int GetHashCode() {
            return data.GetHashCode();
        }
    }
}
