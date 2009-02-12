using System;
using System.Collections.Generic;
using System.Text;
using SimpleStorageEngine.Persistance;
using System.Reflection;

namespace SimpleStorageEngine.ActiveRecord {
    class RowSerializer<T> where T : new() {


        public RowSerializer() {
        }

        public Row ToRow(T obj) {
            Row row = new Row();
            Type type = typeof(T); 
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
                row[property.Name]= property.GetValue(obj, null); 
            }
            return row;
        }

        public T FromRow(Row row) {
            T obj = new T();
            Type type = typeof(T);
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
                property.SetValue(obj, row[property.Name],null);
            }
            return obj;
        }

    }
}
