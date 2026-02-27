using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BanDoUong_User.Models;

namespace BanDoUong_User.Controllers
{
    public class GIO_HANGController : Controller
    {
        private QL_BAN_DO_UONGDb db = new QL_BAN_DO_UONGDb();

        // ================= GET =================
        [HttpGet]
        public ActionResult Index(int[] selectedIds)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("DangNhap", "TAI_KHOAN");

            int userId = (int)Session["UserId"];

            var gioHang = db.GIO_HANG
                .Include(g => g.CHI_TIET_GIO_HANG
                    .Select(ct => ct.SAN_PHAM.SAN_PHAM_SIZE))
                .FirstOrDefault(g => g.tai_khoan_id == userId);

            if (gioHang == null)
            {
                gioHang = new GIO_HANG
                {
                    CHI_TIET_GIO_HANG = new List<CHI_TIET_GIO_HANG>()
                };
            }

            // nếu chưa chọn gì
            var selected = selectedIds?.ToList() ?? new List<int>();

            decimal tongTien = 0;
            int tongSoLuong = 0;

            foreach (var item in gioHang.CHI_TIET_GIO_HANG)
            {
                if (!selected.Contains(item.id)) continue;

                var giaSize = item.SAN_PHAM.SAN_PHAM_SIZE
                    .FirstOrDefault(x => x.size_id == item.size_id)?.gia ?? 0;

                int soLuong = item.so_luong ?? 1;

                tongTien += giaSize * soLuong;
                tongSoLuong += soLuong;
            }

            // truyền dữ liệu xuống view
            ViewBag.TongTien = tongTien;
            ViewBag.TongSoLuong = tongSoLuong;
            ViewBag.SelectedIds = selected;

            // cập nhật icon giỏ hàng
            Session["so_luong"] = gioHang.CHI_TIET_GIO_HANG
                .Sum(x => (int?)x.so_luong) ?? 0;

            return View(gioHang);
        }


        // ================= POST (CHECK + UPDATE) =================
        [HttpPost]
        public ActionResult Index(
    List<int> selectedIds,
    List<int> ItemIds,
    List<int> SizeIds,
    List<int> Quantities
)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("DangNhap", "TAI_KHOAN");

            int userId = (int)Session["UserId"];

            var gioHang = db.GIO_HANG
                .Include(g => g.CHI_TIET_GIO_HANG.Select(ct => ct.SAN_PHAM.SAN_PHAM_SIZE))
                .FirstOrDefault(g => g.tai_khoan_id == userId);

            if (gioHang == null)
                return RedirectToAction("Index");

            // ===== UPDATE SIZE + QTY =====
            if (ItemIds != null && SizeIds != null && Quantities != null)
            {
                for (int i = 0; i < ItemIds.Count; i++)
                {
                    int ctId = ItemIds[i];

                    var ct = gioHang.CHI_TIET_GIO_HANG
                        .FirstOrDefault(x => x.id == ctId);

                    if (ct != null)
                    {
                        ct.size_id = SizeIds[i];
                        ct.so_luong = Quantities[i] < 1 ? 1 : Quantities[i];
                    }
                }
            }

            db.SaveChanges();

            // ===== TÍNH TỔNG =====
            decimal tongTien = 0;
            int tongSoLuong = 0;

            if (selectedIds != null)
            {
                foreach (var ct in gioHang.CHI_TIET_GIO_HANG
                             .Where(x => selectedIds.Contains(x.id)))
                {
                    var gia = ct.SAN_PHAM.SAN_PHAM_SIZE
                        .FirstOrDefault(s => s.size_id == ct.size_id)?.gia ?? 0;

                    int soLuong = ct.so_luong ?? 1;

                    tongTien += gia * soLuong;
                    tongSoLuong += soLuong;
                }
            }

            // truyền xuống view
            ViewBag.TongTien = tongTien;
            ViewBag.TongSoLuong = tongSoLuong;
            ViewBag.SelectedIds = selectedIds ?? new List<int>();

            Session["so_luong"] = gioHang.CHI_TIET_GIO_HANG
                .Sum(x => (int?)x.so_luong) ?? 0;

            return View(gioHang);
        }


        // ================= XÓA =================
        public ActionResult Xoa(int id)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("DangNhap", "TAI_KHOAN");

            int userId = (int)Session["UserId"];

            var ct = db.CHI_TIET_GIO_HANG.Find(id);
            if (ct != null)
            {
                db.CHI_TIET_GIO_HANG.Remove(ct);
                db.SaveChanges();
            }

            Session["so_luong"] = db.CHI_TIET_GIO_HANG
                .Count(x => x.GIO_HANG.tai_khoan_id == userId);

            return RedirectToAction("Index");
        }
    }
}
