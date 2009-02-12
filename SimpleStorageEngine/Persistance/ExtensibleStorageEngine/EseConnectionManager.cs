using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.Persistance.ExtensibleStorageEngine {
    public class EseConnectionManager : ConnectionManager {
        public EseConnectionManager(string path) {
        }

        protected override Connection OpenConnection() {
            throw new NotImplementedException();
        }

        public override void CreateDatabase() {
            throw new NotImplementedException();
        }
    }
}
