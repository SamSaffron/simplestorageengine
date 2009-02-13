using System;
using System.Collections.Generic;
using System.Text;
using SimpleStorageEngine.Persistance;
using System.Reflection;

namespace SimpleStorageEngine.ActiveRecord {
    class RowSerializer<T> where T : new() {

        private static PropertyInfo[] properties; 
        internal static PropertyInfo[] Properties {
            get 
            {
                if (properties == null) {
                    properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                }
                return properties;
            }
        }

        public RowSerializer() {
        }

        public Row ToRow(T obj) {
            Row row = new Row();
            Type type = typeof(T); 
            foreach (var property in Properties) {
                row[property.Name]= property.GetValue(obj, null); 
            }
            return row;
        }

        public T FromRow(Row row) {
            T obj = new T();
            Type type = typeof(T);
            foreach (var property in Properties) {
                property.SetValue(obj, row[property.Name],null);
            }
            return obj;
        }

    }
}
