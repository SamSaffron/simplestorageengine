using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Messaging;

namespace SimpleStorageEngine.Persistance {
    public abstract class ConnectionManager {
        
        private const string ConnectionSlot = "SimpleStorageConnection";

        Connection GetConnection() {
            Connection connection = CallContext.GetData(ConnectionSlot) as Connection;
            if (connection == null)
            {
                connection = OpenConnection();
                connection.OnClosed += new EventHandler(connection_OnClosed);
                CallContext.SetData(ConnectionSlot, connection); 
            }
            return connection;
        }

        void connection_OnClosed(object sender, EventArgs args) {
            CallContext.SetData(ConnectionSlot, null); 
        }

        public abstract void CreateDatabase(); 
        protected abstract Connection OpenConnection();
    }
}
