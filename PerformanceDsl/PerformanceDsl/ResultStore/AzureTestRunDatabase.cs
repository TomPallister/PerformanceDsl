using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace PerformanceDsl.ResultStore
{
    public class AzureTestRunDatabase : ITestRunDatabase
    {
        private readonly CloudTable _cloudTable;

        public AzureTestRunDatabase(CloudStorageAccount storageAccount)
        {
            CloudTableClient cloudTableClient = storageAccount.CreateCloudTableClient();
            _cloudTable = cloudTableClient.GetTableReference(ConfigurationManager.AppSettings["TestRunTableName"]);
            _cloudTable.CreateIfNotExists();
        }

        public void Store(Run run)
        {
            var runEntity = new RunEntity(run.TestRunGuid, run.ProjectName, run.StartDate);
            TableOperation insertOperation = TableOperation.Insert(runEntity);
            _cloudTable.Execute(insertOperation);
        }

        public List<Run> Get()
        {
            var runs = new List<Run>();
            IQueryable<RunEntity> query = from entity in _cloudTable.CreateQuery<RunEntity>()
                select entity;
            foreach (RunEntity resultEntity in query)
            {
                var run = new Run(resultEntity.TestRunGuid, resultEntity.ProjectName,
                    resultEntity.StartDate);
                runs.Add(run);
            }
            return runs;
        }
    }
}