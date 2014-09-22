using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FirstOneTo.Authentication;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using PerformanceDsl.Helpers;

namespace FirstOneTo.ReadModel.Infrastructure.Azure
{
    public class AzureUserDatabase : IReadModelUserDatabase
    {
        private readonly CloudTable _cloudTable;

        public AzureUserDatabase(CloudStorageAccount storageAccount)
        {
            CloudTableClient cloudTableClient = storageAccount.CreateCloudTableClient();
            _cloudTable = cloudTableClient.GetTableReference(ConfigurationManager.AppSettings["UserTableName"]);
            _cloudTable.CreateIfNotExists();
            IUser demo = GetUser("demo");
            if (demo == null)
            {
                AddUser("demo", "demo", new List<string> {"demo", "admin"});
            }
            IUser nonadmin = GetUser("nonadmin");
            if (nonadmin == null)
            {
                AddUser("nonadmin", "nonadmin", new List<string> {"demo",});
            }
        }

        public IUser GetUser(string userName, string password)
        {
            IQueryable<UserIdentity> query =
                from entity in _cloudTable.CreateQuery<UserIdentityEntity>()
                where entity.UserName == userName
                      && entity.Password == password
                select
                    new UserIdentity(entity.UserName, entity.Password,
                        AzureListStringHelpers.GetItemsFromDelimitedString(entity.Claims));
            return query.FirstOrDefault();
        }

        public IUser GetUser(string userName)
        {
            IQueryable<UserIdentity> query =
                from entity in _cloudTable.CreateQuery<UserIdentityEntity>()
                where entity.UserName == userName
                select
                    new UserIdentity(entity.UserName, entity.Password,
                        AzureListStringHelpers.GetItemsFromDelimitedString(entity.Claims));
            return query.FirstOrDefault();
        }


        public void AddUser(string userName, string password, List<string> claims)
        {
            var submittedChallengeEntity = new UserIdentityEntity(userName, password,
                AzureListStringHelpers.CreateDelimitedString(claims));
            TableOperation insertOperation = TableOperation.Insert(submittedChallengeEntity);
            _cloudTable.Execute(insertOperation);
        }
    }
}