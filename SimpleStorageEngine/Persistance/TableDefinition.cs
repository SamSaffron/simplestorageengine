using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.Persistance {
    public class TableDefinition {

        List<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();
        List<string[]> indexes;

        public TableDefinition AddColumn(string columnName, Type type) 
        {
            return AddColumn(columnName, type, false);
        }

        public TableDefinition AddColumn(string columnName, Type type, bool isPrimaryKey) 
        { 
            AddColumn(new ColumnDefinition(columnName, type, isPrimaryKey)); 
            return this; 
        }

        public TableDefinition AddColumn(ColumnDefinition columnDefinition) 
        {
            columnDefinitions.Add(columnDefinition);
            return this; 
        }

        public TableDefinition AddIndex(params string[] columns) 
        {
            indexes.Add(columns);
            return this;
        }
    }
}
