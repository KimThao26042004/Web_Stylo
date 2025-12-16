using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApp_Stylo.Models;

namespace WebApp_Stylo.Controllers
{
    public class TaiKhoanController : Controller
    {
        private fashion_shopEntities db = new fashion_shopEntities();

        public ActionResult Index(string searchTerm, int page = 1)
        {

            int pageSize = 10;
            var query = db.TaiKhoans.AsQueryable();
            if (!String.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(a => a.TenDangNhap.ToLower().Contains(searchTerm.ToLower()));
            }

            int totalRecords = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var xaccounts = query
                .OrderBy(t => t.RoleId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(xaccounts);
        }

        // GET: TaiKhoan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaiKhoan/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaiKhoan taiKhoan)
        {
            if (!ModelState.IsValid)
                return View(taiKhoan);

            db.TaiKhoans.Add(taiKhoan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: TaiKhoan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
                return HttpNotFound();

            return View(taiKhoan);
        }

        // POST: TaiKhoan/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaiKhoan taiKhoan)
        {
            if (!ModelState.IsValid)
                return View(taiKhoan);

            var existing = db.TaiKhoans.Find(taiKhoan.TaiKhoanID);
            if (existing == null)
                return HttpNotFound();

            existing.TenDangNhap = taiKhoan.TenDangNhap;
            existing.Role = taiKhoan.Role;

            // chỉ update mật khẩu nếu có nhập
            if (!string.IsNullOrWhiteSpace(taiKhoan.MatKhauHash))
            {
                existing.MatKhauHash = taiKhoan.MatKhauHash.Trim();
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            db.TaiKhoans.Remove(taiKhoan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var taiKhoan = db.TaiKhoans
                .Include(t => t.Role)
                .Include(t => t.KhachHangs)
                .Include(t => t.NhanViens)
                .FirstOrDefault(t => t.TaiKhoanID == id);

            if (taiKhoan == null)
                return HttpNotFound();

            return View(taiKhoan);
        }

    }

}