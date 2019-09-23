using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Nexus.Infrastructure.TagHelpers
{
    [HtmlTargetElement("li", Attributes = SelectedLiClassAttributeName)]
    public class SelectedLiTagHelper : TagHelper
    {
        public const string SelectedLiClassAttributeName = "x-selected-li-class";
        public const string SlugAttributeName = "x-slug-to-match";

        [HtmlAttributeNotBound, ViewContext] public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(SelectedLiClassAttributeName)]
        public string SelectedLiClass { get; set; }

        [HtmlAttributeName(SlugAttributeName)]
        public string Slug { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext.RouteData.Values.ContainsKey("slug"))
            {
                if (Slug.Equals((string)ViewContext.RouteData.Values["slug"], StringComparison.CurrentCulture))
                {
                    output.AddClass(SelectedLiClass, HtmlEncoder.Default);
                }
            }
        }
    }
}