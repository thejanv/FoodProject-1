using FoodManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodManagement.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        FoodManagementEntities food = new FoodManagementEntities();
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
            else if(user.PASSWORD != repassword)
            {
                ViewBag.msg = "Password and re-type password are not matched";
                return View(user);
            }
            else
            {
                return View(user);
            }
        }
        [Authorize]
        public ActionResult Content()
        {

            return View();
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
                    return View("Content");
                }
                else
                {
                    ViewBag.msg = "Please provide correct EMail and Password";
                    return View("Login");
                }

            }
            else
            {
                ViewBag.msg = "Please provide correct EMail and Password";
                return View("Login");
            }
        }
    }
}