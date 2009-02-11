using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Isam.Esent.Interop;

namespace SimpleStorageEngine.Persistance.ExtensibleStorageEngine {
    class EseTable : Table {

        EseConnection connection;
        string name;
        IEnumerable<ColumnInfo> columnInfos; 

        internal EseTable(EseConnection connection, string name) {
            this.connection = connection;
            this.name = name;
            columnInfos = Api.GetTableColumns(connection.session, connection.dbid, name);
        }

        public override void Insert(Row o) {
            using (var transaction = new Transaction(connection.session)) 
            using (var table = new Microsoft.Isam.Esent.Interop.Table(connection.session, connection.dbid, name, OpenTableGrbit.None))
            using (var update = new Update(connection.session, table, JET_prep.Insert))
            {
                foreach (var column in columnInfos) {
                    Api.SetColumn(connection.session, table, column.Columnid, ToBytes(o[column.Name])); 
                }
                update.Save(); 
                transaction.Commit(CommitTransactionGrbit.None); 
            }
        }

        public override Row Get(object key) {
            using (var table = new Microsoft.Isam.Esent.Interop.Table(connection.session, connection.dbid, name, OpenTableGrbit.None))
            {
                Row row = new Row();
                Api.JetSetCurrentIndex(connection.session, table, null);
                Api.MakeKey(connection.session, table, ToBytes(key), MakeKeyGrbit.NewKey);
                Api.JetSeek(connection.session, table, SeekGrbit.SeekEQ);
                foreach (var column in columnInfos) {

                    // we need some special handling.

                    var bytes = Api.RetrieveColumn(connection.session, table, column.Columnid);

                    if (column.Coltyp == JET_coltyp.Long) {
                        row[column.Name] = BitConverter.ToInt32(bytes, 0);
                    } else {
                        row[column.Name] = FromBytes<object>(bytes);
                    }
                }

                return row;
            }
            // TODO: error wrapping
        }

        public override bool Exists(object key) {
            using (var table = new Microsoft.Isam.Esent.Interop.Table(connection.session, connection.dbid, name, OpenTableGrbit.None)) {
                Row row = new Row();
                Api.JetSetCurrentIndex(connection.session, table, null);
                Api.MakeKey(connection.session, table, ToBytes(key), MakeKeyGrbit.NewKey);
                return Api.TrySeek(connection.session, table, SeekGrbit.SeekEQ);
            }
        }

        public override void Delete(object key) {
            using (var transaction = new Transaction(connection.session)) 
            using (var table = new Microsoft.Isam.Esent.Interop.Table(connection.session, connection.dbid, name, OpenTableGrbit.None)) {
                Row row = new Row();
                Api.JetSetCurrentIndex(connection.session, table, null);
                Api.MakeKey(connection.session, table, ToBytes(key), MakeKeyGrbit.NewKey);
                Api.JetSeek(connection.session, table, SeekGrbit.SeekEQ);
                Api.JetDelete(connection.session, table);
                transaction.Commit(CommitTransactionGrbit.None);
            }
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
                foreach (var column_info in columnInfos)
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
