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
    public class DON_HANGController : Controller
    {
        private QL_BAN_DO_UONGDb db = new QL_BAN_DO_UONGDb();

        // GET: DON_HANG
        public ActionResult Index(string searchMaDon, string sortGia, string trangThai, int page = 1)
        {
            int pageSize = 10;

            var donHangs = db.DON_HANG
                .Include(d => d.DIA_CHI)
                .Include(d => d.THANH_TOAN)
                .AsQueryable();

            // 🔍 TÌM KIẾM THEO MÃ ĐƠN
            if (!string.IsNullOrEmpty(searchMaDon))
            {
                donHangs = donHangs.Where(d =>
                    (d.id.ToString() + d.tai_khoan_id.ToString())
                    .Contains(searchMaDon));
            }
            // 🔘 LỌC THEO TRẠNG THÁI
            if (!string.IsNullOrEmpty(trangThai))
            {
                donHangs = donHangs.Where(d =>
                    d.THANH_TOAN.Any(t => t.trang_thai == trangThai));
            }


            // ↕️ SẮP XẾP
            ViewBag.SortGia = sortGia;
            switch (sortGia)
            {
                case "gia_asc":
                    donHangs = donHangs.OrderBy(d => d.tong_tien);
                    break;
                case "gia_desc":
                    donHangs = donHangs.OrderByDescending(d => d.tong_tien);
                    break;
                default:
                    donHangs = donHangs.OrderByDescending(d => d.id);
                    break;
            }

            // 📄 PHÂN TRANG
            int totalItems = donHangs.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var data = donHangs
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchMaDon = searchMaDon;
            ViewBag.TrangThai = trangThai;


            return View(data);
        }


        // GET: DON_HANG/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var donHang = db.DON_HANG
                .Include(d => d.TAI_KHOAN)
                .Include(d => d.DIA_CHI)
                .Include(d => d.THANH_TOAN)
                .Include(d => d.CHI_TIET_DON_HANG.Select(ct => ct.SAN_PHAM))
                .Include(d => d.CHI_TIET_DON_HANG.Select(ct => ct.SIZE))
                .Include(d => d.CHI_TIET_DON_HANG
                    .Select(ct => ct.CHI_TIET_DON_HANG_TOPPING
                        .Select(t => t.TOPPING)))
                .FirstOrDefault(d => d.id == id);

            if (donHang == null)
                return HttpNotFound();

            return View(donHang);
        }


        // GET: DON_HANG/Create
        public ActionResult Create()
        {
            ViewBag.dia_chi_id = new SelectList(db.DIA_CHI, "id", "nguoi_nhan");
            ViewBag.tai_khoan_id = new SelectList(db.TAI_KHOAN, "id", "ten_dang_nhap");
            return View();
        }

        // POST: DON_HANG/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,tai_khoan_id,dia_chi_id,tong_tien,ghi_chu")] DON_HANG dON_HANG)
        {
            if (ModelState.IsValid)
            {
                db.DON_HANG.Add(dON_HANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.dia_chi_id = new SelectList(db.DIA_CHI, "id", "nguoi_nhan", dON_HANG.dia_chi_id);
            ViewBag.tai_khoan_id = new SelectList(db.TAI_KHOAN, "id", "ten_dang_nhap", dON_HANG.tai_khoan_id);
            return View(dON_HANG);
        }

        // GET: DON_HANG/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DON_HANG dON_HANG = db.DON_HANG.Find(id);
            if (dON_HANG == null)
            {
                return HttpNotFound();
            }
            ViewBag.dia_chi_id = new SelectList(db.DIA_CHI, "id", "nguoi_nhan", dON_HANG.dia_chi_id);
            ViewBag.tai_khoan_id = new SelectList(db.TAI_KHOAN, "id", "ten_dang_nhap", dON_HANG.tai_khoan_id);
            return View(dON_HANG);
        }

        // POST: DON_HANG/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
     int id,
     string nguoi_nhan,
     string so_dien_thoai,
     string dia_chi_cu_the,
     string ghi_chu)
        {
            var donHang = db.DON_HANG
                .Include(d => d.DIA_CHI)
                .FirstOrDefault(d => d.id == id);

            if (donHang == null)
                return HttpNotFound();

            // cập nhật ghi chú đơn hàng
            donHang.ghi_chu = ghi_chu;

            // cập nhật địa chỉ giao hàng
            if (donHang.DIA_CHI != null)
            {
                donHang.DIA_CHI.nguoi_nhan = nguoi_nhan;
                donHang.DIA_CHI.so_dien_thoai = so_dien_thoai;
                donHang.DIA_CHI.dia_chi_cu_the = dia_chi_cu_the;
            }

            db.SaveChanges();

            // 👉 QUAY LẠI DETAILS, KHÔNG PHẢI INDEX
            return RedirectToAction("Details", new { id = id });
        }


        // GET: DON_HANG/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DON_HANG dON_HANG = db.DON_HANG.Find(id);
            if (dON_HANG == null)
            {
                return HttpNotFound();
            }
            return View(dON_HANG);
        }

        // POST: DON_HANG/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var donHang = db.DON_HANG
                .Include(d => d.CHI_TIET_DON_HANG.Select(ct => ct.CHI_TIET_DON_HANG_TOPPING))
                .Include(d => d.THANH_TOAN)
                .FirstOrDefault(d => d.id == id);

            if (donHang == null)
                return HttpNotFound();

            // 1️⃣ xóa topping
            foreach (var ct in donHang.CHI_TIET_DON_HANG)
            {
                db.CHI_TIET_DON_HANG_TOPPING.RemoveRange(ct.CHI_TIET_DON_HANG_TOPPING);
            }

            // 2️⃣ xóa chi tiết đơn hàng
            db.CHI_TIET_DON_HANG.RemoveRange(donHang.CHI_TIET_DON_HANG);

            // 3️⃣ xóa thanh toán
            db.THANH_TOAN.RemoveRange(donHang.THANH_TOAN);

            // 4️⃣ xóa đơn hàng
            db.DON_HANG.Remove(donHang);

            db.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult UpdateTrangThai(int donHangId, string trangThai)
        {
            var thanhToan = db.THANH_TOAN
                .FirstOrDefault(t => t.don_hang_id == donHangId);

            if (thanhToan == null)
                return Json(new { success = false });

            thanhToan.trang_thai = trangThai;
            db.SaveChanges();

            return Json(new { success = true });
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
