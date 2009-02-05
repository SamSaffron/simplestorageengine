using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Isam.Esent.Interop;

namespace SimpleStorageEngine.Persistance.ExtensibleStorageEngine {
    class EseTable : Table {

        EseConnection connection;
        string name; 

        internal EseTable(EseConnection connection, string name) {
            this.connection = connection;
            this.name = name;
            // validate that table exists ? 
        }

        public override void Insert(Row o) {
            throw new NotImplementedException();
        }

        public override Row Get(object key) {
            throw new NotImplementedException();
        }

        public override bool Exists(object key) {
            throw new NotImplementedException();
        }

        public override void Delete(object key) {
            throw new NotImplementedException();
        }

        public override void Upsert(Row row) {
            throw new NotImplementedException();
        }

        public override void Truncate() {
            throw new NotImplementedException();
        }

        public override List<ColumnDefinition> Columns {
            get 
            {
                
                string primaryKey = ""; 

                foreach (var index in Api.GetTableIndexes(connection.session, connection.dbid, name))
                {
                    if ((index.Grbit & CreateIndexGrbit.IndexPrimary) > 0) 
                    {
                        primaryKey = index.IndexSegments[0].ColumnName;
                    }
            	}

                var rval = new List<ColumnDefinition> (); 
                foreach (var column_info in Api.GetTableColumns(connection.session, connection.dbid, name))
                {
                    var def = new ColumnDefinition(column_info.Name, typeof(object), false);
                    def.IsPrimaryKey = def.Name == primaryKey; 
                    rval.Add(def); 
                }
                return rval;
            }
        }

        public override void Dispose() {
        }        
    }
}
