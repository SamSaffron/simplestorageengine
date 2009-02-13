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
        

        #region IConnection Members

        internal EseConnection (string filename) {
            this.filename = filename;
            tableCreator = new EseTableCreator(this); 
        }


        internal void Connect() 
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
            var tran = new EseTransaction(this);
            tran.BeginTransaction();
            return tran;
        }

        public override bool InTransaction {
            get { throw new NotImplementedException(); }
        }

        public override void CreateTable(TableDefinition def) {
            tableCreator.Create(def);
        }

        public override bool TableExists(string name) {
            bool found = false;
            foreach (string tableName in Api.GetTableNames(session, dbid)) {
                found = (tableName == name);
                if (found) break;
            }
            return found;
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
