using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebStore.ViewModels
{
    public class DeletingFromCartViewModel
    {
        public decimal CartTotalValue { get; set; }
        public int CartItemNumber { get; set; }
        public int ProductAmountToDelete { get; set; }
        public int DeleteItemId { get; set; }
    }
}