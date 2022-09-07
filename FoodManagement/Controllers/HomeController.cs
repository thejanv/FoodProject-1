using FoodManagement.Models;
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
            return View();
        }
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
        [ValidateAntiForgeryToken]
        public ActionResult Authenticate()
        {
            if (ModelState.IsValid)
            {
                var data = food.USER_REGISTRATION.ToList();
                int count = 0;

                foreach (USER_REGISTRATION item in data)
                {

                    if (Request.Form["email"].Equals(item.EMAIL) && Request.Form["password"].Equals(item.PASSWORD))
                    {

                        count++;
                    }

                }
                if (count > 0)
                {
                    return RedirectToAction("Content");
                }
                else
                {
                    ViewBag.msg = "Please provide correct E-Mail and Password";
                    return View("Login");
                }

            }
            else
            {
                ViewBag.msg = "Please provide correct E-Mail and Password";
                return View("Login");
            }
        }
        //===========Add to Cart and Check out=============================================================
       /* public ActionResult AddToCart()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddToCart(int id,int quantity)
        {
            var data = food.FOOD_TYPE.Find(id);
            int addId = data.TYPEID;
            
            string name = data.NAME;
            int totalPrice = data.PRICE * quantity;

            if (data != null)
            {
                food.ADDTOCARTs.Add(new ADDTOCART (addId,name,quantity,totalPrice));
                food.SaveChanges();
                return RedirectToAction("AddToCart");
            }
            else
            {
                return View();
            }

           
            
        }*/
    }
}