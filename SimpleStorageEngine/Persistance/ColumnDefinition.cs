using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.Persistance {
    public class ColumnDefinition {

        public ColumnDefinition(string columnName, Type type, bool isPrimaryKey) {
            this.Name = columnName;
            this.Type = type; 
            this.IsPrimaryKey = isPrimaryKey;
        }

        public bool IsPrimaryKey { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
    }
}
