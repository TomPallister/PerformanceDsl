using System;
using System.Collections.Generic;
using FirstOneTo.Authentication;
using FirstOneTo.ReadModel.Infrastructure.Azure;
using FluentAssertions;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NUnit.Framework;

namespace FirstOneTo.ReadModel.Infrastructure.Tests
{
    [TestFixture]
    public class AzureUserDatabaseTests
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            var rnd = new Random();
            int number = rnd.Next(999999999);
            _tableReference = string.Format("DatabaseForUnitTesting{0}", number);
            _storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
            _database = new AzureUserDatabase(_storageAccount);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(_tableReference);
            table.DeleteIfExists();
        }

        private AzureUserDatabase _database;
        private CloudStorageAccount _storageAccount;
        private string _tableReference;

        [Test]
        public void can_get_user_by_name()
        {
            _database.AddUser("someotherusername", "somepassword", new List<string> {"someusername", "admin"});
            IUser user = _database.GetUser("someotherusername");
            user.UserName.Should().Be("someotherusername");
            user.Claims.Should().NotBeEmpty();
        }

        [Test]
        public void can_get_user_by_name_and_password()
        {
            _database.AddUser("someotherotherusername", "somepassword", new List<string> {"someusername", "admin"});
            IUser user = _database.GetUser("someotherotherusername");
            user.UserName.Should().Be("someotherotherusername");
            user.Claims.Should().NotBeEmpty();
        }

        [Test]
        public void can_store_user()
        {
            _database.AddUser("someusername", "somepassword", new List<string> {"someusername", "admin"});
        }
    }
}