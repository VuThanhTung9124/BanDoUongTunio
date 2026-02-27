using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanDoUongTunio.Models;
using PagedList;

namespace BanDoUongTunio.Controllers
{
    public class SAN_PHAMController : Controller
    {
        private QL_BAN_DO_UONGDb db = new QL_BAN_DO_UONGDb();

        // GET: SAN_PHAM

        public ActionResult Index(int? page, string searchSP, string sortPrice)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            var sp = db.SAN_PHAM.AsQueryable();

            // 🔍 Tìm kiếm
            if (!string.IsNullOrEmpty(searchSP))
            {
                sp = sp.Where(x => x.ten_san_pham.Contains(searchSP));
            }

            // 🔽 Sắp xếp giá
            switch (sortPrice)
            {
                case "asc":
                    sp = sp.OrderBy(x => x.gia_co_ban);
                    break;

                case "desc":
                    sp = sp.OrderByDescending(x => x.gia_co_ban);
                    break;

                default:
                    sp = sp.OrderBy(x => x.id); // mặc định
                    break;
            }

            ViewBag.SearchSP = searchSP;
            ViewBag.SortPrice = sortPrice;

            return View(sp.ToPagedList(pageNumber, pageSize));
        }



        // GET: SAN_PHAM/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SAN_PHAM sAN_PHAM = db.SAN_PHAM.Find(id);
            if (sAN_PHAM == null)
            {
                return HttpNotFound();
            }
            return View(sAN_PHAM);
        }

        // GET: SAN_PHAM/Create
        public ActionResult Create()
        {
            ViewBag.danh_muc_id = new SelectList(db.DANH_MUC, "id", "ten_danh_muc");
            return View();
        }

        // POST: SAN_PHAM/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
    SAN_PHAM sAN_PHAM,
    HttpPostedFileBase ImageFile
)
        {
            // DEBUG nếu fail
            foreach (var error in ModelState)
            {
                if (error.Value.Errors.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine(
                        error.Key + " : " + error.Value.Errors[0].ErrorMessage
                    );
                }
            }

            if (ModelState.IsValid)
            {
                // 1️⃣ UPLOAD ẢNH
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(ImageFile.FileName);
                    string uploadPath = Server.MapPath("~/Image/Image_DoUong/" + fileName);
                    ImageFile.SaveAs(uploadPath);

                    sAN_PHAM.hinh_anh = fileName;
                }
                else
                {
                    sAN_PHAM.hinh_anh = "no-image-news.png";
                }

                // 2️⃣ LƯU SẢN PHẨM
                db.SAN_PHAM.Add(sAN_PHAM);
                db.SaveChanges();

                // 3️⃣ TẠO SIZE
                var sizes = db.SIZEs.ToList();
                foreach (var size in sizes)
                {
                    decimal gia = sAN_PHAM.gia_co_ban ?? 0;

                    if (size.ten_size == "M") gia *= 1.25m;
                    if (size.ten_size == "L") gia *= 1.5m;

                    db.SAN_PHAM_SIZE.Add(new SAN_PHAM_SIZE
                    {
                        san_pham_id = sAN_PHAM.id,
                        size_id = size.id,
                        gia = gia
                    });
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.danh_muc_id = new SelectList(
                db.DANH_MUC, "id", "ten_danh_muc", sAN_PHAM.danh_muc_id
            );

            return View(sAN_PHAM);
        }


        // GET: SAN_PHAM/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SAN_PHAM sAN_PHAM = db.SAN_PHAM.Find(id);
            if (sAN_PHAM == null)
            {
                return HttpNotFound();
            }
            ViewBag.danh_muc_id = new SelectList(db.DANH_MUC, "id", "ten_danh_muc", sAN_PHAM.danh_muc_id);
            return View(sAN_PHAM);
        }

        // POST: SAN_PHAM/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SAN_PHAM sAN_PHAM, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Nếu người dùng chọn ảnh mới
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(ImageFile.FileName);
                    string uploadPath = Server.MapPath("~/Image/Image_DoUong/" + fileName);
                    ImageFile.SaveAs(uploadPath);

                    sAN_PHAM.hinh_anh = fileName;
                }
                // Nếu không chọn ảnh → giữ nguyên (nhờ HiddenFor bên View)

                db.Entry(sAN_PHAM).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.danh_muc_id = new SelectList(
                db.DANH_MUC, "id", "ten_danh_muc", sAN_PHAM.danh_muc_id
            );

            return View(sAN_PHAM);
        }


        // GET: SAN_PHAM/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SAN_PHAM sAN_PHAM = db.SAN_PHAM.Find(id);
            if (sAN_PHAM == null)
            {
                return HttpNotFound();
            }
            return View(sAN_PHAM);
        }

        // POST: SAN_PHAM/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var sp = db.SAN_PHAM
                .Include(x => x.SAN_PHAM_SIZE)
                .FirstOrDefault(x => x.id == id);

            if (sp == null)
                return RedirectToAction("Index");

            // 1️⃣ Xóa size của sản phẩm
            db.SAN_PHAM_SIZE.RemoveRange(sp.SAN_PHAM_SIZE);

            // 2️⃣ Xóa sản phẩm
            db.SAN_PHAM.Remove(sp);

            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult TrangChu()
        {
            return View();
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
