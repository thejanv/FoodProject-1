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
        public ActionResult Index()
        {
            return View();
        }
    }
}