using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebStore.DAL;
using WebStore.Models;

namespace WebStore.Infrastructure
{
    public class CategoryDynamicNodeProvider : DynamicNodeProviderBase
    {
        private ProductsContext db = new ProductsContext();

        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode nodes)
        {
            var returnValues = new List<DynamicNode>();

            foreach (Category category in db.Category)
            {
                DynamicNode node = new DynamicNode();
                node.Title = category.CategoryName;
                node.Key = "Category_" + category.CategoryId;
                node.RouteValues.Add("CategoryName", category.CategoryName);
                returnValues.Add(node);
            }
            return returnValues;
        }
    }
}