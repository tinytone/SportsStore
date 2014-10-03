using System;
using System.Web.Mvc;

using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Binders
{
    // Note: this class needs to be added to the Binders collection upon the Application Startup
    public class CartModelBinder : IModelBinder
    {
        //// ----------------------------------------------------------------------------------------------------------

        private const string SessionKey = "Cart";

        //// ----------------------------------------------------------------------------------------------------------
		 
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // Get the Cart from the Session
            var cart = (Cart)controllerContext.HttpContext.Session[SessionKey];

            // Create the Cart if there wasn't one in the Session Data
            if (cart == null)
            {
                cart = new Cart();

                controllerContext.HttpContext.Session[SessionKey] = cart;
            }

            return cart;
        }

        //// ----------------------------------------------------------------------------------------------------------
    }
}