using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleStorageEngine.Persistance.ExtensibleStorageEngine;
using SimpleStorageEngine.Persistance;
using System.IO;

namespace TestSimpleStorageEngine {
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TestEse {
        public TestEse() {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //

        static string directory = "test_data";
        static string filename = Path.GetFullPath(directory + "\\test.edb");
        
        [TestInitialize()]
        public void MyTestInitialize() 
        {
            Directory.CreateDirectory(directory);
            EseConnection.CreateDatabase(filename);
            
        }
        
        [TestCleanup()]
        public void MyTestCleanup() 
        {
            Directory.Delete(directory, true);
        }
        
        #endregion

        private EseConnection GetConnection() { 
           return EseConnection.Open(filename); 
        }



        [TestMethod]
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

        [TestMethod]
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

        [TestMethod] 
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


    }
}
