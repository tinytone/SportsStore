using System.Linq;
using System.Web.Mvc;

using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        //// ----------------------------------------------------------------------------------------------------------

        private readonly IProductRepository repository;

        //// ----------------------------------------------------------------------------------------------------------

        public int PageSize = 4;

        //// ----------------------------------------------------------------------------------------------------------

        public ProductController(IProductRepository productRepository)
        {
            this.repository = productRepository;
        }

        //// ----------------------------------------------------------------------------------------------------------
		 
        public ViewResult List(string category, int page = 1)
        {
            var pagedProducts = this.repository.Products
                                    .Where(p => category == null || p.Category == category)
                                    .OrderBy(p => p.ProductId)
                                    .Skip((page - 1) * this.PageSize)
                                    .Take(this.PageSize);

            var totalItems = category == null ? this.repository.Products.Count() : repository.Products.Count(p => p.Category == category);

            var pagingInfo = new PagingInfo
                                 {
                                     CurrentPage = page, 
                                     ItemsPerPage = PageSize, 
                                     TotalItems = totalItems
                                 };

            var model = new ProductListViewModel
                            {
                                Products = pagedProducts,
                                PagingInfo = pagingInfo,
                                CurrentCategory = category
                            };
            
            return View(model);
        }
    }
}