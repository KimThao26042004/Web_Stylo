using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp_Stylo.Models;

namespace WebApp_Stylo.Controllers
{
    public class SanPhamController : Controller
    {
        private fashion_shopEntities db = new fashion_shopEntities();

        // GET: SanPham
        public ActionResult Index()
        {
            // Lấy tất cả sản phẩm và bao gồm thông tin DanhMuc và ThuongHieu
            var products = db.SanPhams.Include(s => s.DanhMuc).Include(s => s.ThuongHieu).ToList();
            return View(products);
        }

        // GET: SanPham/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SanPham/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sanPham);
        }

        // GET: SanPham/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPham/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sanPham);
        }

        // GET: SanPham/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: SanPham/Search (Chức năng tìm kiếm)
        public ActionResult Search(string searchTerm)
        {
            var products = from p in db.SanPhams
                           select p;

            if (!String.IsNullOrEmpty(searchTerm))
            {
                // Lọc các sản phẩm theo tên sản phẩm chứa từ khóa tìm kiếm (case-insensitive)
                products = products.Where(p => p.TenSanPham.Contains(searchTerm));
            }

            return View("Index", products.ToList());  // Trả về view Index với kết quả tìm kiếm
        }
    }
}
