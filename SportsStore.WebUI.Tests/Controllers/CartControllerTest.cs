using System;
using System.Linq;
using System.Web.UI;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Rhino.Mocks;

using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Tests.Controllers
{
    [TestClass]
    public class CartControllerTest
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ProductRepositoryIsNull_ExpectArgumentNullException()
        {
            // Arrange
            IProductRepository productRepository = null;
            var orderProcessor = this.mocks.StrictMock<IOrderProcessor>();

            this.mocks.ReplayAll();

            // Act
            GetCartController(productRepository, orderProcessor);

            // Assert
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OrderProcessorIsNull_ExpectArgumentNullException()
        {
            // Arrange
            var productRepository = this.mocks.StrictMock<IProductRepository>();
            IOrderProcessor orderProcessor = null;

            this.mocks.ReplayAll();

            // Act
            GetCartController(productRepository, orderProcessor);

            // Assert
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Constructor_AllDependanciesAreValid_ExpectInstance()
        {
            // Arrange
            var productRepository = this.mocks.StrictMock<IProductRepository>();
            var orderProcessor = this.mocks.StrictMock<IOrderProcessor>();

            this.mocks.ReplayAll();

            // Act
            var controller = GetCartController(productRepository, orderProcessor);

            // Assert
            Assert.IsNotNull(controller);
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void AddToCart_ProductDoesNotExistInCart_ExpectProductToBeAddedToCart()
        {
            // Arrange
            const int ProductId = 1;
            const string ReturnUrl = null;

            var cart = new Cart();

            var products = new[]
                            {
                                new Product { ProductId = ProductId, Name = "P1", Category = "Apples" }
                            }.AsQueryable();

            var productRepository = this.mocks.StrictMock<IProductRepository>();
            Expect.Call(productRepository.Products).Return(products);

            var orderProcessor = this.mocks.StrictMock<IOrderProcessor>();

            this.mocks.ReplayAll();

            var controller = GetCartController(productRepository, orderProcessor);

            // Act
            controller.AddToCart(cart, ProductId, ReturnUrl);

            // Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductId, ProductId);
            Assert.AreEqual(cart.Lines.ToArray()[0].Quantity, 1);
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void AddToCart_ProductDoesExistInCart_ExpectQuantityToBeUpdatedToCart()
        {
            // Arrange
            const int ProductId = 1;
            const string ReturnUrl = null;

            var cart = new Cart();

            var products = new[]
                            {
                                new Product { ProductId = ProductId, Name = "P1", Category = "Apples" }
                            }.AsQueryable();

            var productRepository = this.mocks.StrictMock<IProductRepository>();
            Expect.Call(productRepository.Products).Return(products).Repeat.Any();

            var orderProcessor = this.mocks.StrictMock<IOrderProcessor>();

            this.mocks.ReplayAll();

            var controller = GetCartController(productRepository, orderProcessor);
            controller.AddToCart(cart, ProductId, ReturnUrl);

            // Act
            controller.AddToCart(cart, ProductId, ReturnUrl);

            // Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductId, ProductId);
            Assert.AreEqual(cart.Lines.ToArray()[0].Quantity, 2);
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void AddToCart_ProductDoesNotExistInCart_ExpectGoesToCartScreen()
        {
            // Arrange
            const int ProductId = 1;
            const string ReturnUrl = "myUrl";

            var cart = new Cart();

            var products = new[]
                            {
                                new Product { ProductId = ProductId, Name = "P1", Category = "Apples" }
                            }.AsQueryable();

            var productRepository = this.mocks.StrictMock<IProductRepository>();
            Expect.Call(productRepository.Products).Return(products);

            var orderProcessor = this.mocks.StrictMock<IOrderProcessor>();

            this.mocks.ReplayAll();

            var controller = GetCartController(productRepository, orderProcessor);

            // Act
            var result = controller.AddToCart(cart, ProductId, ReturnUrl);

            // Assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], ReturnUrl);
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Index_Call_ExpectCanViewCartContents()
        {
            // Arrange
            const string ReturnUrl = "myUrl";

            var cart = new Cart();
            var productRepository = this.mocks.StrictMock<IProductRepository>();
            var orderProcessor = this.mocks.StrictMock<IOrderProcessor>();

            this.mocks.ReplayAll();

            var controller = GetCartController(productRepository, orderProcessor);

            // Act
            var result = controller.Index(cart, ReturnUrl).ViewData.Model as CartIndexViewModel;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, ReturnUrl);
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Checkout_WithEmptyCart_ExpectModelStateError()
        {
            // Arrange
            var cart = new Cart();

            var productRepository = this.mocks.StrictMock<IProductRepository>();
            var orderProcessor = this.mocks.StrictMock<IOrderProcessor>();

            var shippingDetails = new ShippingDetails();

            this.mocks.ReplayAll();

            var controller = GetCartController(productRepository, orderProcessor);

            // Act
            var result = controller.Checkout(cart, shippingDetails);

            // Assert
            Assert.AreEqual(result.ViewName, string.Empty);
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Checkout_WithInvalidShippingDetails_ExpectModelStateError()
        {
            // Arrange
            var cart = new Cart();
            cart.AddItem(new Product(), 1);

            var productRepository = this.mocks.StrictMock<IProductRepository>();
            var orderProcessor = this.mocks.StrictMock<IOrderProcessor>();

            var shippingDetails = new ShippingDetails();

            this.mocks.ReplayAll();

            var controller = GetCartController(productRepository, orderProcessor);
            controller.ModelState.AddModelError("error", "error");

            // Act
            var result = controller.Checkout(cart, shippingDetails);

            // Assert
            Assert.AreEqual(result.ViewName, string.Empty);
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Checkout_WithValidShippingDetailsAndEntryInCart_ExpectOrderProcessed()
        {
            // Arrange
            var cart = new Cart();
            cart.AddItem(new Product(), 1);

            var shippingDetails = new ShippingDetails();

            var productRepository = this.mocks.StrictMock<IProductRepository>();
            var orderProcessor = this.mocks.StrictMock<IOrderProcessor>();
            Expect.Call(() => orderProcessor.ProcessCart(cart, shippingDetails));

            this.mocks.ReplayAll();

            var controller = GetCartController(productRepository, orderProcessor);

            // Act
            var result = controller.Checkout(cart, shippingDetails);

            // Assert
            Assert.AreEqual(result.ViewName, "Completed");
            Assert.IsTrue(result.ViewData.ModelState.IsValid);
        }

        //// ----------------------------------------------------------------------------------------------------------
		 
        private CartController GetCartController(IProductRepository productRepository, IOrderProcessor orderProcessor)
        {
            return new CartController(productRepository, orderProcessor);
        }

        //// ----------------------------------------------------------------------------------------------------------
		 
    }
}
