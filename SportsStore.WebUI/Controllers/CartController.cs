using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;

using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        //// ----------------------------------------------------------------------------------------------------------

        private readonly IProductRepository repository;

        private readonly IOrderProcessor orderProcessor;

        //// ----------------------------------------------------------------------------------------------------------

        public CartController(IProductRepository productRepository, IOrderProcessor orderProcessor)
        {
            Contract.Requires<ArgumentNullException>(productRepository != null);
            Contract.Requires<ArgumentNullException>(orderProcessor != null);

            this.repository = productRepository;
            this.orderProcessor = orderProcessor;
        }

        //// ----------------------------------------------------------------------------------------------------------

        public ViewResult Index(Cart cart, string returnUrl)
        {
            var viewModel = new CartIndexViewModel
                                {
                                    Cart = cart, 
                                    ReturnUrl = returnUrl
                                };

            return View(viewModel);
        }

        //// ----------------------------------------------------------------------------------------------------------
		 
        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            var product = GetProduct(productId);

            if (product != null)
                cart.AddItem(product, 1);

            return RedirectToAction("Index", new { returnUrl });
        }

        //// ----------------------------------------------------------------------------------------------------------
		 
        private Product GetProduct(int productId)
        {
            return this.repository.Products.FirstOrDefault(p => p.ProductId == productId);
        }

        //// ----------------------------------------------------------------------------------------------------------

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            var product = GetProduct(productId);

            if (product != null)
                cart.RemoveLine(product);

            return RedirectToAction("Index", new { returnUrl });
        }

        //// ----------------------------------------------------------------------------------------------------------

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        //// ----------------------------------------------------------------------------------------------------------

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        //// ----------------------------------------------------------------------------------------------------------

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (!cart.Lines.Any())
                ModelState.AddModelError(string.Empty, "Sorry, your cart is empty!");

            if (ModelState.IsValid)
            {
                this.orderProcessor.ProcessCart(cart, shippingDetails);
                cart.Clear();

                return View("Completed");
            }

            return View(shippingDetails);
        }

        //// ----------------------------------------------------------------------------------------------------------
    }
}