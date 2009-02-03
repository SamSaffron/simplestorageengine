using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.Persistance {
    interface IConnection : IDisposable {
        
        void Close();

        ITransaction BeginTransaction(); 

        void CreateTable(string name, TableDefinition def);
        void DropTable(string name);
        Table GetTable(string name); 

    }

}
