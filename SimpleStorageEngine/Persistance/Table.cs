using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SimpleStorageEngine.Persistance {
    public abstract class Table : IDisposable {


        // Helper methods
        protected byte[] ToBytes(object obj) {

            if (IsSimpleType(obj)) 
            {
                return BitConverterBytes(obj); 
            }

            using (MemoryStream ms = new MemoryStream()) {
                BinaryFormatter bif = new BinaryFormatter();
                bif.Serialize(ms, obj);
                byte[] array = ms.ToArray();
                return array;
            }
        }

        private byte[] BitConverterBytes(object obj) {

            Type type = obj.GetType(); 

            if (type == typeof(int)) return BitConverter.GetBytes((int)obj); 
            if (type == typeof(float)) return BitConverter.GetBytes((float)obj); 
            if (type == typeof(long)) return BitConverter.GetBytes((long)obj);
            if (type == typeof(short)) return BitConverter.GetBytes((short)obj);

            throw new NotImplementedException("Bit converter encountered an unexpected type"); 
        }

        private bool IsSimpleType(object obj) {
            return obj is int || obj is long || obj is float || obj is short; 
        }

        protected TObject FromBytes<TObject>(byte[] raw) {

            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream(raw)) {
                object o = bf.Deserialize(ms);
                return (TObject)o;
            }
        }

        public abstract void Insert(Row o);
        public abstract Row Get(object key);
        public abstract IEnumerable<Row> GetRows();
        public abstract IEnumerable<Row> GetRows(Row indexValue); 
        public abstract bool Exists(object key);
        public abstract void Delete(object key);
        public abstract void Upsert(Row row);
        public abstract void Truncate();
        public abstract int Count { get; }

        public abstract List<ColumnDefinition> Columns { get; }

        //IEnumerable<TObject> Find(SearchCriteria criteria); 

        #region IDisposable Members

        public abstract void Dispose();

        #endregion
    }
}
