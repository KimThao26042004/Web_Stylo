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
    public class VanDonController : Controller
    {
        private fashion_shopEntities db = new fashion_shopEntities();

        // GET: VanDon
        public ActionResult Index(string searchTerm, int page = 1)
        {
            int pageSize = 10;
            var query = db.VanDons.AsQueryable();

            if (!String.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(v => v.MaVanDon.Contains(searchTerm));
            }

            int totalRecords = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var xshipping = query
                .OrderBy(t => t.MaVanDon)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(xshipping);
        }

        // GET: VanDon/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VanDon/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VanDon vanDon)
        {
            if (ModelState.IsValid)
            {
                db.VanDons.Add(vanDon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vanDon);
        }

        // GET: VanDon/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var vanDon = db.VanDons.Find(id);
            if (vanDon == null)
                return HttpNotFound();

            return View(vanDon);
        }

        // POST: VanDon/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VanDon vanDon)
        {
            if (!ModelState.IsValid)
                return View(vanDon);

            var existing = db.VanDons.Find(vanDon.VanDonID);
            if (existing == null)
                return HttpNotFound();

            existing.TrangThaiGiao = vanDon.TrangThaiGiao;
            existing.NgayGui = vanDon.NgayGui;
            existing.NgayGiaoDuKien = vanDon.NgayGiaoDuKien;
            existing.DVVC = vanDon.DVVC;
            existing.PhiVanChuyen = vanDon.PhiVanChuyen;

            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: VanDon/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VanDon vanDon = db.VanDons.Find(id);
            if (vanDon == null)
            {
                return HttpNotFound();
            }
            return View(vanDon);
        }

        // POST: VanDon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VanDon vanDon = db.VanDons.Find(id);
            db.VanDons.Remove(vanDon);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}