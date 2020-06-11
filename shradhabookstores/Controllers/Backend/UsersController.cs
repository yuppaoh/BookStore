using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using shradhabookstores.EF;

namespace shradhabookstores.Controllers.Backend
{
    public class UsersController : Controller
    {
        private BookStoreEntities db = new BookStoreEntities();

        // Login
        // -----------------------------------------------------
        public ActionResult Login()
        {
            return View("~/Views/Backend/Users/Login.cshtml");
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            using (BookStoreEntities context = new BookStoreEntities())
            {
                // Cach 1: Viet theo dang Method
                var objEmployeeLogin = context.Users
                    .Where(p => p.UserName == login && p.UserPassword == password)
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
                        return View("~/Views/Backend/Users/Login.cshtml");
                    }
                    else if (login.Length < 3)
                    {
                        TempData["msgLoi_Login"] = "Username must be >= 3 characters.";
                        return View("~/Views/Backend/Users/Login.cshtml");
                    }
                    else
                    {
                        TempData["msgLoi_Login"] = String.Empty;
                    }

                    // validate password
                    if (String.IsNullOrEmpty(password) || password.Length < 3)
                    {
                        TempData["msgLoi_Password"] = "Please, enter password >=3 characters.";
                        return View("~/Views/Backend/Users/Login.cshtml");
                    }
                    else
                    {
                        TempData["msgLoi_Password"] = String.Empty;
                    }

                    TempData["msgLoi"] = "Login fail. Check your username and password agian please!";
                    return View("~/Views/Backend/Users/Login.cshtml");
                    //return String.Format("Khong hop le {0} {1}", objEmployeeLogin.EmpName, objEmployeeLogin.EmpRole);
                }
                else
                {
                    TempData["msgLoi"] = String.Empty;
                    //TempData["username"] = login;
                    Session["LoginUser"] = login;

                    //return View();
                    return RedirectToAction("../Products/Index");
                    //return View("~/Views/Backend/Products/Index.cshtml");
                }
            }
        }

        // GET: Users
        public ActionResult Index()
        {
            if (Session["LoginUser"] != null)
            {
                return View("~/Views/Backend/Users/Index.cshtml", db.Users.ToList());
            }
            return RedirectToAction("../Users/Login");
            
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Backend/Users/Details.cshtml", user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View("~/Views/Backend/Users/Create.cshtml");
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Email,UserName,UserPassword,PhoneNo,UserRole")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Backend/Users/Edit.cshtml", user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Email,UserName,UserPassword,PhoneNo,UserRole")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Backend/Users/Delete.cshtml", user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
