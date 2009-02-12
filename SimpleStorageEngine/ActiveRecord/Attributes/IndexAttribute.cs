using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.ActiveRecord {
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class IndexAttribute : Attribute {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string[] columns;

        // This is a positional argument
        public IndexAttribute(params string[] columns) {
            this.columns = columns;
        }

        public string[] Columns {
            get { return columns; }
        }
    }
}
