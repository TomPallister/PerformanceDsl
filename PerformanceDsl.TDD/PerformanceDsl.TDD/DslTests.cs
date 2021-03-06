﻿using System;
using System.Threading.Tasks;
using PerformanceDsl.Logging;
using Xunit;

namespace PerformanceDsl.TDD
{
    public class DslTests
    {
        private readonly ApiLogger _logger;

        public DslTests()
        {
            _logger = new ApiLogger();
        }

        [Fact]
        public async Task can_register_user_on_web_forms_app_async()
        {
            var tests = new Tests.Tests(Guid.NewGuid(), _logger);
            await tests.ASyncTestWebFormsGetAndPost();
        }

        [Fact]
        public async Task can_get_from_mvc_app_async()
        {
            var tests = new Tests.Tests(Guid.NewGuid(), _logger);
            await tests.ASyncTestMvcGetRequest();
        }

        [Fact]
        public async Task can_post_to_mvc_app_async()
        {
            var tests = new Tests.Tests(Guid.NewGuid(), _logger);
            await tests.ASyncTestMvcPostRequest();
        }

        [Fact]
        public async Task can_get_from_bbc()
        {
            var tests = new Tests.Tests(Guid.NewGuid(), _logger);
            await tests.BbcGetRequest();
        }
    }
}