using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Moq;

using Ninject;

using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        //// ----------------------------------------------------------------------------------------------------------

        private IKernel ninjectKernel;

        //// ----------------------------------------------------------------------------------------------------------

        public NinjectControllerFactory()
        {
            this.ninjectKernel = new StandardKernel();
            AddBindings();
        }

        //// ----------------------------------------------------------------------------------------------------------

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)this.ninjectKernel.Get(controllerType);
        }

        //// ----------------------------------------------------------------------------------------------------------
		 
        private void AddBindings()
        {
            /*
            var mockProducts = new List<Product>
                                   {
                                       new Product { Name = "Football", Price = 25 },
                                       new Product { Name = "Surf Board", Price = 179 },
                                       new Product { Name = "Running Shoes", Price = 95 },
                                   }.AsQueryable();

            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(mockProducts);

            this.ninjectKernel.Bind<IProductRepository>().ToConstant(mock.Object);
            */

            var emailSettings = new EmailSettings { WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false") };
            
            this.ninjectKernel.Bind<IProductRepository>().To<EFProductRepository>();
            this.ninjectKernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings", emailSettings);
        }

        //// ----------------------------------------------------------------------------------------------------------
    }
}