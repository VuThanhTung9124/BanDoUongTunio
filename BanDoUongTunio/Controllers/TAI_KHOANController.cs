using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanDoUongTunio.Models;

        using PagedList;
namespace BanDoUongTunio.Controllers
{
    public class TAI_KHOANController : Controller
    {
        private QL_BAN_DO_UONGDb db = new QL_BAN_DO_UONGDb();

     

public ActionResult Index(string searchTK, int? page)
    {
        int pageSize = 5;                 // số tài khoản / trang
        int pageNumber = page ?? 1;

        var taiKhoanQuery = db.TAI_KHOAN.AsQueryable();

        // 🔍 TÌM KIẾM THEO EMAIL HOẶC SỐ ĐIỆN THOẠI
        if (!string.IsNullOrEmpty(searchTK))
        {
            taiKhoanQuery = taiKhoanQuery.Where(t =>
                t.email.Contains(searchTK) ||
                t.so_dien_thoai.Contains(searchTK)
            );
        }

        // bắt buộc OrderBy
        taiKhoanQuery = taiKhoanQuery.OrderBy(t => t.id);

        // giữ keyword cho view
        ViewBag.SearchTK = searchTK;

        var taiKhoanPaged = taiKhoanQuery.ToPagedList(pageNumber, pageSize);

        return View(taiKhoanPaged);
    }



    // GET: TAI_KHOAN/Details/5
    public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAI_KHOAN tAI_KHOAN = db.TAI_KHOAN.Find(id);
            if (tAI_KHOAN == null)
            {
                return HttpNotFound();
            }
            return View(tAI_KHOAN);
        }

        // GET: TAI_KHOAN/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TAI_KHOAN/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ten_dang_nhap,mat_khau,ho_ten,email,so_dien_thoai")] TAI_KHOAN tAI_KHOAN)
        {
            if (ModelState.IsValid)
            {
                db.TAI_KHOAN.Add(tAI_KHOAN);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tAI_KHOAN);
        }

        // GET: TAI_KHOAN/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAI_KHOAN tAI_KHOAN = db.TAI_KHOAN.Find(id);
            if (tAI_KHOAN == null)
            {
                return HttpNotFound();
            }
            return View(tAI_KHOAN);
        }

        // POST: TAI_KHOAN/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ten_dang_nhap,mat_khau,ho_ten,email,so_dien_thoai")] TAI_KHOAN tAI_KHOAN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tAI_KHOAN).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tAI_KHOAN);
        }

        // GET: TAI_KHOAN/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAI_KHOAN tAI_KHOAN = db.TAI_KHOAN.Find(id);
            if (tAI_KHOAN == null)
            {
                return HttpNotFound();
            }
            return View(tAI_KHOAN);
        }

        // POST: TAI_KHOAN/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var taiKhoan = db.TAI_KHOAN
                .Include(t => t.DON_HANG
                    .Select(d => d.CHI_TIET_DON_HANG
                        .Select(ct => ct.CHI_TIET_DON_HANG_TOPPING)))
                .Include(t => t.DON_HANG.Select(d => d.THANH_TOAN))
                .Include(t => t.DIA_CHI)
                .Include(t => t.GIO_HANG.Select(g => g.CHI_TIET_GIO_HANG))
                .FirstOrDefault(t => t.id == id);

            if (taiKhoan == null)
                return HttpNotFound();

            // 🔹 XÓA GIỎ HÀNG
            foreach (var gio in taiKhoan.GIO_HANG)
            {
                db.CHI_TIET_GIO_HANG.RemoveRange(gio.CHI_TIET_GIO_HANG);
            }
            db.GIO_HANG.RemoveRange(taiKhoan.GIO_HANG);

            // 🔹 XÓA ĐƠN HÀNG
            foreach (var don in taiKhoan.DON_HANG)
            {
                // XÓA TOPPING TRƯỚC
                foreach (var ct in don.CHI_TIET_DON_HANG)
                {
                    db.CHI_TIET_DON_HANG_TOPPING
                        .RemoveRange(ct.CHI_TIET_DON_HANG_TOPPING);
                }

                // XÓA CHI TIẾT ĐƠN
                db.CHI_TIET_DON_HANG.RemoveRange(don.CHI_TIET_DON_HANG);

                // XÓA THANH TOÁN
                db.THANH_TOAN.RemoveRange(don.THANH_TOAN);
            }

            db.DON_HANG.RemoveRange(taiKhoan.DON_HANG);

            // 🔹 XÓA ĐỊA CHỈ
            db.DIA_CHI.RemoveRange(taiKhoan.DIA_CHI);

            // 🔹 CUỐI CÙNG XÓA TÀI KHOẢN
            db.TAI_KHOAN.Remove(taiKhoan);

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
