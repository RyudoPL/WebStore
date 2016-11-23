using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebStore.DAL;
using WebStore.Models;

namespace WebStore.Infrastructure
{
    public class CartManager
    {
        private ProductsContext db;
        private ISessionManager session;

        public CartManager(ISessionManager session, ProductsContext db)
        {
            this.session = session;
            this.db = db;
        }

        /// <summary>
        /// Gets all items from the cart
        /// </summary>
        /// <returns>All items from the cart</returns>
        public List<CartItem> GetCart()
        {
            List<CartItem> cart;

            if(session.Get<List<CartItem>>(Consts.CartSessionKey) == null)
            {
                cart = new List<CartItem>();
            }
            else
            {
                cart = session.Get<List<CartItem>>(Consts.CartSessionKey) as List<CartItem>;
            }

            return cart;
        }

        /// <summary>
        /// Add product to the cart
        /// </summary>
        /// <param name="productId">Id of the product we are adding to the cart</param>
        public void AddToCart(int productId)
        {
            var cart = GetCart();
            var cartInfo = cart.Find(p => p.Product.ProductId == productId);

            if(cartInfo != null)
            {
                cartInfo.Amount++;
            }
            else
            {
                var productToAdd = db.Product.Where(p => p.ProductId == productId).SingleOrDefault();

                if(productToAdd != null)
                {
                    var newCartItem = new CartItem()
                    {
                        Product = productToAdd,
                        Amount = 1,
                        Value = productToAdd.ProductPrice
                    };
                    cart.Add(newCartItem);
                }
            }
            session.Set(Consts.CartSessionKey, cart);
        }

        /// <summary>
        /// Function that deletes an item from the cart
        /// </summary>
        /// <param name="productId">Id of the product we want to delete</param>
        /// <returns></returns>
        public int DeleteFromCart(int productId)
        {
            var cart = GetCart();
            var cartItem = cart.Find(p => p.Product.ProductId == productId);

            if(cartItem != null)
            {
                if(cartItem.Amount > 1)
                {
                    cartItem.Amount--;
                    return cartItem.Amount;
                }
                else
                {
                    cart.Remove(cartItem);
                }
            }
            return 0;
        }

        /// <summary>
        /// Function that returns total value of all item in the cart
        /// </summary>
        /// <returns>Total value of all items in the cart</returns>
        public decimal GetCartValue()
        {
            var cart = GetCart();
            return cart.Sum(p => (p.Amount * p.Product.ProductPrice));
        }

        /// <summary>
        /// Function that returns number of items in the cart 
        /// </summary>
        /// <returns>int - Number of items in the cart</returns>
        public int GetTotalCartItemNumber()
        {
            var cart = GetCart();
            int amount = cart.Sum(p => p.Amount);
            return amount;
        }

        /// <summary>
        /// Function that creates a new order,
        /// </summary>
        /// <param name="newOrder"></param>
        /// <param name="userId">Id of the user which is currently logged in</param>
        /// <returns>Order which client made</returns>
        public Order MakeOrder(Order newOrder, string userId)
        {
            var cart = GetCart();
            newOrder.DateAdded = DateTime.Now;
            newOrder.UserId = userId;

            db.Order.Add(newOrder);

            if(newOrder.OrderDetails == null)
            {
                newOrder.OrderDetails = new List<OrderDetail>();
            }

            decimal cartValue = 0;

            foreach(var cartItem in cart)
            {
                var newOrderItem = new OrderDetail()
                {
                    ProductId = cartItem.Product.ProductId,
                    Quantity = cartItem.Amount,
                    EndPrice = cartItem.Value
                };

                cartValue += (cartItem.Amount * cartItem.Product.ProductPrice);
                newOrder.OrderDetails.Add(newOrderItem);
            }

            newOrder.OrderValue = cartValue;
            db.SaveChanges();

            return newOrder;
        }

        /// <summary>
        /// Make the cart empty
        /// </summary>
        public void EmptyCart()
        {
            session.Set<List<CartItem>>(Consts.CartSessionKey, null);
        }
    }
}