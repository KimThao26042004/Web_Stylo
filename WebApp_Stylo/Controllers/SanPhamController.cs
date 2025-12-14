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
        public ActionResult Index(string searchTerm, int page = 1)
        {
            int pageSize = 10;
            var query = db.SanPhams.Include(s => s.DanhMuc).Include(s => s.ThuongHieu).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(s => s.TenSanPham.Contains(searchTerm));
            }

            int totalRecords = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var products = db.SanPhams
                             .Where(s => string.IsNullOrEmpty(searchTerm) || s.TenSanPham.Contains(searchTerm))
                             .OrderBy(s => s.TenSanPham)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .Select(s => new { s.SanPhamID, s.TenSanPham, s.DanhMuc.Ten, s.ThuongHieu })  // Chọn cột cần thiết
                             .ToList();


            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

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
