using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Rhino.Mocks;

using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Tests.Controllers
{
    [TestClass]
    public class ProductControllerTest
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
        public void List_CanPaginate_ExpectCorrectProductResults()
        {
            // Arrange
            const int Page = 2;

            string category = null;

            var products = new[]
                            {
                                new Product { ProductId = 1, Name = "P1" },
                                new Product { ProductId = 2, Name = "P2" },
                                new Product { ProductId = 3, Name = "P3" },
                                new Product { ProductId = 4, Name = "P4" },
                                new Product { ProductId = 5, Name = "P5" }
                            }.AsQueryable();

            var productRepository = this.mocks.StrictMock<IProductRepository>();
            Expect.Call(productRepository.Products).Return(products).Repeat.Twice();

            this.mocks.ReplayAll();

            var controller = new ProductController(productRepository) { PageSize = 3 };

            // Act
            var result = controller.List(category, Page).Model as ProductListViewModel;
            
            // Assert
            Assert.IsNotNull(result);
            var productArray = result.Products.ToArray();

            Assert.IsTrue(productArray.Length == 2);
            Assert.AreEqual(productArray[0].Name, "P4");
            Assert.AreEqual(productArray[1].Name, "P5");
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void List_CanPaginate_ExpectCorrectPagedResults()
        {
            // Arrange
            const int CurrentPage = 2;
            const int PageSize = 3;

            string category = null;

            var products = new[]
                            {
                                new Product { ProductId = 1, Name = "P1" },
                                new Product { ProductId = 2, Name = "P2" },
                                new Product { ProductId = 3, Name = "P3" },
                                new Product { ProductId = 4, Name = "P4" },
                                new Product { ProductId = 5, Name = "P5" }
                            }.AsQueryable();

            var productRepository = this.mocks.StrictMock<IProductRepository>();
            Expect.Call(productRepository.Products).Return(products).Repeat.Twice();

            this.mocks.ReplayAll();

            var controller = new ProductController(productRepository) { PageSize = PageSize };

            // Act
            var result = controller.List(category, CurrentPage).Model as ProductListViewModel;

            // Assert
            Assert.IsNotNull(result);
            var pageInfo = result.PagingInfo;

            Assert.AreEqual(pageInfo.CurrentPage, CurrentPage);
            Assert.AreEqual(pageInfo.ItemsPerPage, PageSize);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void List_CanFilterByCategory_ExpectCorrectlyFilteredResults()
        {
            // Arrange
            const int CurrentPage = 1;
            const string Category = "Cat2";

            var products = new[]
                            {
                                new Product { ProductId = 1, Name = "P1", Category = "Cat1" },
                                new Product { ProductId = 2, Name = "P2", Category = "Cat2" },
                                new Product { ProductId = 3, Name = "P3", Category = "Cat1" },
                                new Product { ProductId = 4, Name = "P4", Category = "Cat2" },
                                new Product { ProductId = 5, Name = "P5", Category = "Cat3" }
                            }.AsQueryable();

            var productRepository = this.mocks.StrictMock<IProductRepository>();
            Expect.Call(productRepository.Products).Return(products).Repeat.Twice();

            this.mocks.ReplayAll();

            var controller = new ProductController(productRepository);

            // Act
            var result = controller.List(Category, CurrentPage).Model as ProductListViewModel;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.CurrentCategory, Category);

            var productArray = result.Products.ToArray();
            Assert.AreEqual(productArray.Length, 2);
            Assert.IsTrue(productArray[0].Name == "P2" && productArray[0].Category == Category);
            Assert.IsTrue(productArray[1].Name == "P4" && productArray[1].Category == Category);
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void List_CanFilterByCategories_ExpectCorrectTotalItemsCountForEachCategory()
        {
            // Arrange

            var products = new[]
                            {
                                new Product { ProductId = 1, Name = "P1", Category = "Cat1" },
                                new Product { ProductId = 2, Name = "P2", Category = "Cat2" },
                                new Product { ProductId = 3, Name = "P3", Category = "Cat1" },
                                new Product { ProductId = 4, Name = "P4", Category = "Cat2" },
                                new Product { ProductId = 5, Name = "P5", Category = "Cat3" },
                                new Product { ProductId = 6, Name = "P6", Category = "Cat3" },
                                new Product { ProductId = 7, Name = "P7", Category = "Cat3" },
                                new Product { ProductId = 8, Name = "P8", Category = "Cat3" }
                            }.AsQueryable();

            var productRepository = this.mocks.StrictMock<IProductRepository>();
            Expect.Call(productRepository.Products).Return(products).Repeat.Any();

            this.mocks.ReplayAll();

            var controller = new ProductController(productRepository);

            // Act
            var totalItemsForCat1 = ((ProductListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            var totalItemsForCat2 = ((ProductListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            var totalItemsForCat3 = ((ProductListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;

            // Assert
            Assert.AreEqual(totalItemsForCat1, 2);
            Assert.AreEqual(totalItemsForCat2, 2);
            Assert.AreEqual(totalItemsForCat3, 4);
        }

        //// ----------------------------------------------------------------------------------------------------------
    }
}
