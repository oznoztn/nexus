using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Nexus.Infrastructure.TagHelpers
{
    [HtmlTargetElement("a", Attributes = IsHiddenAttributeName)]
    [HtmlTargetElement("a", Attributes = HiddenAnchorClassAttributeName)]
    public class NexusAnchorTagHelper : TagHelper
    {
        private const string IsHiddenAttributeName = "nex-hidden";
        private const string HiddenAnchorClassAttributeName = "nex-hidden-class";
        private string DefaultHiddenAnchorClassValue { get; }
        private bool IsAdminLoggedIn { get; }
        
        [HtmlAttributeName(IsHiddenAttributeName)]
        public bool IsHidden { get; set; }

        [HtmlAttributeName(HiddenAnchorClassAttributeName)]
        public string HiddenAnchorClass { get; set; }

        public NexusAnchorTagHelper(IHtmlGenerator generator, IHttpContextAccessor contextAccessor)
        {
            DefaultHiddenAnchorClassValue = "hidden-title";

            IsAdminLoggedIn = contextAccessor.HttpContext.User.IsInRole("Administrator");
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (IsHidden)
            {
                if (IsAdminLoggedIn)
                {
                    bool hasClassAttr = context.AllAttributes.TryGetAttribute("class", out var @class);
                    bool hasHiddenClassValue = !string.IsNullOrWhiteSpace(HiddenAnchorClass);

                    if (hasClassAttr)
                    {
                        output.Attributes.SetAttribute("class",
                            hasHiddenClassValue
                                ? string.Concat(@class.Value, " ", HiddenAnchorClass)
                                : string.Concat(@class.Value, " ", DefaultHiddenAnchorClassValue));
                    }
                    else
                    {
                        output.Attributes.SetAttribute("class", hasHiddenClassValue ? HiddenAnchorClass : DefaultHiddenAnchorClassValue);
                    }

                    base.Process(context, output);
                }
                else
                {
                    // admin is not logged in and the anchor is hidden so interrupt the rendering
                }
            }
            else
            {
                base.Process(context, output);
            }
        }
    }
}
