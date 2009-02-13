using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Isam.Esent.Interop;

namespace SimpleStorageEngine.Persistance.ExtensibleStorageEngine {
    public class EseTransaction : ITransaction {
        #region ITransaction Members

        EseConnection connection;
        bool disposed = false;
        bool inTransaction = false;
        Transaction transaction;

        internal EseTransaction(EseConnection connection) {
            this.connection = connection;
        }

        internal void BeginTransaction() {
            transaction = new Transaction(connection.session);
            inTransaction = true;
        }

        public void Commit() {
            if (inTransaction) {
                transaction.Commit(CommitTransactionGrbit.None);
                inTransaction = false;
            } else {
                throw new EseException("An attempt was made to commit a finalized transaction!"); 
            }
        }

        public void Rollback() {
            if (inTransaction) {
                transaction.Rollback();
                inTransaction = false;
            } else {
                throw new EseException("An attempt was made to commit a finalized transaction!"); 
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {

            if (disposed) return;

            if (inTransaction) {
                Rollback();
                // log a warning that we automatically rolled back 
            }
            disposed = true;
            // the hard one is can we rollback this transaction in the GC thread ? 
            GC.SuppressFinalize(this); 
        }

        #endregion

        ~EseTransaction() {
            if (!disposed) {
                Dispose();
            }
        }
    }
}
