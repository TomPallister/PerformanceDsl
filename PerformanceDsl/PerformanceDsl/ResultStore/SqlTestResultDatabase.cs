using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Newtonsoft.Json;
using PerformanceDsl.ResultStore.Infrastructure;
using PerformanceDsl.ResultStore.Infrastructure.Enums;
using PerformanceDsl.ResultStore.Infrastructure.Extensions;

namespace PerformanceDsl.ResultStore
{
    public class SqlTestResultDatabase : ITestResultDatabase
    {
        public void Store(Result result)
        {
            using (var sP = new StoredProcedure(DataBase.Default, "sp_InsertResult", SetUpResultParameters(result)))
            {
                sP.Execute();
            }
        }

        public List<Result> Get()
        {
            throw new NotImplementedException();
        }

        private List<SqlParameter> SetUpResultParameters(Result result)
        {
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@Json", JsonConvert.SerializeObject(result)),
                new SqlParameter("@TestRunId", result.TestRunGuid),
                new SqlParameter("@CurrentHtml", result.CurrentHtml),
                new SqlParameter("@StepName", result.StepName),
                new SqlParameter("@HttpStatusCode", result.HttpStatusCode),
                new SqlParameter("@HttpPostMethod", result.HttpPostMethod.ToString()),
                new SqlParameter("@Url", result.Url),
                new SqlParameter("@ScenarioName", result.ScenarioName),
                new SqlParameter("@ElapsedTimeInMilliseconds", result.ElapsedTimeInMilliseconds),
                new SqlParameter("@Date", result.Date.ToUniversalTime())
            };

            return sqlParameters;
        }

        public List<Result> Get(Guid guid)
        {
            throw new NotImplementedException();
        }

        private Result SetUpResult(SqlDataReader reader)
        {
            return JsonConvert.DeserializeObject<Result>(reader.SafeValue("Json", ""));
        }
    }
}