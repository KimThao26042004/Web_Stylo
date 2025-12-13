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
    public class PhanLoaiController : Controller
    {
        private fashion_shopEntities db = new fashion_shopEntities();

        // GET: PhanLoai
        public ActionResult Index(string searchTerm)
        {
            var categories = from c in db.PhanLoais
                             select c;

            if (!String.IsNullOrEmpty(searchTerm))
            {
                categories = categories.Where(c => c.Ten.Contains(searchTerm));
            }

            return View(categories.ToList());
        }

        // GET: PhanLoai/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhanLoai/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PhanLoai phanLoai)
        {
            if (ModelState.IsValid)
            {
                db.PhanLoais.Add(phanLoai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phanLoai);
        }

        // GET: PhanLoai/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanLoai phanLoai = db.PhanLoais.Find(id);
            if (phanLoai == null)
            {
                return HttpNotFound();
            }
            return View(phanLoai);
        }

        // POST: PhanLoai/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PhanLoai phanLoai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phanLoai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phanLoai);
        }

        // GET: PhanLoai/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanLoai phanLoai = db.PhanLoais.Find(id);
            if (phanLoai == null)
            {
                return HttpNotFound();
            }
            return View(phanLoai);
        }

        // POST: PhanLoai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhanLoai phanLoai = db.PhanLoais.Find(id);
            db.PhanLoais.Remove(phanLoai);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}