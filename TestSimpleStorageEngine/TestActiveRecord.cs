using System;
using System.Text;
using System.Collections.Generic;

using NUnit.Framework;
using SimpleStorageEngine.Persistance;
using SimpleStorageEngine.Persistance.ExtensibleStorageEngine;
using SimpleStorageEngine.ActiveRecord;
using System.IO;

namespace TestSimpleStorageEngine {

    // first param is the class, second is the primary key
    public class Order : ActiveRecord<Order,int> {
        BelongsTo<Customer> Customer { get; set; }
        [PrimaryKey(AutoIncrement=true)]
        public int Id { get; set; }
        public string Details { get; set; }
    } 


    [Index("FirstName", "LastName")]
    [Index("LastName", "FirstName")] 
    public class Customer : ActiveRecord<Customer,int> 
    {

        public HasMany<Order> Orders { get; set; }

        [PrimaryKey(AutoIncrement=true)]
        public int Id { get; set; }

        [ColumnInfo(MinLength=4, MaxLength=255, Nullable=false)]
        public string FirstName { get; set; }

        [ColumnInfo(MinLength=4, MaxLength=255, Nullable=false)] 
        public string LastName { get; set; }

        public string Comments { get; set; }
    }

    [TestFixture]
    public class TestActiveRecord {

        public void Demo() 
        {
            
            var customer = Customer.Build();
            customer.FirstName = "bob";
            customer.LastName = "doe";

            var order = customer.Orders.Build();
            order.Details = "This is the first order"; 
            customer.Save();

            var customer2 = Customer.Find(customer.Id);

            Assert.AreEqual(1, customer2.Orders.Count); 

        }


        #region Additional test attributes

        static string directory = "test_data";
        static string filename = Path.GetFullPath(directory + "\\test.edb");
        static ConnectionManager connectionManager;


        [TestFixtureSetUp]
        public static void MyClassInitialize() {
            connectionManager = new EseConnectionManager("c:\\temp\\db");
            ActiveRecordSettings.ConnectionManager = connectionManager;
        }

        
        #endregion

        [Test]
        public void TestMethod1() {
            //
            // TODO: Add test logic	here
            //
        }
    }
}
