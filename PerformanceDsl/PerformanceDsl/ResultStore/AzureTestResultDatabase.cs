using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

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
            _cloudTable = cloudTableClient.GetTableReference("Results");
            _cloudTable.CreateIfNotExists();
        }

        public void Store(Result result)
        {
            var resultEntity = new ResultEntity(
                result.HttpStatusCode, 
                result.CurrentHtml,
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
}