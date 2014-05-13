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
        public void can_register_user_on_web_forms_app()
        {
            const string hostUrl = "http://www.testwebformsapp.dev/";
            var tests = new PerformanceDsl.Tests.Tests();
            tests.TestWebFormsGetAndPost(hostUrl);
        }

        [Fact]
        public void can_get_from_mvc_app()
        {
            const string hostUrl = "http://www.testmvcapp.dev/";
            var tests = new PerformanceDsl.Tests.Tests();
            tests.TestMvcGetRequest(hostUrl);
        }

        [Fact]
        public void can_post_to_mvc_app()
        {
            const string hostUrl = "http://www.testmvcapp.dev/";
            var tests = new PerformanceDsl.Tests.Tests();
            tests.TestMvcPostRequest(hostUrl);
        }
    }
}
