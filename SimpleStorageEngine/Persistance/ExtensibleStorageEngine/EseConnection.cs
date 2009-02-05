using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Isam.Esent.Interop;

namespace SimpleStorageEngine.Persistance.ExtensibleStorageEngine {
    public class EseConnection : IConnection {

        Instance instance;
        Session session; 
        string filename;
        JET_DBID dbid;
        bool disposed = false;
        

        #region IConnection Members

        private EseConnection (string filename) {
            this.filename = filename;
        }


        public static void CreateDatabase(string filename) 
        {
            try {
                using (Instance instance = new Instance("newdb")) {
                    instance.Init();
                    using (Session session = new Session(instance)) {
                        JET_DBID dbid;
                        Api.JetCreateDatabase(session, filename, null, out dbid, CreateDatabaseGrbit.None);
                    }
                }
            } 
            catch 
            {
                // TODO: Wrap database already exists exception
                throw;            
            }

        }

        public static EseConnection Open(string filename) 
        {
            try {
                var connection = new EseConnection(filename);
                connection.Connect();
                return connection;
            } catch 
            {
                // TODO: wrap database is corrupt or does not exist. 
                throw;
            }
        }

        private void Connect() 
        {
            instance = new Instance("connection");
            instance.Parameters.CircularLog = true;
            instance.Init();
            session = new Session(instance);
            Api.JetAttachDatabase(session, filename, AttachDatabaseGrbit.None);
            Api.JetOpenDatabase(session, filename, null, out dbid, OpenDatabaseGrbit.None); 
        }


        public void Close() {

            if (disposed) return; 

            // Ensure all data is flushed 
            using (var transaction = new Transaction(session)) 
            {
                transaction.Commit(CommitTransactionGrbit.WaitLastLevel0Commit); 
            }

            // TODO : exception out if there is an in progress transaction 
            session.Dispose();
            instance.Dispose();
        }

        public ITransaction BeginTransaction() {
            throw new NotImplementedException();
        }

        public void CreateTable(string name, TableDefinition def) {
            using (var transaction = new Transaction(session)) 
            {
                JET_TABLEID tableid; 
                Api.JetCreateTable(session, dbid, name, 16, 100, out tableid);

                foreach (var column in def.ColumnDefinitions) {

                    JET_COLUMNDEF column_def = new JET_COLUMNDEF(); 
                    JET_COLUMNID column_id;

                    column_def.coltyp = JET_coltyp.LongBinary;
                    Api.JetAddColumn(session, tableid, column.Name, column_def, null, 0, out column_id); 
                }

                transaction.Commit(CommitTransactionGrbit.LazyFlush); 
            }
        }

        public void DropTable(string name) {
            throw new NotImplementedException();
        }

        public Table GetTable(string name) {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {
            // TODO: make this multithreaded ... 
            // This is tricky cause we may need to reattach the DB 

            if (disposed) return; 

            Close(); 
            disposed = true;
            GC.SuppressFinalize(this); 
        }

        #endregion

        ~EseConnection() 
        {
            try {
                Dispose();
            } catch 
            {
                // LOG
                // Don't crash a finalizer thread. 
            }
        }
    }
}
