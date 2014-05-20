using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

namespace PerformanceDsl.ResultStore.Infrastructure
{
    /// <summary>
    ///     Hook from ASP.Net control to the MVC Partials. Overrides the render method and renders the given View and Model
    ///     in to the ASP.Net controls container
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Partial<T> : UserControl
    {
        #region Members

        private ViewDataDictionary viewdata;

        #endregion

        #region Ctor

        /// <summary>
        ///     Constructor
        /// </summary>
        public Partial(T model, string viewPath)
        {
            Model = model;
            PartialPath = viewPath;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public Partial(string viewPath)
        {
            PartialPath = viewPath;
        }

        #endregion

        #region Overrides

        /// <summary>
        ///     Renders the MVC Partial using the Path and Model then writes it in to the controls HtmlTextWriter
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            RenderToWriter(writer);
            base.Render(writer);
        }

        /// <summary>
        ///     Renders the MVC Partial using the Path and Model then writes it in to the controls HtmlTextWriter
        /// </summary>
        /// <param name="writer"></param>
        protected void RenderToWriter(TextWriter writer)
        {
            //get a wrapper for the legacy WebForm context
            var httpCtx = new HttpContextWrapper(HttpContext.Current);

            //create a mock route that points to the empty controller
            var rt = new RouteData();
            rt.Values.Add("controller", "WebFormController");

            if (Model == null)
            {
                Model = GetModel();
            }

            //create a view context and assign the model
            var td = new TempDataDictionary();
            ViewData.Model = Model;

            //create a controller context for the route and http context
            var ctx = new ControllerContext(new RequestContext(httpCtx, rt), new WebFormController());

            //find the partial view using the viewengine
            IView view = ViewEngines.Engines.FindPartialView(ctx, PartialPath).View;

            //Note: If you're getting view null errors here, it'll be because your view path is wrong
            var vctx = new ViewContext(ctx, view, ViewData, td, writer);

            //render the partial view
            view.Render(vctx, writer);
        }

        /// <summary>
        ///     Returns the Model
        /// </summary>
        /// <returns></returns>
        public virtual T GetModel()
        {
            throw new NotImplementedException(
                "If the model is not provided at initialization of object, it must be returned by GetModel(). Implement GetModel().");
        }

        public string RenderControl()
        {
            //Create memory writer 
            var sb = new StringBuilder();
            var memWriter = new StringWriter(sb);
            RenderToWriter(memWriter);
            //Flush memory and return output 
            memWriter.Flush();
            return sb.ToString();
        }

        /// <summary>
        ///     Fake Controller
        /// </summary>
        private class WebFormController : Controller
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Full path to the partial view e.g. /Views/Dashboard/PersonalDetails.cshtml
        /// </summary>
        public virtual string PartialPath { get; private set; }

        /// <summary>
        ///     Model Data
        /// </summary>
        public virtual T Model { get; set; }

        /// <summary>
        ///     Provides access to the controllers view bag
        /// </summary>
        public ViewDataDictionary ViewData
        {
            get
            {
                if (viewdata == null)
                {
                    viewdata = new ViewDataDictionary();
                }
                return viewdata;
            }
        }

        #endregion
    }
}