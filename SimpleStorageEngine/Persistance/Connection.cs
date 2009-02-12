using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.Persistance {
    public abstract class Connection : IDisposable {

        protected bool disposed;
        public event EventHandler OnClosed;

        public virtual void Close() {
            if (OnClosed!=null) {
                OnClosed(this, null); 
            }
        }

        public abstract ITransaction BeginTransaction();
        public abstract bool InTransaction { get; }

        public abstract bool TableExists(string name); 
        public abstract void CreateTable(TableDefinition def);
        public abstract void DropTable(string name);
        public abstract Table GetTable(string name);

        public virtual void Dispose() {
            // TODO: make this multithreaded ... 
            // This is tricky cause we may need to reattach the DB 

            if (disposed) return;

            Close();
            disposed = true;
            GC.SuppressFinalize(this); 
        }

        ~Connection() 
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
