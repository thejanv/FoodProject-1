/*using FoodManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace FoodManagement.Controllers
{
    public class CartController : Controller
    {
        private FoodManagementEntities db = new FoodManagementEntities();


        public ActionResult Index()
        {
            var aDDTOCARTs = db.ADDTOCARTs.Include(a => a.USER_REGISTRATION).Include(a => a.FOOD_TYPE);
            return View(aDDTOCARTs.ToList());
        }

        private int IsExistig(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            for (var n = 0; n < cart.Count; n++)
            {
                if (cart[n].Type.TYPEID.Equals(id))
                {
                    return n;
                }
            }
            return -1;
        }

        public ActionResult OrderNow(int id)
        {
            var item = db.FOOD_TYPE.Find(id);
            var userID = ((Session["user"] as USER_REGISTRATION).USERID);

            if ((bool)Session["flag"])
            {
                int foodCount = db.FOOD_TYPE.Count();
                for (var i = 1; i <= foodCount; i++)
                {
                    var userCart = db.ADDTOCARTs.Where(x => (x.USERID == userID) &&
                                                 (x.TYPEID == i)).FirstOrDefault();
                    if (userCart != null)
                    {
                        if (Session["cart"] != null)
                        {
                            List<Item> cart = (List<Item>)Session["cart"];
                            cart.Add(new Item(userCart.FOOD_TYPE, (int)userCart.QUANTITY));
                            Session["cart"] = cart;
                        }
                        else
                        {
                            List<Item> cart = new List<Item>();
                            cart.Add(new Item(userCart.FOOD_TYPE, (int)userCart.QUANTITY));
                            Session["cart"] = cart;
                        }
                    }
                }
                Session["flag"] = false;
            }


            if (item.QUANTITY != 0)
            {
                if (Session["cart"] == null)
                {
                    List<Item> cart = new List<Item>();
                    cart.Add(new Item(item, 1));
                    Session["cart"] = cart;
                }
                else
                {
                    List<Item> cart = (List<Item>)Session["cart"];
                    int index = IsExistig(id);
                    if (index.Equals(-1))
                    {
                        cart.Add(new Item(item, 1));
                    }
                    else
                    {
                        cart[index].Quantity++;
                    }

                    Session["cart"] = cart;
                }
            }

            return RedirectToAction("Content");

        }

        public ActionResult Delete(int id)
        {
            var item = db.FOOD_TYPE.Find(id);
            int index = IsExistig(id);
            List<Item> cart = (List<Item>)Session["cart"];
            cart[index].Quantity--;
            if (cart[index].Quantity.Equals(0))
            {
                cart.RemoveAt(index);
            }
            Session["cart"] = cart;
            return View("Cart");
        }

        public ActionResult Checkout()
        {
            List<Item> cart = (List<Item>)Session["cart"];
            var userID = ((Session["user"] as USER_REGISTRATION).USERID);

            for (int i = 0; i < cart.Count; i++)
            {
                var item = cart[i];
                var userCart = db.ADDTOCARTs.Where(x => (x.USERID == userID) &&
                                                (x.TYPEID == i + 1)).FirstOrDefault();
                if (userCart != null)
                {
                    userCart.QUANTITY = item.Quantity;
                }
                else
                {

                    userCart.QUANTITY -= item.Quantity;
                    db.ADDTOCARTs.Add(new ADDTOCART { USERID = userID, TYPEID = item.Type.TYPEID, NAME = item.Type.NAME, QUANTITY = item.Quantity, PRICE = item.Type.PRICE });
                }
            }
            db.SaveChanges();
            return View();
        }
    }
}*/