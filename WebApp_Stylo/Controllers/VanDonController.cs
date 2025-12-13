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
        public ActionResult Index(string searchTerm)
        {
            var shipping = from v in db.VanDons
                           select v;

            if (!String.IsNullOrEmpty(searchTerm))
            {
                shipping = shipping.Where(v => v.MaVanDon.Contains(searchTerm));
            }

            return View(shipping.ToList());
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

        // POST: VanDon/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VanDon vanDon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vanDon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vanDon);
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