using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.Persistance {
    public class ColumnDefinition {

        public ColumnDefinition(string columnName, Type type) :
            this(columnName, type, ColumnProperties.None) {
        }

        public ColumnDefinition(string columnName, Type type, ColumnProperties flags) {
            this.Name = columnName;
            this.Type = type;
            this.ColumnProperties = ColumnProperties;
        }

 
        public string Name { get; set; }
        public Type Type { get; set; }
        public ColumnProperties ColumnProperties { get; set; }

        public bool IsPrimaryKey { 
            get { 
                return ((ColumnProperties & ColumnProperties.PrimaryKey) == ColumnProperties.PrimaryKey); 
            } 
        }
    }

}
