using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.Persistance {
    
    [Flags]
    public enum ColumnProperties 
	{
	    None = 0, 
        PrimaryKey = 1,
        AutoIncrement = 2
	} 
}
