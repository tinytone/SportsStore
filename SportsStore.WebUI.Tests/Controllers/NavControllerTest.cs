using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Rhino.Mocks;

using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Tests.Controllers
{
    [TestClass]
    public class NavControllerTest
    {
        //// ----------------------------------------------------------------------------------------------------------

        private MockRepository mocks;

        //// ----------------------------------------------------------------------------------------------------------

        [TestInitialize]
        public void TestInitialize()
        {
            this.mocks = new MockRepository();
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestCleanup]
        public void TestCleanup()
        {
            // Ensure all our mock expectations were satisfied.
            this.mocks.VerifyAll();
        }
        
        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Menu_GetDistinctCategories_ExpectDistinctCategories()
        {
            // Arrange
            string category = null;

            var products = new[]
                            {
                                new Product { ProductId = 1, Name = "P1", Category = "Apples" },
                                new Product { ProductId = 2, Name = "P2", Category = "Apples" },
                                new Product { ProductId = 3, Name = "P3", Category = "Plums" },
                                new Product { ProductId = 4, Name = "P4", Category = "Oranges" },
                                new Product { ProductId = 5, Name = "P5", Category = "Oranges" }
                            }.AsQueryable();

            var productRepository = this.mocks.StrictMock<IProductRepository>();
            Expect.Call(productRepository.Products).Return(products).Repeat.Twice();

            this.mocks.ReplayAll();

            var controller = new NavController(productRepository);

            // Act
            var results = ((IEnumerable<String>)controller.Menu(category).Model).ToArray();

            // Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Length == 3);
            Assert.AreEqual(results[0], "Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2], "Plums");
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Menu_SelectedCategorySet_ExpectCategoryWithinTheViewBag()
        {
            // Arrange
            const string Category = "Apples";

            var products = new[]
                            {
                                new Product { ProductId = 1, Name = "P1", Category = "Apples" },
                                new Product { ProductId = 2, Name = "P2", Category = "Apples" },
                                new Product { ProductId = 3, Name = "P3", Category = "Plums" },
                                new Product { ProductId = 4, Name = "P4", Category = "Oranges" },
                                new Product { ProductId = 5, Name = "P5", Category = "Oranges" }
                            }.AsQueryable();

            var productRepository = this.mocks.StrictMock<IProductRepository>();
            Expect.Call(productRepository.Products).Return(products).Repeat.Twice();

            this.mocks.ReplayAll();

            var controller = new NavController(productRepository);

            // Act
            var viewBag = controller.Menu(Category).ViewBag;

            // Assert
            Assert.IsNotNull(viewBag);
            Assert.AreEqual(viewBag.SelectedCategory, Category);
        }

        //// ----------------------------------------------------------------------------------------------------------
    }
}
