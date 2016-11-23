using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebStore.Infrastructure
{
    /// <summary>
    /// Class that holds all the consts we need (cache)
    /// </summary>
    public class Consts
    {
        public const string NewProductsCacheKey = "NewProductsCacheKey";
        public const string BestsellersCacheKey = "BestsellersCacheKey";
        public const string CategoriesCacheKey = "CategoriesCacheKey";
        public const string CartSessionKey = "CartSessionKey";
    }
}