using System.Web;
using System.Web.SessionState;

namespace PerformanceDsl.ResultStore.Infrastructure.Handlers
{
    public abstract class RequireJSHandler : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public abstract void ProcessRequest(HttpContext context);

        protected string GetConfig()
        {
            return @"
                    requirejs.config({
                        baseUrl: '/site/scriptlibrary',
                        paths: {
                            bindingHandlers: 'BindingHandlers',
                            bootstrap: 'bootstrap/js/bootstrap',
                            bootstrapPlugins: 'bootstrap/js',
                            domReady: 'Plugins/domReady',
                            jquery: 'Libraries/jquery',
                            jqueryui: 'Libraries/jquery-ui',
                            knockout: 'Libraries/knockout',
                            libraries: 'Libraries',
                            plugins: 'Plugins',
                            viewModels: 'ViewModels'
                        },
                        shim: {
                            'bootstrap': ['jquery']
                        }
                    });";
        }
    }
}