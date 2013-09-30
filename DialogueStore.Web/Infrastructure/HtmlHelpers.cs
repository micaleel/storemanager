using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace DialogueStore.Web.Infrastructure
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            var builder = new TagBuilder("li")
                {
                    InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName).ToHtmlString()
                };

            var sameController = controllerName.Equals(currentController, StringComparison.InvariantCultureIgnoreCase);
            var sameAction = actionName.Equals(currentAction, StringComparison.InvariantCultureIgnoreCase);

            if (sameController && sameAction)
            {
                builder.AddCssClass("active");
            }

            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString NavLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues = null)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            var actionLink = routeValues == null
                                 ? htmlHelper.ActionLink(linkText, actionName, controllerName)
                                 : htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, null);

            var builder = new TagBuilder("li") { InnerHtml = actionLink.ToHtmlString() };

            var sameController = controllerName.Equals(currentController, StringComparison.InvariantCultureIgnoreCase);
            var sameAction = actionName.Equals(currentAction, StringComparison.InvariantCultureIgnoreCase);

            if (sameController && sameAction)
            {
                builder.AddCssClass("active");
            }

            return new MvcHtmlString(builder.ToString());
        }
    }
}