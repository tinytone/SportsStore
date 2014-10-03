using System;
using System.Web.Mvc;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SportsStore.WebUI.HTMLHelpers;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Tests.HTMLHelpers
{
    [TestClass]
    public class HtmlHelperExtensions
    {
        //// ----------------------------------------------------------------------------------------------------------
		 
        [TestMethod]
        public void PageLinks_GeneragePageLinks_ExpectThreeGeneratedATags()
        {
            // Arrange
            HtmlHelper htmlHelper = null;

            var pagingInfo = new PagingInfo { CurrentPage = 2, TotalItems = 28, ItemsPerPage = 10 };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Act
            var result = htmlHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Assert
            Assert.AreEqual(result.ToString(), @"<a href=""Page1"">1</a><a class=""selected"" href=""Page2"">2</a><a href=""Page3"">3</a>");
        }

        //// ----------------------------------------------------------------------------------------------------------
    }
}