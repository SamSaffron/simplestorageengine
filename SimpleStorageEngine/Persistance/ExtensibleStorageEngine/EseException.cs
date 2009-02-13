using System;
using System.Collections.Generic;
using System.Text;
using SimpleStorageEngine.Persistance.Exceptions;
using System.Runtime.Serialization;

namespace SimpleStorageEngine.Persistance.ExtensibleStorageEngine {
    public class EseException : PersistanceException {
        public EseException(string message)
            : base(message) {
        }

        public EseException(string message, Exception innerException)
            : base(message, innerException) {
        }


        protected EseException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }
    }
}
