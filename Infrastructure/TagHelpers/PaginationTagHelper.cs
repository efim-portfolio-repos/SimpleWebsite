using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SimpleWebsite.Infrastructure.TagHelpers
{
    [HtmlTargetElement("pagination", Attributes = "elements-count")]
    public class PaginationTagHelper : TagHelper
    {
        private IUrlHelperFactory _urlHelperFactory;
        public PaginationTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public int ElementsOnPage { get; set; } = 5;
        public int ElementsCount { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int LinkCount { get; set; } = 5;

        public string ClassForActive { get; set; }
        public string ClassForArrow { get; set; }
        public string ClassForLink { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContextData { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContextData);

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            int pagesCount = ElementsCount / ElementsOnPage + (ElementsCount % ElementsOnPage == 0 ? 0 : 1);
            int fromPage;
            int toPage;
            if (pagesCount <= LinkCount)
            {
                fromPage = 1;
                toPage = pagesCount;
            }
            else if (CurrentPage + LinkCount - 1 < pagesCount)
            {
                fromPage = CurrentPage;
                toPage = CurrentPage + LinkCount - 1;
            }
            else
            {
                fromPage = pagesCount - LinkCount;
                toPage = pagesCount;
            }



            if (CurrentPage > 1)
            {
                output.Content.AppendHtml(GetAnchor(urlHelper, ClassForArrow, "&laquo;", 1));
                output.Content.AppendHtml(GetAnchor(urlHelper, ClassForArrow, "&lt;", CurrentPage - 1));
            }

            for (int i = fromPage; i <= toPage; i++)
            {
                if (CurrentPage == i)
                {
                    output.Content.AppendHtml(GetAnchor(urlHelper, ClassForActive, i.ToString(), i));
                }
                else
                {
                    output.Content.AppendHtml(GetAnchor(urlHelper, ClassForLink, i.ToString(), i));
                }
            }

            if (CurrentPage < pagesCount)
            {
                output.Content.AppendHtml(GetAnchor(urlHelper, ClassForArrow, "&gt;", CurrentPage + 1));
                output.Content.AppendHtml(GetAnchor(urlHelper, ClassForArrow, "&raquo;", pagesCount));
            }
        }

        private string GetAnchor(IUrlHelper urlHelper, string classes, string content, int pageId)
        {
            string href = urlHelper.Action((string)ViewContextData.RouteData.Values["Action"], new
            {
                page = pageId
            });
            string classAttr = classes == null ? "" : $"class=\"{classes}\"";
            return $"<a {classAttr} href=\"{href}\">{content}</a>";
        }
    }
}