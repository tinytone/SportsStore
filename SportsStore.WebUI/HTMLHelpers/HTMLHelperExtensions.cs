using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.HTMLHelpers
{
    public static class HtmlHelperExtensions
    {
        //// ----------------------------------------------------------------------------------------------------------
		 
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            var result = new StringBuilder();

            for (var i = 1; i <= pagingInfo.TotalPages; i++)
            {
                var tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString(CultureInfo.InvariantCulture);

                if (i == pagingInfo.CurrentPage)
                    tag.AddCssClass("selected");

                result.Append(tag);
            }

            return MvcHtmlString.Create(result.ToString());
        }

        //// ----------------------------------------------------------------------------------------------------------
    }
}