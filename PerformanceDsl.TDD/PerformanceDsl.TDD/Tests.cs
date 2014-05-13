using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PerformanceDsl.TDD
{
    public class Tests
    {
        [Fact]
        public void can_register_user_on_web_forms_app_sync()
        {
            var tests = new PerformanceDsl.Tests.Tests(Guid.NewGuid());
            tests.SyncTestWebFormsGetAndPost();
        }

        [Fact]
        public void can_get_from_mvc_app_sync()
        {
            var tests = new PerformanceDsl.Tests.Tests(Guid.NewGuid());
            tests.SyncTestMvcGetRequest();
        }

        [Fact]
        public void can_post_to_mvc_app_sync()
        {
            var tests = new PerformanceDsl.Tests.Tests(Guid.NewGuid());
            tests.SyncTestMvcPostRequest();
        }
    }
}
