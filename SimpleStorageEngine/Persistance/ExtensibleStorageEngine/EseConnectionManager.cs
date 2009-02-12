using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Isam.Esent.Interop;

namespace SimpleStorageEngine.Persistance.ExtensibleStorageEngine {
    public class EseConnectionManager : ConnectionManager {

        string filename; 

        public EseConnectionManager(string filename) {
            this.filename = filename; 
        }

        protected override Connection OpenConnection() {

            try {
                var connection = new EseConnection(filename);
                connection.Connect();
                return connection;
            } catch {
                // TODO: wrap database is corrupt or does not exist. 
                throw;
            }
        }

        public override void CreateDatabase() {
            try {
                using (Instance instance = new Instance("newdb")) {
                    instance.Init();
                    using (Session session = new Session(instance)) {
                        JET_DBID dbid;
                        Api.JetCreateDatabase(session, filename, null, out dbid, CreateDatabaseGrbit.None);
                    }
                }
            } catch {
                // TODO: Wrap database already exists exception
                throw;
            }
        }
    }
}
