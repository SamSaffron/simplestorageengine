using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Isam.Esent.Interop;

namespace SimpleStorageEngine.Persistance.ExtensibleStorageEngine {
    internal class EseTableCreator {
        EseConnection connection;
        static Dictionary<Type, JET_COLUMNDEF> columnDefs;
 
        static EseTableCreator()
        {
            columnDefs = new Dictionary<Type, JET_COLUMNDEF>();

            columnDefs[typeof(Int32)] = Def(JET_coltyp.Long, ColumndefGrbit.ColumnNotNULL);
            columnDefs[typeof(string)] = Def(JET_coltyp.Text, ColumndefGrbit.None, 255);
        }

        static JET_COLUMNDEF Def(JET_coltyp colType, ColumndefGrbit grbit) {
            return Def(colType, grbit,-1);
        }
        static JET_COLUMNDEF Def(JET_coltyp colType, ColumndefGrbit grbit, int cbMax) 
        {
            JET_COLUMNDEF def = new JET_COLUMNDEF(); 
            def.grbit = grbit;
            def.coltyp = colType;
            if (cbMax > 0) def.cbMax = cbMax; 
            return def;
        }

        
        
        public EseTableCreator(EseConnection connection) {
            this.connection = connection;
        }

        public void Create(TableDefinition def) 
        { 
            if (def.ColumnDefinitions.FindAll(x => x.IsPrimaryKey).Count != 1) 
            {
                // TODO do we allow 0 primary keys? 
                throw new EseException("Ensure one primary key is defined for the table"); 
            }

            using (var transaction = new Transaction(connection.session)) 
            {
                JET_TABLEID tableid; 
                Api.JetCreateTable(connection.session, connection.dbid, def.TableName, 16, 100, out tableid);

                foreach (var column in def.ColumnDefinitions) {

                    AddColumn(tableid, column);
                }

                foreach (var item in def.Indexes) {
                    // all indexes are ascending
                    var indexName = string.Join("|", item.ToArray());
                    var indexDef = "+" + string.Join("\0+", item.ToArray()) + "\0\0";
                    Api.JetCreateIndex(connection.session, tableid, indexName, CreateIndexGrbit.IndexSortNullsHigh, indexDef, indexDef.Length, 100);
                }

                transaction.Commit(CommitTransactionGrbit.LazyFlush); 
            }
        }

        public void AddColumn(JET_TABLEID tableid, ColumnDefinition column) {
            
            JET_COLUMNDEF column_def;
            JET_COLUMNID column_id;

            if (columnDefs.ContainsKey(column.Type)) {
                column_def = columnDefs[column.Type];
            } else {
                column_def = new JET_COLUMNDEF();
                column_def.coltyp = JET_coltyp.LongBinary;
            }

            // TODO validate only one of these
            if (column.IsAutoIncrement) {
                column_def.grbit = column_def.grbit | ColumndefGrbit.ColumnAutoincrement;
            }

            Api.JetAddColumn(connection.session, tableid, column.Name, column_def, null, 0, out column_id);

            if (column.IsPrimaryKey) {
                var indexDef = "+" + column.Name + "\0\0";
                Api.JetCreateIndex(connection.session, tableid, "primary", CreateIndexGrbit.IndexPrimary, indexDef, indexDef.Length, 100);
            }
        }
    }
}
