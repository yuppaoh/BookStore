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
            TempData["login_id"] = ViewBag.Msg;
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
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.ManufactureId = new SelectList(db.Manufactures, "ManufactureId", "ManufactureName", product.ManufactureId);
            ViewBag.Product = product;
            return View(product);
        }

        public ActionResult ProductOrder(string id)
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["LoginUser"])))
            {
               
                return RedirectToAction("Product");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaymentId = new SelectList(db.Payments, "PaymentId", "PaymentName");
            ViewBag.paymentlist =  db.Payments.ToList();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductOrder(int Distance, int PaymentId, string ProductId, int Quantity, double UnitPrice)
        {
            if (ModelState.IsValid)
            {
                Order order = new Order();
                order.OrderId = "1".PadLeft(8, '0');
                order.CustomerId = Convert.ToString(Session["LoginUser"]);
                order.OrderDate = DateTime.Now;
                order.PlaceOfDelivery = "";
                order.Distance = Distance;
                order.PaymentId = PaymentId;
                db.Orders.Add(order);

                //
                //(DateTime.Now - order.OrderDate).Value.Hours ;


                OrderDetail orderDetail = new OrderDetail();
                orderDetail.Order = order;
                orderDetail.ProductId = ProductId;
                orderDetail.Quantity = Quantity;
                orderDetail.UnitPrice = UnitPrice;
                db.OrderDetails.Add(orderDetail);


                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();

            //ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", order.CustomerId);
            //ViewBag.PaymentId = new SelectList(db.Payments, "PaymentId", "PaymentName", order.PaymentId);
            //return View(order);
        }




        // Login
        // -----------------------------------------------------
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            using (BookStoreEntities context = new BookStoreEntities())
            {
                // Cach 1: Viet theo dang Method
                var objEmployeeLogin = context.Customers
                    .Where(p => p.CustomerId == login && p.CustomerPassword == password)
                    .FirstOrDefault();

                if (objEmployeeLogin == null)
                {
                    TempData["login_oldValue"] = login;
                    TempData["password_oldValue"] = password;

                    //return "Khong hop le";
                    // validate login
                    if (String.IsNullOrEmpty(login))
                    {
                        TempData["msgLoi_Login"] = "Please, enter Username!";
                        return View("~/Views/Home/Login.cshtml");
                    }
                    else if (login.Length < 3)
                    {
                        TempData["msgLoi_Login"] = "Username must be >= 3 characters.";
                        return View("~/Views/Home/Login.cshtml");
                    }
                    else
                    {
                        TempData["msgLoi_Login"] = String.Empty;
                    }

                    // validate password
                    if (String.IsNullOrEmpty(password) || password.Length < 3)
                    {
                        TempData["msgLoi_Password"] = "Please, enter password >=3 characters.";
                        return View("~/Views/Home/Login.cshtml");
                    }
                    else
                    {
                        TempData["msgLoi_Password"] = String.Empty;
                    }

                    TempData["msgLoi"] = "Login fail. Check your username and password agian please!";
                    return View("~/Views/Home/Login.cshtml");
                    //return String.Format("Khong hop le {0} {1}", objEmployeeLogin.EmpName, objEmployeeLogin.EmpRole);
                }
                else
                {
                    //return String.Format("Xin chao anh {0} {1}", objEmployeeLogin.EmpName, objEmployeeLogin.EmpRole);
                    TempData["msgLoi"] = String.Empty;
                    //TempData["username"] = login;
                    Session["LoginUser"] = login;

                    //return View();
                    return RedirectToAction("/Product");
                    //return View("~/Views/Home/Product.cshtml");
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Product");
        }

        // -----------------------------------------------------

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