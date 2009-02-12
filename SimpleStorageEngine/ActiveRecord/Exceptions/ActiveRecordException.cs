using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace SimpleStorageEngine.ActiveRecord {
    [Serializable]
    public class ActiveRecordException : Exception {

        public ActiveRecordException(string message)
            : base(message) {
        }

        public ActiveRecordException(string message, Exception innerException)
            : base(message, innerException) {
        }

 
        protected ActiveRecordException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }
    }
}
