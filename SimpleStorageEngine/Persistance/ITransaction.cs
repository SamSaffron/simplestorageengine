using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.Persistance {
    public interface ITransaction : IDisposable {
        void Commit();
        void Rollback();
    }
}
