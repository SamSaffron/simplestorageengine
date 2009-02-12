using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using SimpleStorageEngine.Persistance.ExtensibleStorageEngine;
using SimpleStorageEngine.Persistance;
using System.IO;

namespace TestSimpleStorageEngine {
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestFixture]
    public class TestPersistance {
        public TestPersistance() {
        }


        #region Additional test attributes
       
        static string directory = "test_data";
        static string filename = Path.GetFullPath(directory + "\\test.edb");
        
        [SetUp]
        public void MyTestInitialize() 
        {
            Directory.CreateDirectory(directory);
            EseConnection.CreateDatabase(filename);
            
        }
        
        [TearDown]
        public void MyTestCleanup() 
        {
            Directory.Delete(directory, true);
        }
        
        #endregion

        private EseConnection GetConnection() { 
           return EseConnection.Open(filename); 
        }



        [Test]
        public void TestTableCreation() 
        {
            CreatePersonTable();

            using (var connection = GetConnection()) {
                Table table = connection.GetTable("person");
                var columns = table.Columns;
                columns.Sort((x,y) => x.Name.CompareTo(y.Name)); 
                Assert.AreEqual(2, columns.Count);
                Assert.AreEqual("name", table.Columns[0].Name);
                Assert.AreEqual(false, table.Columns[0].IsPrimaryKey); 
                Assert.AreEqual("ssn", table.Columns[1].Name);
                Assert.AreEqual(true, table.Columns[1].IsPrimaryKey); 
            }

        }

        private void CreatePersonTable() {
            using (var connection = GetConnection()) {
                connection.CreateTable("person", new TableDefinition()
                    .AddColumn("ssn", typeof(int), true)
                    .AddColumn("name", typeof(string))
                    );
            }
        }

        [Test]
        public void TestDataLookup() 
        {
            CreatePersonTable();

            using (var connection = GetConnection()) 
            {
                var t = connection.GetTable("person");
                var row = new Row();
                row["ssn"] = 1000;
                row["name"] = "Booboo";
                t.Insert(row); 
            }

            using (var connection = GetConnection()) {
                var t = connection.GetTable("person");
                var row = t.Get(1000);
                Assert.AreEqual(row["ssn"], 1000);
                Assert.AreEqual(row["name"], "Booboo"); 
            }
        }

        [Test] 
        public void TestDataDeletion() 
        {
            CreatePersonTable();
            using (var connection = GetConnection()) 
            {
                var t = connection.GetTable("person");
                var row = new Row();
                row["ssn"] = 1000;
                row["name"] = "Booboo";
                t.Insert(row); 
            }

            using (var connection = GetConnection()) 
            {
                var t = connection.GetTable("person");
                Assert.AreEqual(true, t.Exists(1000)); 
                t.Delete(1000);
                Assert.AreEqual(false, t.Exists(1000)); 
            }
        }

        [Test]
        public void TestTruncate() 
        {
            CreatePersonTable();
            using (var connection = GetConnection()) 
            {
                var t = connection.GetTable("person");
                
                for (int i = 0; i < 100; i++) {
                    var row = new Row();
                    row["ssn"] = i;
                    row["name"] = "Booboo";
                    t.Insert(row); 
                }
                Assert.AreEqual(100, t.Count); 
                t.Truncate();
                Assert.AreEqual(0, t.Count);
            }
        }

        [Test]
        public void TestUpsert() 
        {
            CreatePersonTable(); 
            using (var connection = GetConnection()) {

                var t = connection.GetTable("person");

                var row = new Row();
                row["ssn"] = 1;
                row["name"] = "Booboo";
                t.Upsert(row);

                Assert.AreEqual("Booboo", t.Get(1)["name"]);

                row["name"] = "bob";
                t.Upsert(row); 
                
                Assert.AreEqual("bob", t.Get(1)["name"]);
            }
        }

    }
}
