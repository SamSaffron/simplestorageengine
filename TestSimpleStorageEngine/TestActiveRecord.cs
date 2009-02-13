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

    [TableName("person")]
    public class Person : ActiveRecord<Person> {

        public string Name { get; set; }
    }

    [TableName("person")]
    public class Person2 : ActiveRecord<Person2> {

        public string Name { get; set; }
        public string LastName { get; set; }
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


        [SetUp]
        public void Initialize() {
            Directory.CreateDirectory(directory);

            connectionManager = new EseConnectionManager(filename);
            connectionManager.CreateDatabase();
            ActiveRecordSettings.ConnectionManager = connectionManager;
        }

        [TearDown]
        public void Cleanup() {
            Directory.Delete(directory, true);
        }

        
        #endregion

        [Test]
        public void TestSimpleFunctionality() {
            Person.Migrate();
            var p = Person.Build();
            p.Name = "hello";
            p.Save();
            p = Person.Find(p.Id);
            Assert.AreEqual("hello", p.Name);
        }

        [Test]
        public void TestSimpleMigration() {
            Person.Migrate();
            
            var p = Person.Build();
            p.Name = "Sam";
            p.Save();
            int id = p.Id; 
            
            Person2.Migrate();
            
            var p2 = Person2.Find(id);
            
            Assert.AreEqual("Sam", p2.Name);
            
            p2.LastName = "Bob";
            p2.Save();
            p2 = Person2.Find(p2.Id);

            Assert.AreEqual(p2.LastName, "Bob"); 

        }
    }
}
