using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.Persistance {
    public class TableDefinition {

        
        public TableDefinition() {
            ColumnDefinitions = new List<ColumnDefinition>();
            Indexes = new List<IndexDefinition>();
        }

        public List<ColumnDefinition> ColumnDefinitions { get; private set; }
        public List<IndexDefinition> Indexes { get; private set; }

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
            ColumnDefinitions.Add(columnDefinition);
            return this; 
        }

        public TableDefinition AddIndex(params string[] columns) 
        {
            var def = new IndexDefinition();
            def.AddRange(columns); 
            Indexes.Add(def);
            return this;
        }
    }
}
