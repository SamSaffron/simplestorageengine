using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageEngine.Persistance {
     public abstract class Table {
        public abstract void Insert(Row o);
        public abstract Row Get(object key);
        public abstract bool Exists(object key); 
        public abstract void Delete(object key);
        public abstract void Upsert(Row row);
        public abstract void Truncate();

        public abstract List<ColumnDefinition> Columns { get;}

        //IEnumerable<TObject> Find(SearchCriteria criteria); 
    }
}
