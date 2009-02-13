using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Reflection;

namespace TestSqliteMergeModule {
    class Program {
        static void Main(string[] args) {

            SqliteMergeModule.AssemblyLoader.LoadSqlite();
            Test();
        }

        static void Test() {
            SQLiteConnection cnn = new SQLiteConnection("Data Source=helloworld.db");
            cnn.Open();
            var cmd = cnn.CreateCommand();
            cmd.CommandText = "select 'hello world'";
            string s = (string)cmd.ExecuteScalar();
            cnn.Close();
            Console.WriteLine(s);
            Console.ReadKey();
        }
    }
}
