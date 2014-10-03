using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Tests.Entities
{
    [TestClass]
    public class CartTest
    {
        //// ----------------------------------------------------------------------------------------------------------
		 
        [TestMethod]
        public void Constructor_Instantiate_ExpectInstanceWithDefaultData()
        {
            // Arrange
            
            // Act
            var cart = new Cart();

            // Assert
            Assert.IsNotNull(cart);
            Assert.IsNotNull(cart.Lines);
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void AddItem_AddSingleProduct_ExpectProductToBeAdded()
        {
            // Arrange
            var product = new Product { ProductId = 1, Name = "P1" };

            var cart = new Cart();

            // Act
            cart.AddItem(product, 1);

            // Assert
            var lines = cart.Lines.ToArray();
            Assert.AreEqual(lines.Length, 1);
            Assert.AreEqual(lines[0].Product, product);
        }


        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void AddItem_AddTwoProducts_ExpectTwoProductToBeAdded()
        {
            // Arrange
            var product1 = new Product { ProductId = 1, Name = "P1" };
            var product2 = new Product { ProductId = 2, Name = "P2" };

            var cart = new Cart();

            // Act
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);

            // Assert
            var lines = cart.Lines.ToArray();

            Assert.AreEqual(lines.Length, 2);
            Assert.AreEqual(lines[0].Product, product1);
            Assert.AreEqual(lines[1].Product, product2);
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void AddItem_AddProductToExistingProductInCart_ExpectQuantityOfExistingProductToBeIncremented()
        {
            // Arrange
            var product1 = new Product { ProductId = 1, Name = "P1" };
            var product2 = new Product { ProductId = 2, Name = "P2" };

            var cart = new Cart();

            // Act
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 10);

            // Assert
            var lines = cart.Lines.OrderBy(p => p.Product.ProductId).ToArray();

            Assert.AreEqual(lines.Length, 2);
            Assert.AreEqual(lines[0].Product, product1);
            Assert.AreEqual(lines[0].Quantity, 11);
            Assert.AreEqual(lines[1].Product, product2);
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RemoveLine_ProductExistsInCart_ExpectProductToBeRemoved()
        {
            // Arrange
            var product1 = new Product { ProductId = 1, Name = "P1" };
            var product2 = new Product { ProductId = 2, Name = "P2" };

            var cart = new Cart();
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 10);

            // Act
            cart.RemoveLine(product1);

            // Assert
            var lines = cart.Lines.OrderBy(p => p.Product.ProductId).ToArray();

            Assert.AreEqual(lines.Length, 1);
            Assert.AreEqual(lines[0].Product, product2);
            Assert.AreEqual(lines.Count(p => p.Product.ProductId == product1.ProductId), 0);
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ComputeTotalValue_On5Products_ExpectCorrectTotalValue()
        {
            // Arrange
            var product1 = new Product { ProductId = 1, Name = "P1", Price = 100M };
            var product2 = new Product { ProductId = 2, Name = "P2", Price = 50M };

            var cart = new Cart();
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 3);

            // Act
            var totalValue = cart.ComputeTotalValue();

            // Assert
            Assert.AreEqual(totalValue, 450);
        }

        //// ----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Clear_ProductsExist_ExpectNoProductsToExistInCart()
        {
            // Arrange
            var product1 = new Product { ProductId = 1, Name = "P1", Price = 100M };
            var product2 = new Product { ProductId = 2, Name = "P2", Price = 50M };

            var cart = new Cart();
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 3);

            // Act
            cart.Clear();

            // Assert
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        //// ----------------------------------------------------------------------------------------------------------
    }
}
