using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.ActiveRecord {
    [global::System.AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ColumnInfoAttribute : Attribute {

        public ColumnInfoAttribute() { 
        } 

        // This is a positional argument
        public ColumnInfoAttribute(int minLength, int maxLength, bool nullable) {
            this.Nullable = nullable;
            this.MaxLength = maxLength;
            this.MinLength = minLength; 
        }

        // This is a named argument
        public bool Nullable { get; set; }
        public int MaxLength { get; set; }
        public int MinLength { get; set; } 
    }
}
