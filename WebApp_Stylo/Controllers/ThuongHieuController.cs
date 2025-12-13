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
    public class ThuongHieuController : Controller
    {
        private fashion_shopEntities db = new fashion_shopEntities();

        // GET: ThuongHieu
        public ActionResult Index(string searchTerm, int page = 1)
        {
            int pageSize = 10;  
            var query = db.ThuongHieux.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.Ten.Contains(searchTerm));
            }

            int totalRecords = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var brands = query
                .OrderBy(t => t.Ten)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(brands);
        }

        // GET: ThuongHieu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ThuongHieu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ThuongHieu thuongHieu)
        {
            if (ModelState.IsValid)
            {
                db.ThuongHieux.Add(thuongHieu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(thuongHieu);
        }

        // GET: ThuongHieu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThuongHieu thuongHieu = db.ThuongHieux.Find(id);
            if (thuongHieu == null)
            {
                return HttpNotFound();
            }
            return View(thuongHieu);
        }

        // POST: ThuongHieu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ThuongHieu thuongHieu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thuongHieu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(thuongHieu);
        }

        // GET: ThuongHieu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThuongHieu thuongHieu = db.ThuongHieux.Find(id);
            if (thuongHieu == null)
            {
                return HttpNotFound();
            }
            return View(thuongHieu);
        }

        // POST: ThuongHieu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ThuongHieu thuongHieu = db.ThuongHieux.Find(id);
            db.ThuongHieux.Remove(thuongHieu);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}