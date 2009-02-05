using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.Persistance.ExtensibleStorageEngine {
    public class EseException : Exception
    {
      public EseException() { }
      public EseException( string message ) : base( message ) { }
      public EseException( string message, Exception inner ) : base( message, inner ) { }
      protected EseException( 
    	System.Runtime.Serialization.SerializationInfo info, 
    	System.Runtime.Serialization.StreamingContext context ) : base( info, context ) { }
    }
}
