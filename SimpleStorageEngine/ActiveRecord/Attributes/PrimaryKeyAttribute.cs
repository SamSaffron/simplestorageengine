using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.ActiveRecord {
    [global::System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class PrimaryKeyAttribute : Attribute {
        public PrimaryKeyAttribute() {
        }

        public bool AutoIncrement { get; set; }
    }
}
