using System;
using System.Threading.Tasks;
using PerformanceDsl.Logging;
using Xunit;

namespace PerformanceDsl.TDD
{
    public class Tests
    {
        private readonly ILogger _logger;

        public Tests()
        {
            _logger = new Log4NetLog();
        }

        [Fact]
        public void can_register_user_on_web_forms_app_sync()
        {
            var tests = new PerformanceDsl.Tests.Tests(Guid.NewGuid(), _logger);
            tests.SyncTestWebFormsGetAndPost();
        }

        [Fact]
        public async Task can_register_user_on_web_forms_app_async()
        {
            var tests = new PerformanceDsl.Tests.Tests(Guid.NewGuid(), _logger);
            await tests.ASyncTestWebFormsGetAndPost();
        }

        [Fact]
        public void can_get_from_mvc_app_sync()
        {
            var tests = new PerformanceDsl.Tests.Tests(Guid.NewGuid(), _logger);
            tests.SyncTestMvcGetRequest();
        }

        [Fact]
        public async Task can_get_from_mvc_app_async()
        {
            var tests = new PerformanceDsl.Tests.Tests(Guid.NewGuid(), _logger);
            await tests.ASyncTestMvcGetRequest();
        }

        [Fact]
        public void can_post_to_mvc_app_sync()
        {
            var tests = new PerformanceDsl.Tests.Tests(Guid.NewGuid(), _logger);
            tests.SyncTestMvcPostRequest();
        }

        [Fact]
        public async Task can_post_to_mvc_app_Async()
        {
            var tests = new PerformanceDsl.Tests.Tests(Guid.NewGuid(), _logger);
            await tests.ASyncTestMvcPostRequest();
        }
    }
}