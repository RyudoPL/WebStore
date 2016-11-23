using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebStore.App_Start;
using WebStore.DAL;
using WebStore.Models;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ProductsContext db = new ProductsContext();

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            Error
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set
            {
                _userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            if(TempData["ViewData"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["ViewData"];
            }

            if(User.IsInRole("Admin"))
            {
                ViewBag.UserIsAdmin = true;
            }
            else
            {
                ViewBag.UserIsAdmin = false;
            }

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if(user == null)
            {
                return View("Error");
            }

            var model = new ManageCredentialsViewModel
            {
                Message = message,
                UserData = user.UserData
            };
            return View(model);
        }

        /// <summary>
        /// Gets user data which customer can change
        /// </summary>
        /// <param name="userData">Data of the user which is currently logged in</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeProfile([Bind(Prefix = "UserData")]UserData userData)
        {
            if(ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                user.UserData = userData;
                var result = await UserManager.UpdateAsync(user);

                AddErrors(result);
            }

            if(!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword([Bind(Prefix = "ChangePasswordViewModel")]ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);

            if(!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            var message = ManageMessageId.ChangePasswordSuccess;

            return RedirectToAction("Index", new { Message = message });
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("password-error", error);
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        /// <summary>
        /// Gets a list of all orders (admin) or selected user's orders (user)
        /// </summary>
        /// <returns>List of all orders a customer made</returns>
        public ActionResult OrdersList()
        {
            bool isAdmin = User.IsInRole("Admin");
            ViewBag.UserIsAdmin = isAdmin;
            IEnumerable<Order> userOrders;

            //Shows all orders if user is an admin
            if (isAdmin)
            {
                userOrders = db.Order.Include("OrderDetails").OrderByDescending(o => o.DateAdded).ToArray();
            }
            //shows customer's orders if user is not an admin
            else
            {
                var userId = User.Identity.GetUserId();
                userOrders = db.Order.Where(o => o.UserId == userId).Include("OrderDetails").OrderByDescending(o => o.DateAdded).ToArray();
            }

            return View(userOrders);
        }

        /// <summary>
        /// Changes selected order status
        /// </summary>
        /// <param name="order">Order the admin want to change</param>
        /// <returns>New order status</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public OrderState ChangeOrderStatus(Order order)
        {
            Order orderToModify = db.Order.Find(order.OrderId);

            orderToModify.OrderState = order.OrderState;
            db.SaveChanges();

            return order.OrderState;
        }
    }
}