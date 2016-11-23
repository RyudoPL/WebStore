using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebStore.DAL;

namespace WebStore.Controllers
{
    public class CategoryController : Controller
    {
        private ProductsContext db = new ProductsContext();
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets list of all products from selected category
        /// </summary>
        /// <param name="categoryName">Name of the category from which we want to see products</param>
        /// <param name="searchQuery">Product name or merchant we are looking for</param>
        /// <returns></returns>
        public ActionResult List(string categoryName, string searchQuery = null)
        {
            var category = db.Category.Include("Products").Where(c => c.CategoryName.ToLower() == categoryName.ToLower()).Single();
            var products = category.Products.Where(a => (searchQuery == null ||
            a.ProductName.ToLower().Contains(searchQuery.ToLower()) || 
            a.ProductMerchant.ToLower().Contains(searchQuery.ToLower())) 
            && !a.Hidden);

            if(Request.IsAjaxRequest())
            {
                return PartialView("_ProductList", products);
            }

            return View(products);
        }

        /// <summary>
        /// Returns details of the product we chose
        /// </summary>
        /// <param name="id">Id of the product which we want to check</param>
        /// <returns>Product details view</returns>
        public ActionResult Details(int id)
        {
            var product = db.Product.Find(id);
            return View(product);
        }

        /// <summary>
        /// Gets list of all available categories - cached
        /// </summary>
        /// <returns>Partial view with all available categories</returns>
        [ChildActionOnly]
        [OutputCache(Duration = 60000)]
        public ActionResult CategoryMenu()
        {
            var categories = db.Category.ToList();
            return PartialView("_CategoryMenu", categories);
        }

        /// <summary>
        /// Filter products by term
        /// </summary>
        /// <param name="term">Term which we want to use to filter products</param>
        /// <returns>List of products that matched the term</returns>
        public ActionResult ProductsFilter(string term)
        {
            var products = db.Product.Where(a => !a.Hidden && a.ProductName.ToLower().Contains(term.ToLower())).Take(5).Select(a => new { label = a.ProductName });
            return Json(products, JsonRequestBehavior.AllowGet);
        }
    }
}