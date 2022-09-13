using FoodManagement.Models;
using FoodManagement.PayPal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;
//C:\Users\Aashif Ameer\Source\Repos\FoodProject\FoodManagement\App_Data\
namespace FoodManagement.Controllers
{
    public class HomeController : Controller
    {
        Random r = new Random();

        // GET: Home
        FoodManagementEntities food = new FoodManagementEntities();

        //=========================For Admin Page=============Start==========================
        [HttpGet]
        public ActionResult AdminAdd()
        {
            var foodIdList = food.FOOD_ITEM.ToList();
            Session["foodid"] = new SelectList(foodIdList,"FOODID","NAME");
            return View();
        }

        // Admin add  post
        [HttpPost]
        public ActionResult AdminAdd(FOOD_TYPE product)
        {


            SaveImage(product);
            food.FOOD_TYPE.Add(product);
            food.SaveChanges();
            return RedirectToAction("Display");
        }

        // Admin update get
        public ActionResult AdminUpdate(int id)
        {
            var foodIdList = food.FOOD_ITEM.ToList();
            Session["foodid"] = new SelectList(foodIdList, "FOODID", "NAME");
            var data = food.FOOD_TYPE.Where(x => x.TYPEID == id).FirstOrDefault();
            return View(data);
        }

        // Admin update post
        [HttpPost]
        public ActionResult AdminUpdate(FOOD_TYPE product, int id)
        {
            var data = food.FOOD_TYPE.Where(x => x.TYPEID == id).FirstOrDefault();
            if (data != null)
            {
                data.FOODID = product.FOODID;
                data.NAME = product.NAME;
                data.PRICE = product.PRICE;
                data.QUANTITY = product.QUANTITY;
                if (product.ImageFile != null)
                {
                    PicDelete(data);
                    SaveImage(product, data);
                }
                food.SaveChanges();
                return RedirectToAction("Display");
            }
            else
            {
                return View();
            }
        }

        // delete photo in product folder
        private void PicDelete(FOOD_TYPE data)
        {
            var t = Path.Combine(Server.MapPath(data.IMGPATH));
            FileInfo fi = new FileInfo(t);
            if (fi.Exists)
                fi.Delete();
        }

        // save image to product folder
        private void SaveImage(FOOD_TYPE newData, FOOD_TYPE prewData = null)
        {
            prewData = prewData ?? newData;
            string fileName = r.Next().ToString();
            string extension = Path.GetExtension(newData.ImageFile.FileName);
            fileName = fileName + extension;
            prewData.IMGPATH = "~/Content/Product/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Content/Product/"), fileName);
            newData.ImageFile.SaveAs(fileName);
        }

        // Delete Get
        public ActionResult AdminDelete(int? id)
        {
            var data = food.FOOD_TYPE.Where(x => x.TYPEID == id).FirstOrDefault();
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        // Detele Post
        [HttpPost, ActionName("AdminDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var data = food.FOOD_TYPE.Where(x => x.TYPEID == id).FirstOrDefault();
            PicDelete(data);
            food.FOOD_TYPE.Remove(data);
            food.SaveChanges();
            return RedirectToAction("Display");
        }

        public ActionResult Display()
        {
            ViewData["data"] = food.PAIDITEMS.ToList();
            return View(food.FOOD_TYPE.ToList());
        }

        public ActionResult AdminLogin()
        {
            return View();
        }

        public ActionResult AdminAuthorize()
        {

            if (Request.Form["user_id"].Equals("F00D123") && Request.Form["password"].Equals("F00DTrain@123"))
            {

                return RedirectToAction("Display");
            }
            else
            {
                ViewBag.msg = "Please provide correct Admin id and Password";
                return View("AdminLogin");
            }

        }
        //=========================END============================================================================
        //========================User View Page Starts===========================================================
        public ActionResult Front()
        {
            Session.Clear();
            return View();
        }
        // Adding users 
        public ActionResult Index()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(USER_REGISTRATION user, string repassword)
        {
            if (ModelState.IsValid && repassword == user.PASSWORD)
            {
                food.USER_REGISTRATION.Add(user);
                food.SaveChanges();
                return RedirectToAction("Login");
            }
            else if (user.PASSWORD != repassword)
            {
                ViewBag.msg = "Password and re-type password are not matched";
                return View(user);
            }
            else
            {
                return View(user);
            }
        }

        // [Authorize]
        //Displaying all the products 
        public new ActionResult Content(string name)
        {
            if (name != null)
            {
                var item = from v in food.FOOD_TYPE
                           where v.NAME.StartsWith(name)
                           select v;
                return View(item);
            }
            else
            {
                return View(food.FOOD_TYPE.ToList());
            }
        }

        //[ValidateAntiForgeryToken]
        public ActionResult Login()
        {

            return View();
        }
        
        public ActionResult Authenticate()
        {
                var data = food.USER_REGISTRATION.ToList();


            foreach (USER_REGISTRATION item in data)
            {

                if (Request.Form["email"].Equals(item.EMAIL) && Request.Form["password"].Equals(item.PASSWORD))
                {
                    Session["user"] = item;
                    Session["flag"] = true;
                    return RedirectToAction("Content");
                }

            }



            ViewBag.msg = "Please provide correct E-Mail Id and Password";
                    return View("Login");
        }
        //Updating 
        public ActionResult UserUpdate()
        {
            var data = Session["user"];
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserUpdate(USER_REGISTRATION user,string repassword)
        {
            var id = (Session["user"] as USER_REGISTRATION).USERID;
            
            var data = food.USER_REGISTRATION.Find(id);
            if (ModelState.IsValid && data != null && user.PASSWORD == repassword)
            {
                data.NAME = user.NAME;
                data.EMAIL = user.EMAIL;
                data.MOBILE = user.MOBILE;
                data.PASSWORD = user.PASSWORD;
                data.ADDRESS = user.ADDRESS;

                food.SaveChanges();
                return RedirectToAction("Content");
            }
            else if (user.PASSWORD != repassword)
            {
                ViewBag.msg = "Password and re-type password are not matched";
                return View(user);
            }
            else
            {
                return View();
            }
        }
        //===========Add to Cart and Check out=============================================================


        //======================================================================
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
        void cartCall()
        {
            if ((bool)Session["flag"])
            {
                var userID = ((Session["user"] as USER_REGISTRATION).USERID);
                int foodCount = food.FOOD_TYPE.Count();
                for (var i = 1; i <= foodCount; i++)
                {
                    var userCart = food.ADDTOCARTs.Where(x => (x.USERID == userID) &&
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
        }

        public ActionResult OrderNow(int id)
        {
            var item = food.FOOD_TYPE.Find(id);
            cartCall();

            


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
            var item = food.FOOD_TYPE.Find(id);
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
                var userCart = food.ADDTOCARTs.Where(x => (x.USERID == userID) &&
                                                (x.TYPEID == i + 1)).FirstOrDefault();
                if (userCart == null)
                {
                    food.ADDTOCARTs.Add(new ADDTOCART { USERID = userID, TYPEID = item.Type.TYPEID, NAME = item.Type.NAME, QUANTITY = item.Quantity, PRICE = (item.Type.PRICE*item.Quantity)});
                }
                else
                {
                    userCart.QUANTITY -= item.Quantity;
                }
            }
            food.SaveChanges();
            return View();
        }

        public ActionResult Cart()
        {
            cartCall();
            return View(food.ADDTOCARTs.ToList());
        }
        //============= PAY PAL VERIFICATION =======================================

        public ActionResult PayPal()
        {
            return View();
        }
        public ActionResult Success()
        {
           
                List<Item> cart = (List<Item>)Session["cart"];
                var userID = ((Session["user"] as USER_REGISTRATION).USERID);
                foreach (var item in cart)
                {
                    food.PAIDITEMS.Add(new PAIDITEM { USERID = userID, TYPEID = item.Type.TYPEID, NAME = item.Type.NAME, QUANTITY = item.Quantity, PRICE = (item.Type.PRICE * item.Quantity) });
                }

            /*var count = from i in FOOD_TYPE where i*/

            foreach (var item in cart)
            {
                var userCart = food.ADDTOCARTs.Where(x => (x.USERID == userID) &&
                                             (x.TYPEID == item.Type.TYPEID)).FirstOrDefault();
                if (userCart != null)
                {
                    food.ADDTOCARTs.Remove(userCart);
                }
            }
            food.SaveChanges();
                Session["cart"] = null;

            /*ViewBag.result = PDTHolder.Success(Request.QueryString.Get("tx"));*/
            return RedirectToAction("Content");
        }

        public ActionResult PaidItemDelete(int? id)
        {
            var data = food.PAIDITEMS.Where(x => x.TYPEID == id).FirstOrDefault();
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        // Detele Post
        [HttpPost, ActionName("PaidItemDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult PaidItemDelete(int id)
        {
            var data = food.PAIDITEMS.Where(x => x.TYPEID == id).FirstOrDefault();
            
            food.PAIDITEMS.Remove(data);
            food.SaveChanges();
            return RedirectToAction("Display");
        }

    }
}