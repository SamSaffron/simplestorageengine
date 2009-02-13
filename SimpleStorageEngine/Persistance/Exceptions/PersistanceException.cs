using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace SimpleStorageEngine.Persistance.Exceptions {
    [Serializable]
    public class PersistanceException : Exception {
        public PersistanceException(string message)
            : base(message) {
        }

        public PersistanceException(string message, Exception innerException)
            : base(message, innerException) {
        }


        protected PersistanceException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }
    }
}
