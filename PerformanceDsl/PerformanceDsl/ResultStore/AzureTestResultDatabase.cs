using System;
using System.Configuration;
using System.Linq;
using System.Net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using PerformanceDsl.Helpers;

namespace PerformanceDsl.ResultStore
{
    public class AzureTestResultDatabase : ITestResultDatabase
    {
        private readonly CloudTable _cloudTable;

        public AzureTestResultDatabase()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            CloudTableClient cloudTableClient = storageAccount.CreateCloudTableClient();
            _cloudTable = cloudTableClient.GetTableReference(ConfigurationManager.AppSettings["TableName"]);
            _cloudTable.CreateIfNotExists();
        }

        public void Store(Result result)
        {
            byte[] compressedString = StringCompression.Zip(result.CurrentHtml);

            if (compressedString.Length < 64000)
            {
                var resultEntity = new ResultEntity(
                    result.HttpStatusCode,
                    compressedString,
                    result.ElapsedTimeInMilliseconds,
                    result.HttpPostMethod,
                    result.Url,
                    result.ScenarioName,
                    result.TestRunGuid,
                    result.StepName,
                    result.Date);
                TableOperation insertOperation = TableOperation.Insert(resultEntity);
                _cloudTable.Execute(insertOperation);
            }
            else
            {
                var resultEntity = new ResultEntity(
                    result.HttpStatusCode,
                    StringCompression.Zip("The html was truncated before it was stored."),
                    result.ElapsedTimeInMilliseconds,
                    result.HttpPostMethod,
                    result.Url,
                    result.ScenarioName,
                    result.TestRunGuid,
                    result.StepName,
                    result.Date);
                TableOperation insertOperation = TableOperation.Insert(resultEntity);
                _cloudTable.Execute(insertOperation);
            }
        }

        public IQueryable<Result> Get()
        {
            IQueryable<ResultEntity> query = from entity in _cloudTable.CreateQuery<ResultEntity>()
                select entity;

            IQueryable<Result> results = from resultEntity in query
                                         select new Result((HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), resultEntity.HttpStatusCode), StringCompression.Unzip(resultEntity.CurrentHtml),
                    resultEntity.ElapsedTimeInMilliseconds, (HttpPostMethod)Enum.Parse(typeof(HttpPostMethod), resultEntity.HttpPostMethod), resultEntity.Url,
                    resultEntity.ScenarioName, resultEntity.TestRunGuid, resultEntity.StepName, resultEntity.Date);
            return results;
        }

        public IQueryable<Result> Get(Guid guid)
        {
            IQueryable<ResultEntity> query = from entity in _cloudTable.CreateQuery<ResultEntity>()
                where entity.TestRunGuid == guid
                select entity;
            IQueryable<Result> results = from resultEntity in query
                                         select new Result((HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), resultEntity.HttpStatusCode), StringCompression.Unzip(resultEntity.CurrentHtml),
                    resultEntity.ElapsedTimeInMilliseconds, (HttpPostMethod)Enum.Parse(typeof(HttpPostMethod), resultEntity.HttpPostMethod), resultEntity.Url,
                    resultEntity.ScenarioName, resultEntity.TestRunGuid, resultEntity.StepName, resultEntity.Date);
            return results;
        }
    }
}