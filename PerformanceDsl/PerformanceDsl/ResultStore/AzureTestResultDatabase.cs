using System;
using System.Collections.Generic;
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

        public AzureTestResultDatabase(CloudStorageAccount storageAccount)
        {
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

        public List<Result> Get()
        {
            IQueryable<Result> query = from entity in _cloudTable.CreateQuery<ResultEntity>()
                select
                    new Result((HttpStatusCode) Enum.Parse(typeof (HttpStatusCode), entity.HttpStatusCode),
                        StringCompression.Unzip(entity.CurrentHtml),
                        entity.ElapsedTimeInMilliseconds,
                        (HttpPostMethod) Enum.Parse(typeof (HttpPostMethod), entity.HttpPostMethod), entity.Url,
                        entity.ScenarioName, entity.TestRunGuid, entity.StepName, entity.Date);


            return query.ToList();
        }

        public List<Result> Get(Guid guid)
        {
            IQueryable<Result> query = from entity in _cloudTable.CreateQuery<ResultEntity>()
                where entity.TestRunGuid == guid
                select
                    new Result((HttpStatusCode) Enum.Parse(typeof (HttpStatusCode), entity.HttpStatusCode),
                        StringCompression.Unzip(entity.CurrentHtml),
                        entity.ElapsedTimeInMilliseconds,
                        (HttpPostMethod) Enum.Parse(typeof (HttpPostMethod), entity.HttpPostMethod), entity.Url,
                        entity.ScenarioName, entity.TestRunGuid, entity.StepName, entity.Date);


            return query.ToList();
        }
    }
}