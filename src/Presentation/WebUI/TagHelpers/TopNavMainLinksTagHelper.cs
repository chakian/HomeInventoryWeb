using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebUI.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "highlight-active")]
    public class TopNavMainLinksTagHelper : TagHelper //AnchorTagHelper
    {
        //public TopNavMainLinksTagHelper(IHtmlGenerator generator) : base(generator)
        //{
        //}

        //https://github.com/dpaquette/TagHelperSamples/blob/master/TagHelperSamples/src/TagHelperSamples.Bootstrap/NavLinkTagHelper.cs

        private IUrlHelper urlHelper;

        public TopNavMainLinksTagHelper(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //Remove marker attribute
            output.Attributes.Remove(output.Attributes["highlight-active"]);

            //Get the url from href attribute generaed in the default AnchorTagHelper
            var url = output.Attributes["href"].Value.ToString();

            //Add active css class only when current request matches the generated href
            var currentRouteUrl = this.urlHelper.Action();
            if (url == currentRouteUrl)
            {
                var linkTag = new TagBuilder("a");
                linkTag.Attributes.Add("class", "active");
                output.MergeAttributes(linkTag);
            }



            //var currentClass = output.Attributes["class"]?.Value?.ToString();
            //currentClass = (string.IsNullOrEmpty(currentClass) ? "active" : currentClass += " active");

            //output.Attributes.Remove(output.Attributes["class"]);
            //output.Attributes.Add("class", currentClass);

            //var linkTag = new TagBuilder("a");

            //linkTag.Attributes.Add("class", currentClass);
            
            //output.MergeAttributes(linkTag);

            //base.Process(context, output);
        }
    }
}
