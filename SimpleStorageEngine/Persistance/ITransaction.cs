using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.Persistance {
    public interface ITransaction {
        void Commit();
        void Rollback();
    }
}
