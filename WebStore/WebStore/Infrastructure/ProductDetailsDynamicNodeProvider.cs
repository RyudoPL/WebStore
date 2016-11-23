using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcSiteMapProvider;
using WebStore.DAL;
using WebStore.Models;

namespace WebStore.Infrastructure
{
    public class ProductDetailsDynamicNodeProvider : DynamicNodeProviderBase
    {
        private ProductsContext db = new ProductsContext();

        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode nodes)
        {
            var returnValues = new List<DynamicNode>();

            foreach (Product product in db.Product)
            {
                DynamicNode node = new DynamicNode();
                node.Title = product.ProductName;
                node.Key = "Product_" + product.ProductId;
                node.ParentKey = "Category_" + product.CategoryId;
                node.RouteValues.Add("id", product.ProductId);
                returnValues.Add(node); 
            }
            return returnValues;
        }
    }
}