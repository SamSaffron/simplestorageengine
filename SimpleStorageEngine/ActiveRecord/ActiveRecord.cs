using System;
using System.Collections.Generic;
using System.Text;
using SimpleStorageEngine.Persistance;
using System.Reflection;

namespace SimpleStorageEngine.ActiveRecord {
    public class ActiveRecord <TClass,TKey>  where TClass : class, new()  {

        private static RowSerializer<TClass> rowSerializer = new RowSerializer<TClass>();

        private static TableDefinition tableDefinition;
        protected static TableDefinition TableDefinition {
            get {
                if (tableDefinition == null) {
                    PopulateTableDefinition(); 
                }
                return tableDefinition;
            }
        }



        public static string TableName {
            get {
                return typeof(TClass).Name;
            }
        }

        private static void PopulateTableDefinition()
        {
            tableDefinition = new TableDefinition(TableName);

            foreach (var property in RowSerializer<TClass>.Properties) {

                ColumnProperties properties = ColumnProperties.None;

                bool isPrimaryKey = false;
                foreach (Attribute attribute in property.GetCustomAttributes(false))
                {
                    PrimaryKeyAttribute primaryKeyAttrib = attribute as PrimaryKeyAttribute;
                    if (primaryKeyAttrib != null) {
                        properties |= ColumnProperties.PrimaryKey;
                        if (primaryKeyAttrib.AutoIncrement) {
                            properties |= ColumnProperties.AutoIncrement;
                        }
                        break;
                    }
                }

                tableDefinition.AddColumn(new ColumnDefinition(property.Name, property.PropertyType, properties)); 
            }
        }

        public static TClass Build() {
            return new TClass(); 
        }

        public static TClass Find(TKey key) {
            using (var connection = ActiveRecordSettings.ConnectionManager.GetConnection())
            using (var table = connection.GetTable(TableName)) {
                var row = table.Get(key);
                return rowSerializer.FromRow(row);
            }
        }

        public static void Migrate() {
            Migrate(true); 
        }

        public static void Migrate(bool keepExistingData) 
        {
            using (var connection = ActiveRecordSettings.ConnectionManager.GetConnection()) {
                if (connection.TableExists(TableName)) {
                    throw new NotImplementedException();
                } 
                else {
                    connection.CreateTable(TableDefinition);
                }
            }
        }

        public void Save()
        {
            using (var connection = ActiveRecordSettings.ConnectionManager.GetConnection()) 
            using (var table = connection.GetTable(TableName))
            {
                table.Insert(rowSerializer.ToRow(this as TClass));    
            }
        }
    }

    public class ActiveRecord<TClass> : ActiveRecord<TClass, int> 
        where TClass : class, new()
    {
        [PrimaryKey(AutoIncrement = true)]
        public int Id { get; set; }
    }
}
