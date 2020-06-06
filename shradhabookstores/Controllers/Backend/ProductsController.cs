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

namespace shradhabookstores.Controllers.Backend
{
    public class ProductsController : Controller
    {
        private BookStoreEntities db = new BookStoreEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.Manufacture);
            return View("~/Views/Backend/Products/Index.cshtml", products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(string id)
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
            return View("~/Views/Backend/Products/Details.cshtml", product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.ManufactureId = new SelectList(db.Manufactures, "ManufactureId", "ManufactureName");
            return View("~/Views/Backend/Products/Create.cshtml");
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include =
            "ProductId,CategoryId,ProductImage,ProductName,ProductType,ManufactureId,Author," +
            "ReleaseDate,UnitPrice,Quantity,Note")]
            Product product, HttpPostedFileBase ProductImage)
        {
            if (ModelState.IsValid)
            {
                //Xử lý file: lưu file vào thư mục UploadedFiles
                string _FileName = "";
                string uploadFolderPath = Server.MapPath("~/UploadedFiles");

                // di chuyển file vào thư mục mong muốn
                if (ProductImage != null && ProductImage.ContentLength > 0)
                {
                    _FileName = Path.GetFileName(ProductImage.FileName);
                    string _FileNameExtension = Path.GetExtension(ProductImage.FileName);
                    if ((_FileNameExtension == ".png" || _FileNameExtension == ".jpg"
                        || _FileNameExtension == ".jpeg" || _FileNameExtension == ".svg"
                        ) == false)
                    {
                        return View(String.Format("File extension {0} is not acepted, please check again!", _FileNameExtension));
                    }

                    // Upload file len folder UploadedFiles
                    // ------------------------------------
                    if (Directory.Exists(uploadFolderPath) == false)
                    {
                        Directory.CreateDirectory(uploadFolderPath);
                    }

                    string _Path = Path.Combine(uploadFolderPath, _FileName);
                    ProductImage.SaveAs(_Path);

                    // Lưu file vao database
                    product.ProductImage = _FileName;

                }

                //lưu dữ liệu
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.ManufactureId = new SelectList(db.Manufactures, "ManufactureId", "ManufactureName", product.ManufactureId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(string id)
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
            return View("~/Views/Backend/Products/Edit.cshtml", product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "ProductId,CategoryId,ProductImage,ProductName,ProductType,ManufactureId,Author,ReleaseDate," +
            "UnitPrice,Quantity,Note")] Product product, string image_oldFile, HttpPostedFileBase ProductImage)
        {
            if (ModelState.IsValid)
            {
                string uploadFolderPath = Server.MapPath("~/UploadedFiles");

                if (ProductImage == null) // Nếu không cập nhật ảnh mới (không up file)
                {
                    product.ProductImage = image_oldFile;  // Giữ nguyên ảnh củ
                }
                else // Nếu chọn file ảnh mới để cập nhật ảnh
                {
                    // 1. Xóa file ảnh củ
                    string oldPathFile = Path.Combine(uploadFolderPath, (product.ProductImage == null ? "" : product.ProductImage));
                    if (System.IO.File.Exists(oldPathFile))
                    {
                        System.IO.File.Delete(oldPathFile);
                    }

                    // 2. upload ảnh mới
                    string _FileName = "";

                    // di chuyển file vào thư mục mong muốn
                    if (ProductImage.ContentLength > 0)
                    {
                        _FileName = Path.GetFileName(ProductImage.FileName);
                        string _FileNameExtension = Path.GetExtension(ProductImage.FileName);
                        if ((_FileNameExtension == ".png" || _FileNameExtension == ".jpg"
                            || _FileNameExtension == ".jpeg" || _FileNameExtension == ".svg"
                            ) == false)
                        {
                            return View(String.Format("File extension {0} is not acepted, please check agian!", _FileNameExtension));
                        }

                        if (Directory.Exists(uploadFolderPath) == false)
                        {
                            Directory.CreateDirectory(uploadFolderPath);
                        }

                        string _Path = Path.Combine(uploadFolderPath, _FileName);
                        ProductImage.SaveAs(_Path);

                        // Lưu file vao database
                        product.ProductImage = _FileName;
                    }
                }


                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.ManufactureId = new SelectList(db.Manufactures, "ManufactureId", "ManufactureName", product.ManufactureId);

            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(string id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
