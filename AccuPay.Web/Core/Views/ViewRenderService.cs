using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AccuPay.Web.Core.Views
{
    public class ViewRenderService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public ViewRenderService(
            IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RenderText(string viewName, object model)
        {
            var actionContext = CreateActionContext();
            var view = FindView(viewName, actionContext);
            var content = await Render(view, model, actionContext);

            return content;
        }

        public async Task<string> RenderHtml(string viewName, object model, bool inlineCss = true)
        {
            var actionContext = CreateActionContext();
            var view = FindView(viewName, actionContext);
            var content = await Render(view, model, actionContext);

            if (inlineCss)
            {
                return MoveCssInline(content);
            }
            else
            {
                return content;
            }
        }

        private ActionContext CreateActionContext()
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            return actionContext;
        }

        private IView FindView(string name, ActionContext actionContext)
        {
            var result = _razorViewEngine.FindView(actionContext, name, false);

            if (result.View == null)
            {
                throw new ArgumentNullException($"{name} does not match any available view");
            }

            return result.View;
        }

        private async Task<string> Render(IView view, object model, ActionContext actionContext)
        {
            var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            using var sw = new StringWriter();

            var viewContext = new ViewContext(
                actionContext,
                view,
                viewDictionary,
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                sw,
                new HtmlHelperOptions()
            );

            await view.RenderAsync(viewContext);

            return sw.ToString();
        }

        private string MoveCssInline(string content)
        {
            using var premailer = new PreMailer.Net.PreMailer(content);
            var result = premailer.MoveCssInline(
                removeStyleElements: true,
                ignoreElements: null,
                css: null,
                stripIdAndClassAttributes: true,
                removeComments: true);

            return result.Html;
        }
    }
}
