using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using shradhabookstores.EF;

namespace shradhabookstores.Controllers
{
    public class HomeController : Controller
    {
        private BookStoreEntities db = new BookStoreEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Product()
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.Manufacture);

            ViewBag.ListProducts = products;
            
            return View();
        }


        public ActionResult ProductDetail(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Ex()
        {
            ViewBag.Message = "Ex page.";

            return View();
        }


    }
}