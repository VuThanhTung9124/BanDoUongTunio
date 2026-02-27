using System;
using System.Linq;
using System.Web.Mvc;
using BanDoUongTunio.Models;

namespace BanDoUongTunio.Controllers
{
    public class ThongKeController : Controller
    {
        private QL_BAN_DO_UONGDb db = new QL_BAN_DO_UONGDb();

        public ActionResult Index()
        {
            /* ========== TỔNG QUAN ========== */

            ViewBag.TongDonHang = db.DON_HANG.Count();

            ViewBag.TongDoanhThu = db.THANH_TOAN
                .Where(x => x.trang_thai == "Đã xác nhận")
                .Sum(x => (decimal?)x.DON_HANG.tong_tien) ?? 0;

            ViewBag.TongSanPham = db.SAN_PHAM.Count();
            ViewBag.TongTaiKhoan = db.TAI_KHOAN.Count();
            ViewBag.TongDanhMuc = db.DANH_MUC.Count();


            /* ========== THỐNG KÊ ĐƠN HÀNG ========== */
            // ✔ ĐÚNG: đếm theo trạng thái

            ViewBag.DonChoXacNhan = db.DON_HANG
    .Count(d =>
        !d.THANH_TOAN.Any()
        || d.THANH_TOAN.Any(t => t.trang_thai == "Chờ xác nhận")
    );

            ViewBag.DonDaXacNhan = db.DON_HANG
                .Count(d => d.THANH_TOAN.Any(t => t.trang_thai == "Đã xác nhận"));

            ViewBag.DonDaHuy = db.DON_HANG
                .Count(d => d.THANH_TOAN.Any(t => t.trang_thai == "Đã hủy"));


            ViewBag.GiaTriTB = db.THANH_TOAN
                .Where(x => x.trang_thai == "Đã xác nhận")
                .Average(x => (decimal?)x.DON_HANG.tong_tien) ?? 0;


            /* ========== THỐNG KÊ SẢN PHẨM ========== */
            // ✔ PHẢI thống kê từ CHI_TIET_DON_HANG

            var spBanNhieu = db.CHI_TIET_DON_HANG
                .GroupBy(x => x.san_pham_id)
                .Select(g => new
                {
                    SanPhamId = g.Key,
                    SoLuong = g.Sum(x => x.so_luong)
                })
                .OrderByDescending(x => x.SoLuong)
                .FirstOrDefault();

            ViewBag.SPBanNhieu = spBanNhieu != null
                ? db.SAN_PHAM.Find(spBanNhieu.SanPhamId)?.ten_san_pham
                : "Không có dữ liệu";


            var spBanIt = db.CHI_TIET_DON_HANG
                .GroupBy(x => x.san_pham_id)
                .Select(g => new
                {
                    SanPhamId = g.Key,
                    SoLuong = g.Sum(x => x.so_luong)
                })
                .OrderBy(x => x.SoLuong)
                .FirstOrDefault();

            ViewBag.SPBanIt = spBanIt != null
                ? db.SAN_PHAM.Find(spBanIt.SanPhamId)?.ten_san_pham
                : "Không có dữ liệu";


            /* ========== GIÁ CAO NHẤT / THẤP NHẤT ========== */
            // ✔ LẤY TỪ BẢNG SAN_PHAM

            ViewBag.GiaCaoNhat = db.SAN_PHAM.Max(x => (decimal?)x.gia_co_ban) ?? 0;
            ViewBag.GiaThapNhat = db.SAN_PHAM.Min(x => (decimal?)x.gia_co_ban) ?? 0;


            /* ========== KHÁCH HÀNG MUA NHIỀU NHẤT ========== */
            // ✔ DỰA TRÊN SỐ ĐƠN ĐÃ XÁC NHẬN

            var tkMuaNhieu = db.THANH_TOAN
                .Where(x => x.trang_thai == "Đã xác nhận")
                .GroupBy(x => x.DON_HANG.tai_khoan_id)
                .Select(g => new
                {
                    TaiKhoanId = g.Key,
                    SoDon = g.Count()
                })
                .OrderByDescending(x => x.SoDon)
                .FirstOrDefault();

            if (tkMuaNhieu != null)
            {
                var tk = db.TAI_KHOAN.Find(tkMuaNhieu.TaiKhoanId);

                ViewBag.TKMuaNhieu_TenDangNhap = tk?.ten_dang_nhap ?? "-";
                ViewBag.TKMuaNhieu_HoTen = tk?.ho_ten ?? "-";
                ViewBag.TKMuaNhieu_SDT = tk?.so_dien_thoai ?? "-";
                ViewBag.TKMuaNhieu_Email = tk?.email ?? "-";
                ViewBag.TKMuaNhieu_SoDon = tkMuaNhieu.SoDon;
            }
            else
            {
                ViewBag.TKMuaNhieu_TenDangNhap = "-";
                ViewBag.TKMuaNhieu_HoTen = "-";
                ViewBag.TKMuaNhieu_SDT = "-";
                ViewBag.TKMuaNhieu_Email = "-";
                ViewBag.TKMuaNhieu_SoDon = 0;
            }

            return View();
        }

    }
}
