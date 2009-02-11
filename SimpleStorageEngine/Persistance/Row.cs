using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SimpleStorageEngine.Persistance {
    public class Row {
        Dictionary<string, object> data = new Dictionary<string, object>(); 

        public object this[string columnName]
        {
            get 
            {
                return data[columnName]; 
            }
            set 
            {
                data[columnName] = value;
            }
        }
    }
}
