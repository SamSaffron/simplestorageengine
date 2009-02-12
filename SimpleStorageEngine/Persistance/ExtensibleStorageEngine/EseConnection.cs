using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Isam.Esent.Interop;

namespace SimpleStorageEngine.Persistance.ExtensibleStorageEngine {
    public class EseConnection : Connection {

        internal Instance instance;
        internal Session session;
        internal EseTableCreator tableCreator;

        string filename;
        internal JET_DBID dbid;
        bool disposed = false;
        

        #region IConnection Members

        private EseConnection (string filename) {
            this.filename = filename;
            tableCreator = new EseTableCreator(this); 
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


        public override void Close() {

            if (disposed) return; 

            // Ensure all data is flushed 
            using (var transaction = new Transaction(session)) 
            {
                transaction.Commit(CommitTransactionGrbit.WaitLastLevel0Commit); 
            }

            // TODO : exception out if there is an in progress transaction 
            session.Dispose();
            instance.Dispose();

            base.Close();
        }

        public override ITransaction BeginTransaction() {
            throw new NotImplementedException();
        }

        public override bool InTransaction {
            get { throw new NotImplementedException(); }
        }

        public override void CreateTable(string name, TableDefinition def) {
            tableCreator.Create(name, def);
        }

        public override void DropTable(string name) {
            throw new NotImplementedException();
        }

        public override Table GetTable(string name) {
            return new EseTable(this, name); 
        }

        #endregion


        
    }
}
