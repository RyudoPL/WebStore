using MvcSiteMapProvider.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebStore.DAL;
using WebStore.Infrastructure;
using WebStore.Models;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private ProductsContext db = new ProductsContext();
        // GET: Home

        //Main view of the application - get products and categories from cache 
        public ActionResult Index()
        {
            //Reference to our Cache interface
            ICacheProvider cache = new DefaultCacheProvider();

            List<Category> categories;

            if (cache.IsSet(Consts.CategoriesCacheKey))
            {
                categories = cache.Get(Consts.CategoriesCacheKey) as List<Category>;
            }
            else
            {
                categories = db.Category.ToList();
                cache.Set(Consts.CategoriesCacheKey, categories, 60);
            }

            List<Product> newProducts;

            if(cache.IsSet(Consts.NewProductsCacheKey))
            {
                newProducts = cache.Get(Consts.NewProductsCacheKey) as List<Product>;
            }
            else
            {
                newProducts = db.Product.Where(a => !a.Hidden).OrderByDescending(a => a.DateAdded).Take(3).ToList();
                cache.Set(Consts.NewProductsCacheKey, newProducts, 60);
            }

            List<Product> bestsellers;

            if (cache.IsSet(Consts.BestsellersCacheKey))
            {
                bestsellers = cache.Get(Consts.BestsellersCacheKey) as List<Product>;
            }
            else
            {
                bestsellers = db.Product.Where(a => !a.Hidden && a.Bestseller).OrderBy(a => Guid.NewGuid()).Take(5).ToList();
                cache.Set(Consts.BestsellersCacheKey, bestsellers, 60);
            }

            var viewModel = new HomeViewModel()
            {
                Categories = categories,
                NewProducts = newProducts,
                Bestsellers = bestsellers
                
            };
            return View(viewModel);
        }

        /// <summary>
        /// Gets view of selected static website
        /// </summary>
        /// <param name="name">Name of a static website</param>
        /// <returns>View of selected static website</returns>
        public ActionResult StaticWebsites(string name)
        {
            return View(name);
        }
    }
}