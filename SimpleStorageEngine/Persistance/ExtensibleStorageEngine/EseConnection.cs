using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.Persistance.ExtensibleStorageEngine {
    public class EseConnection : IConnection {

        #region IConnection Members

        private EseConnection () {}

        public static void CreateDatabase(string path) 
        {
        }

        public static EseConnection Open(string path) 
        {
            var connection = new EseConnection();
            return connection;
        }


        public void Close() {
            throw new NotImplementedException();
        }

        public ITransaction BeginTransaction() {
            throw new NotImplementedException();
        }

        public void CreateTable(string name, TableDefinition def) {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        #endregion
    }
}
