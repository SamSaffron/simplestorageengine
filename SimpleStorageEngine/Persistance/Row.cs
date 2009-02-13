using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SimpleStorageEngine.Persistance {
    public class Row : Dictionary<string, object> {

        public override bool Equals(object obj) {
            Row other = obj as Row;
            if (other == null || other.Count != Count) return false;
            
            foreach (var item in this) {
                object val;
                if (!other.TryGetValue(item.Key, out val)) return false;
                if (val == null && item.Value == null) continue;
                if (!val.Equals(item.Value)) return false; 
            }
            return true;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public Row SetValue(string key, object value) {
            this[key] = value;
            return this;
        }
    }
}
