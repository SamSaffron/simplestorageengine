using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleStorageEngine.Persistance.ExtensibleStorageEngine;
using SimpleStorageEngine.Persistance;

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

        EseConnection connection; 
        [TestInitialize()]
        public void MyTestInitialize() 
        {
            EseConnection.CreateDatabase("testdb");
            connection = EseConnection.Open("testdb");
        }
        
        [TestCleanup()]
        public void MyTestCleanup() 
        {
            connection.Close(); 
        }
        
        #endregion

        [TestMethod]
        public void TestTableCreation() 
        {
            connection.CreateTable("person", new TableDefinition()
                .AddColumn("ssn", typeof(int), true)
                .AddColumn("name", typeof(string))
                );


            Table table = connection.GetTable("person");
            Assert.AreEqual(2, table.Columns);

        }
    }
}
