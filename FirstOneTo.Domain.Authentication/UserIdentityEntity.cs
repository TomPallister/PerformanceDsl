using System;
using System.Globalization;
using Microsoft.WindowsAzure.Storage.Table;

namespace FirstOneTo.Authentication
{
    public class UserIdentityEntity : TableEntity
    {
        public UserIdentityEntity()
        {
        }

        public UserIdentityEntity(
            string userName,
            string password,
            string claims)
        {
            PartitionKey = Guid.NewGuid().ToString();
            RowKey = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now;
            Password = password;
            UserName = userName;
            Claims = claims;
        }

        public string Password { get; set; }
        public string UserName { get; set; }
        public string Claims { get; set; }
    }
}