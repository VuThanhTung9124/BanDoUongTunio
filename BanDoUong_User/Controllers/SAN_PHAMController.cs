using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanDoUong_User.Models;
using Microsoft.Ajax.Utilities;
using PagedList;


namespace BanDoUong_User.Controllers
{
    public class SAN_PHAMController : Controller
    {
        private QL_BAN_DO_UONGDb db = new QL_BAN_DO_UONGDb();

        // GET: SAN_PHAM


        public ActionResult Index(
     string searchSP,
     string sortPrice,
     string category,
     int page = 1
 )
        {
            int pageSize = 8;

            var sanPham = db.SAN_PHAM
                            .Include(s => s.DANH_MUC)
                            .AsQueryable();

            // 📂 Lọc theo danh mục
            if (!string.IsNullOrEmpty(category))
            {
                sanPham = sanPham.Where(s => s.DANH_MUC.ten_danh_muc == category);
            }

            // 🔍 Tìm kiếm
            if (!string.IsNullOrEmpty(searchSP))
            {
                sanPham = sanPham.Where(s => s.ten_san_pham.Contains(searchSP));
            }

            // 🔃 Sắp xếp
            switch (sortPrice)
            {
                case "asc":
                    sanPham = sanPham.OrderBy(s => s.gia_co_ban);
                    break;
                case "desc":
                    sanPham = sanPham.OrderByDescending(s => s.gia_co_ban);
                    break;
                default:
                    sanPham = sanPham.OrderBy(s => s.id);
                    break;
            }

            // 📌 Tổng số sản phẩm (sau lọc)
            Session["so_luong_sp"] = sanPham.Count();

            // 📂 Danh mục
            Session["DanhMuc"] = db.DANH_MUC
                                    .Select(d => d.ten_danh_muc)
                                    .ToList();

            ViewBag.Category = category;
            ViewBag.SearchSP = searchSP;
            ViewBag.SortPrice = sortPrice;

            return View(sanPham.ToPagedList(page, pageSize));
        }


        // GET: SAN_PHAM/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var sanPham = db.SAN_PHAM
                            .Include(s => s.DANH_MUC)
                            .Include(s => s.SAN_PHAM_SIZE.Select(x => x.SIZE))
                            .FirstOrDefault(s => s.id == id);

            if (sanPham == null)
                return HttpNotFound();

            // =========================
            // 🔥 SẢN PHẨM TƯƠNG TỰ
            // =========================
            int danhMucId = sanPham.danh_muc_id.Value;

            var sanPhamTuongTu = db.SAN_PHAM
                .Where(s => s.danh_muc_id == danhMucId && s.id != sanPham.id)
                .OrderBy(x => Guid.NewGuid())   // RANDOM
                .Take(4)
                .ToList();

            ViewBag.SanPhamTuongTu = sanPhamTuongTu;

            return View(sanPham);
        }


        [HttpPost]
        public ActionResult ThemGioHang(int sanPhamId, int sizeId, int soLuong)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("DangNhap", "TAI_KHOAN");
            }

            int userId = (int)Session["UserId"];

            var gioHang = db.GIO_HANG
                .FirstOrDefault(g => g.tai_khoan_id == userId);

            if (gioHang == null)
            {
                gioHang = new GIO_HANG
                {
                    tai_khoan_id = userId
                };
                db.GIO_HANG.Add(gioHang);
                db.SaveChanges();
            }

            var ct = db.CHI_TIET_GIO_HANG.FirstOrDefault(x =>
                x.gio_hang_id == gioHang.id &&
                x.san_pham_id == sanPhamId &&
                x.size_id == sizeId);

            if (ct != null)
            {
                ct.so_luong += soLuong;
            }
            else
            {
                ct = new CHI_TIET_GIO_HANG
                {
                    gio_hang_id = gioHang.id,
                    san_pham_id = sanPhamId,
                    size_id = sizeId,
                    so_luong = soLuong
                };
                db.CHI_TIET_GIO_HANG.Add(ct);
            }

            db.SaveChanges();

            // 🔥🔥🔥 CẬP NHẬT SESSION GIỎ HÀNG NGAY LẬP TỨC
            Session["so_luong"] = db.CHI_TIET_GIO_HANG
                .Where(x => x.gio_hang_id == gioHang.id)
                .Sum(x => (int?)x.so_luong) ?? 0;

            return RedirectToAction("Index", "SAN_PHAM");
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
