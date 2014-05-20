using System.Collections.Generic;
using System.Data.SqlClient;
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

        private List<SqlParameter> SetUpResultParameters(Result result)
        {
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@Json", JsonConvert.SerializeObject(result)),
                new SqlParameter("@TestRunId", result.TestRunGuid),
            };

            return sqlParameters;
        }

        private Result SetUpResult(SqlDataReader reader)
        {
            return JsonConvert.DeserializeObject<Result>(reader.SafeValue("Json", ""));
        }
    }
}