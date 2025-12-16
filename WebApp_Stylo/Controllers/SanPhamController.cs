using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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

            var query = db.SanPhams
                .Include(s => s.DanhMuc)
                .Include(s => s.ThuongHieu)
                .Include(s => s.AnhSanPhams)  
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(s => s.TenSanPham.Contains(searchTerm));

            int totalRecords = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var products = query
                .OrderBy(s => s.SanPhamID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.SearchTerm = searchTerm;

            return View(products);
        }



        // GET: SanPham/Create
        public ActionResult Create()
        {
            ViewBag.DanhMucID = new SelectList(db.DanhMucs, "DanhMucID", "Ten");
            ViewBag.ThuongHieuID = new SelectList(db.ThuongHieux, "ThuongHieuID", "Ten");
            return View();
        }


        // POST: SanPham/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
    SanPham sanPham,
    IEnumerable<HttpPostedFileBase> images)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DanhMucID = new SelectList(db.DanhMucs, "DanhMucID", "Ten", sanPham.DanhMucID);
                ViewBag.ThuongHieuID = new SelectList(db.ThuongHieux, "ThuongHieuID", "Ten", sanPham.ThuongHieuID);
                return View(sanPham);
            }

            // 1️⃣ Lưu sản phẩm trước để có SanPhamID
            db.SanPhams.Add(sanPham);
            db.SaveChanges();

            // 2️⃣ Tạo thư mục ảnh theo SanPhamID
            if (images != null)
            {
                string folderPath = Server.MapPath($"~/Content/images/{sanPham.SanPhamID}");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                int index = 1;
                foreach (var file in images)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = $"{sanPham.SanPhamID}_{index}{Path.GetExtension(file.FileName)}";
                        string fullPath = Path.Combine(folderPath, fileName);
                        file.SaveAs(fullPath);

                        // 3️⃣ Lưu DB AnhSanPham
                        db.AnhSanPhams.Add(new AnhSanPham
                        {
                            SanPhamID = sanPham.SanPhamID,
                            URL = $"Content/images/{sanPham.SanPhamID}/{fileName}",
                            IsPrimary = (index == 1)
                        });

                        index++;
                    }
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        // GET: SanPham/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
                return HttpNotFound();

            return View(sanPham);
        }

        // POST: SanPham/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SanPham sanPham)
        {
            if (!ModelState.IsValid)
                return View(sanPham);

            var existing = db.SanPhams.Find(sanPham.SanPhamID);
            if (existing == null)
                return HttpNotFound();

            // 👉 cập nhật thủ công
            existing.TenSanPham = sanPham.TenSanPham;
            //existing.Gia = sanPham.Gia;
            existing.MoTa = sanPham.MoTa;
            existing.DanhMucID = sanPham.DanhMucID;
            existing.ThuongHieuID = sanPham.ThuongHieuID;
            //existing.SoLuong = sanPham.SoLuong;
            //existing.TrangThai = sanPham.TrangThai;

            db.SaveChanges();
            return RedirectToAction("Index");
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
