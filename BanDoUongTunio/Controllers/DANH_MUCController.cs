using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanDoUongTunio.Models;

namespace BanDoUongTunio.Controllers
{
    public class DANH_MUCController : Controller
    {
        private QL_BAN_DO_UONGDb db = new QL_BAN_DO_UONGDb();

        // GET: DANH_MUC
        public ActionResult Index()
        {
            return View(db.DANH_MUC.ToList());
        }

        // GET: DANH_MUC/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DANH_MUC dANH_MUC = db.DANH_MUC.Find(id);
            if (dANH_MUC == null)
            {
                return HttpNotFound();
            }
            return View(dANH_MUC);
        }

        // GET: DANH_MUC/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DANH_MUC/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ten_danh_muc,mo_ta")] DANH_MUC dANH_MUC)
        {
            if (ModelState.IsValid)
            {
                db.DANH_MUC.Add(dANH_MUC);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dANH_MUC);
        }

        // GET: DANH_MUC/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DANH_MUC dANH_MUC = db.DANH_MUC.Find(id);
            if (dANH_MUC == null)
            {
                return HttpNotFound();
            }
            return View(dANH_MUC);
        }

        // POST: DANH_MUC/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ten_danh_muc,mo_ta")] DANH_MUC dANH_MUC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dANH_MUC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dANH_MUC);
        }

        // GET: DANH_MUC/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DANH_MUC dANH_MUC = db.DANH_MUC.Find(id);
            if (dANH_MUC == null)
            {
                return HttpNotFound();
            }
            return View(dANH_MUC);
        }

        // POST: DANH_MUC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //DANH_MUC dANH_MUC = db.DANH_MUC.Find(id);
            //db.DANH_MUC.Remove(dANH_MUC);
            //db.SaveChanges();
            //return RedirectToAction("Index");


            var danhMuc = db.DANH_MUC
                   .Include(d => d.SAN_PHAM)
                   .FirstOrDefault(d => d.id == id);

            db.SAN_PHAM.RemoveRange(danhMuc.SAN_PHAM);
            db.DANH_MUC.Remove(danhMuc);

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
