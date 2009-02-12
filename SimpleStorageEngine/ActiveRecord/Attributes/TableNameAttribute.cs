using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.ActiveRecord {

    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TableNameAttribute : Attribute {
        public TableNameAttribute(string name) {
            this.Name = name;
        }

        public string Name { get; private set; }

    }
}
