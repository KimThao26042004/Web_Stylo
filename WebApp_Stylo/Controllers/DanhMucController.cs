using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebApp_Stylo.Models;

namespace WebApp_Stylo.Controllers
{
    public class DanhMucController : Controller
    {
        private fashion_shopEntities db = new fashion_shopEntities();

        // GET: DanhMuc
        public ActionResult Index(string searchTerm, int page = 1)
        {
            int pageSize = 10;
            var query = db.DanhMucs.AsQueryable();

            if (!String.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.Ten.Contains(searchTerm));
            }



            int totalRecords = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var subCategories = query
                .OrderBy(t => t.Ten)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(subCategories);
        }

        // GET: DanhMuc/Create
        public ActionResult Create()
        {
            ViewBag.PhanLoaiID = new SelectList(db.PhanLoais, "PhanLoaiID", "Ten");
            return View();
        }

        // POST: DanhMuc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DanhMuc danhMuc)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PhanLoaiID = new SelectList(db.PhanLoais, "PhanLoaiID", "Ten", danhMuc.PhanLoaiID);
                return View(danhMuc);
            }

            db.DanhMucs.Add(danhMuc);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: DanhMuc/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
                return HttpNotFound();

            ViewBag.PhanLoaiID = new SelectList(
                db.PhanLoais.ToList(),  
                "PhanLoaiID",           
                "Ten",
                danhMuc.PhanLoaiID     
            );

            return View(danhMuc);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DanhMuc danhMuc)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PhanLoaiID = new SelectList(
                    db.PhanLoais, "PhanLoaiID", "Ten", danhMuc.PhanLoaiID);
                return View(danhMuc);
            }

            // Kiểm tra khóa ngoại
            if (!db.PhanLoais.Any(p => p.PhanLoaiID == danhMuc.PhanLoaiID))
            {
                ModelState.AddModelError("PhanLoaiID", "Phân loại không hợp lệ");
                ViewBag.PhanLoaiID = new SelectList(
                    db.PhanLoais, "PhanLoaiID", "Ten", danhMuc.PhanLoaiID);
                return View(danhMuc);
            }

            var existing = db.DanhMucs.Find(danhMuc.DanhMucID);
            if (existing == null) return HttpNotFound();

            existing.Ten = danhMuc.Ten;
            existing.PhanLoaiID = danhMuc.PhanLoaiID;

            db.SaveChanges();
            return RedirectToAction("Index");
        }



        // GET: DanhMuc/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMuc danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            return View(danhMuc);
        }

        // POST: DanhMuc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DanhMuc danhMuc = db.DanhMucs.Find(id);
            db.DanhMucs.Remove(danhMuc);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}