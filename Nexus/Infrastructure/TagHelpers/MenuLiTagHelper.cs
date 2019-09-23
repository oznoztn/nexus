using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Nexus.Infrastructure.TagHelpers
{
    [HtmlTargetElement("li", Attributes = ControllerAttributeName)]
    [HtmlTargetElement("li", Attributes = ActionAttributeName)]
    [HtmlTargetElement("li", Attributes = SlugAttributeName)]
    [HtmlTargetElement("li", Attributes = MenuTypeAttributeName)]
    public class MenuLiTagHelper : TagHelper
    {
        public const string ControllerAttributeName = "x-controller";
        public const string ActionAttributeName = "x-action";
        public const string SlugAttributeName = "x-slug";
        public const string MenuTypeAttributeName = "x-menu-type";

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(ControllerAttributeName)]
        public string Controller { get; set; }

        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        [HtmlAttributeName(MenuTypeAttributeName)]
        public string MenuType { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string requestedController = ViewContext.RouteData.Values["controller"].ToString();
            string requestedAction = ViewContext.RouteData.Values["action"].ToString();

            if (MenuType == "single")
            {
                if (Controller.Equals(requestedController, StringComparison.InvariantCultureIgnoreCase) &&
                    Action.Equals(requestedAction, StringComparison.InvariantCultureIgnoreCase))
                {
                    output.Attributes.SetAttribute("class", "active");
                }
            }
            else if (MenuType == "multi")
            {
                output.Attributes.SetAttribute("class",
                    Controller.Equals(requestedController, StringComparison.InvariantCultureIgnoreCase)
                        ? "treeview active"
                        : "treeview");
            }
        }
    }
}
