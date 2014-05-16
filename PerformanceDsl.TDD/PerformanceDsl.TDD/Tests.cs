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
        public async Task can_register_user_on_web_forms_app_async()
        {
            var tests = new PerformanceDsl.Tests.Tests(Guid.NewGuid(), _logger);
            await tests.ASyncTestWebFormsGetAndPost();
        }

        [Fact]
        public async Task can_get_from_mvc_app_async()
        {
            var tests = new PerformanceDsl.Tests.Tests(Guid.NewGuid(), _logger);
            await tests.ASyncTestMvcGetRequest();
        }

        [Fact]
        public async Task can_post_to_mvc_app_async()
        {
            var tests = new PerformanceDsl.Tests.Tests(Guid.NewGuid(), _logger);
            await tests.ASyncTestMvcPostRequest();
        }
    }
}