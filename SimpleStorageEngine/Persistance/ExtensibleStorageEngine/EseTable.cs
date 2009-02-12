using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Isam.Esent.Interop;

namespace SimpleStorageEngine.Persistance.ExtensibleStorageEngine {
    class EseTable : Table {

        EseConnection connection;
        string name;
        List<ColumnInfo> columnInfos;
        string primaryKeyColumn; 

        internal EseTable(EseConnection connection, string name) {
            this.connection = connection;
            this.name = name;
            columnInfos = new List<ColumnInfo>();
            foreach (var ci in Api.GetTableColumns(connection.session, connection.dbid, name)) 
            {
                columnInfos.Add(ci); 
            }
            
            foreach (var index in Api.GetTableIndexes(connection.session, connection.dbid, name))
            {
                if ((index.Grbit & CreateIndexGrbit.IndexPrimary) > 0) 
                {
                        primaryKeyColumn = index.IndexSegments[0].ColumnName;
                }
            }
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
                    var bytes = Api.RetrieveColumn(connection.session, table, column.Columnid);
                    // we need some special handling.
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
        
        public override int Count {
            get 
            {
                using (var table = new Microsoft.Isam.Esent.Interop.Table(connection.session, connection.dbid, name, OpenTableGrbit.None)) {
                    Api.JetSetCurrentIndex(connection.session, table, null);
                    int rval; 
                    Api.JetIndexRecordCount(connection.session, table, out rval, 0);
                    return rval;
                }
            }
        }
        public override void Upsert(Row row) {
            using (var transaction = new Transaction(connection.session)) 
            {
                // ESE runs in snapshot isloation and allows nested transaction 
                if (Exists(row[primaryKeyColumn])) {
                    Delete(row[primaryKeyColumn]);
                }
                Insert(row); 
                transaction.Commit(CommitTransactionGrbit.None); 
            }
        }

        public override void Truncate() {
            using (var transaction = new Transaction(connection.session))
            using (var table = new Microsoft.Isam.Esent.Interop.Table(connection.session, connection.dbid, name, OpenTableGrbit.None)) {
                Api.MoveBeforeFirst(connection.session, table);
                while (Api.TryMoveNext(connection.session, table)) 
                {
                    Api.JetDelete(connection.session, table); 
                }
                transaction.Commit(CommitTransactionGrbit.None);
            }
        }

        public override List<ColumnDefinition> Columns {
            get 
            {
                var rval = new List<ColumnDefinition> (); 
                foreach (var column_info in columnInfos)
                {
                    var def = new ColumnDefinition(column_info.Name, typeof(object), false);
                    def.IsPrimaryKey = def.Name == primaryKeyColumn; 
                    rval.Add(def); 
                }
                return rval;
            }
        }

        

        public override void Dispose() {
        }        
    }
}
