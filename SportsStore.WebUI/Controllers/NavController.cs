using System.Linq;
using System.Web.Mvc;

using SportsStore.Domain.Abstract;

namespace SportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        //// ----------------------------------------------------------------------------------------------------------

        private readonly IProductRepository repository;

        //// ----------------------------------------------------------------------------------------------------------

        public NavController(IProductRepository productRepository)
        {
            this.repository = productRepository;
        }

        //// ----------------------------------------------------------------------------------------------------------
		 
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            var categories = this.repository.Products
                                            .Select(x => x.Category)
                                            .Distinct()
                                            .OrderBy(x => x);

            return PartialView(categories);
        }

        //// ----------------------------------------------------------------------------------------------------------
    }
}