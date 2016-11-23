using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebStore.App_Start;
using WebStore.DAL;
using WebStore.Infrastructure;
using WebStore.Models;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        //reference to our CartManager script
        private CartManager cartManager;

        //reference to ISessionManager interface
        private ISessionManager sessionManager { get; set; }

        //reference to our context
        private ProductsContext db;

        //Constructor
        public CartController()
        {
            //Create references
            db = new ProductsContext();
            sessionManager = new SessionManager();
            cartManager = new CartManager(sessionManager, db);
        }

        // GET: Cart
        public ActionResult Index()
        {
            var cartItems = cartManager.GetCart();
            var totalValue = cartManager.GetCartValue();
            CartViewModel cartViewModel = new CartViewModel()
            {
                CartItems = cartItems,
                TotalValue = totalValue
            };
            return View(cartViewModel);
        }

        /// <summary>
        /// Function that adds a product to our cart
        /// </summary>
        /// <param name="id">product id</param>
        /// <returns>Adds product to the cart</returns>
        public ActionResult AddToCart(int id)
        {
            cartManager.AddToCart(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Get number of all elements in the cart
        /// </summary>
        /// <returns>Integer which stores info how many items there are in the cart</returns>
        public int GetCartItemsNumber()
        {
            return cartManager.GetTotalCartItemNumber();
        }

        /// <summary>
        /// Delete item from a cart
        /// </summary>
        /// <param name="productId">Id of the product that needs to be deleted</param>
        /// <returns></returns>
        public ActionResult DeleteFromCart(int productId)
        {
            int itemNumber = cartManager.DeleteFromCart(productId);
            int cartItemNumber = cartManager.GetTotalCartItemNumber();
            decimal cartValue = cartManager.GetCartValue();

            var result = new DeletingFromCartViewModel()
            {
                DeleteItemId = productId,
                ProductAmountToDelete = itemNumber,
                CartTotalValue = cartValue,
                CartItemNumber = cartItemNumber,
            };
            return Json(result);
        }

        /// <summary>
        /// Function that is called when checkout button is pressed
        /// </summary>
        /// <returns>User data when client is logged in, else Login View</returns>
        public async Task<ActionResult> Checkout()
        {
            if (Request.IsAuthenticated)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                var order = new Order
                {
                    CustomerName = user.UserData.Name,
                    CustomerSurname = user.UserData.Surname,
                    Adress = user.UserData.Address,
                    City = user.UserData.City,
                    ZipCode = user.UserData.ZipCode,
                    Email = user.UserData.Email,
                    PhoneNumber = user.UserData.PhoneNumber
                };

                return View(order);
            }
            else
            {
                return RedirectToAction("Login","Account", new { returnUrl = Url.Action("Checkout","Cart")});
            }
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(Order orderDetails)
        {
            if (ModelState.IsValid)
            {
                //get id of the user who is currently logged in
                var userId = User.Identity.GetUserId();

                //create new object (of type Order) based on elements which it has in the cart
                var newOrder = cartManager.MakeOrder(orderDetails, userId);

                //Update customer's details
                var user = await UserManager.FindByIdAsync(userId);
                TryUpdateModel(user.UserData);
                await UserManager.UpdateAsync(user);

                cartManager.EmptyCart();

                return RedirectToAction("ConfirmOrder");
            }
            else
            {
                return View(orderDetails);
            }
        }

        public ActionResult ConfirmOrder()
        {
            return View();
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                _userManager = value;
            }
        }
    }
}