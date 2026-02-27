using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BanDoUong_User.Models;
using BanDoUong_User.ViewModels;

namespace BanDoUong_User.Controllers
{
    public class DON_HANGController : Controller
    {
        private QL_BAN_DO_UONGDb db = new QL_BAN_DO_UONGDb();

        public ActionResult Index()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("DangNhap", "TAI_KHOAN");

            int userId = (int)Session["UserId"];

            var donHangs = db.DON_HANG
                .Include(d => d.DIA_CHI)
                .Include(d => d.THANH_TOAN)
                .Include(d => d.CHI_TIET_DON_HANG
                    .Select(ct => ct.SAN_PHAM))
                .Include(d => d.CHI_TIET_DON_HANG
                    .Select(ct => ct.SIZE))
                .Include(d => d.CHI_TIET_DON_HANG
                    .Select(ct => ct.CHI_TIET_DON_HANG_TOPPING
                        .Select(t => t.TOPPING)))
                .Where(d => d.tai_khoan_id == userId)
                .OrderByDescending(d => d.id)
                .ToList();



            return View(donHangs);
        }




        // ================= THANH TOÁN (CHỌN SP) =================
        [HttpPost]
        public ActionResult ThanhToan(int[] selectedIds)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("DangNhap", "TAI_KHOAN");

            int userId = (int)Session["UserId"];

            var gioHang = db.GIO_HANG
                .Include(g => g.CHI_TIET_GIO_HANG
                    .Select(c => c.SAN_PHAM.SAN_PHAM_SIZE.Select(s => s.SIZE)))
                .FirstOrDefault(g => g.tai_khoan_id == userId);

            if (gioHang == null || selectedIds == null || !selectedIds.Any())
                return RedirectToAction("Index", "GIO_HANG");

            var toppings = db.TOPPINGs.ToList();

            var vm = new ThanhToanVM
            {
                Items = gioHang.CHI_TIET_GIO_HANG
                    .Where(x => selectedIds.Contains(x.id))
                    .Select(x =>
                    {
                        var sizeGia = x.SAN_PHAM.SAN_PHAM_SIZE
                            .First(s => s.size_id == x.size_id);

                        return new GioHangItemVM
                        {
                            ChiTietGioHangId = x.id,
                            SanPhamId = x.san_pham_id.Value,
                            TenSanPham = x.SAN_PHAM.ten_san_pham,
                            HinhAnh = x.SAN_PHAM.hinh_anh,
                            SizeId = x.size_id.Value,
                            TenSize = sizeGia.SIZE.ten_size,
                            SoLuong = x.so_luong ?? 1,
                            DonGia = sizeGia.gia ?? 0,
                            Toppings = toppings,
                            SelectedToppingIds = new List<int>()
                        };
                    }).ToList()
            };

            // ===== TẠO NỘI DUNG CHUYỂN KHOẢN =====
            int nextId = 1;

            if (db.DON_HANG.Any())
            {
                nextId = db.DON_HANG.Max(x => x.id) + 1;
            }

            string noiDung = "DH" + nextId + userId;

            ViewBag.NoiDungChuyenKhoan = noiDung;

            return View(vm);
        }

        // ================= XÁC NHẬN THANH TOÁN =================
        [HttpPost]
        public ActionResult XacNhanThanhToan(ThanhToanVM model)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("DangNhap", "TAI_KHOAN");

            int userId = (int)Session["UserId"];

            // ===== HÀM PHỤ: GÁN LẠI TOPPING KHI RETURN VIEW =====
            void ReloadToppings()
            {
                var toppings = db.TOPPINGs.ToList();

                if (model.Items != null)
                {
                    foreach (var item in model.Items)
                    {
                        item.Toppings = toppings;

                        if (item.SelectedToppingIds == null)
                            item.SelectedToppingIds = new List<int>();
                    }

                    model.TongTien = model.Items.Sum(i => i.DonGia * i.SoLuong);
                }
            }

            // ===== BẮT LỖI THÔNG TIN GIAO HÀNG =====
            if (string.IsNullOrEmpty(model.NguoiNhan) ||
                string.IsNullOrEmpty(model.SoDienThoai) ||
                string.IsNullOrEmpty(model.DiaChiCuThe))
            {
                ViewBag.LoiGiaoHang = "Vui lòng nhập đầy đủ thông tin giao hàng!";
                ReloadToppings();
                return View("ThanhToan", model);
            }

            // ===== CHỈ TÍNH GIÁ KHI CHƯA CHỌN PHƯƠNG THỨC =====
            if (string.IsNullOrEmpty(model.PhuongThucThanhToan))
            {
                decimal tongTien = 0;

                foreach (var item in model.Items)
                {
                    decimal giaGoc = item.DonGia;
                    decimal giaTopping = 0;

                    if (item.SelectedToppingIds != null && item.SelectedToppingIds.Any())
                    {
                        giaTopping = db.TOPPINGs
                            .Where(t => item.SelectedToppingIds.Contains(t.id))
                            .Sum(t => t.gia ?? 0);
                    }

                    item.DonGia = giaGoc + giaTopping;
                    tongTien += item.DonGia * item.SoLuong;
                }

                model.TongTien = tongTien;

                ReloadToppings();
                return View("ThanhToan", model);
            }

            // ================== TẠO ĐƠN HÀNG ==================
            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    // 1️⃣ LƯU ĐỊA CHỈ
                    var diaChi = new DIA_CHI
                    {
                        tai_khoan_id = userId,
                        nguoi_nhan = model.NguoiNhan,
                        so_dien_thoai = model.SoDienThoai,
                        dia_chi_cu_the = model.DiaChiCuThe,
                        ghi_chu = model.GhiChu
                    };
                    db.DIA_CHI.Add(diaChi);
                    db.SaveChanges();

                    // 2️⃣ TẠO ĐƠN HÀNG
                    var donHang = new DON_HANG
                    {
                        tai_khoan_id = userId,
                        dia_chi_id = diaChi.id,
                        tong_tien = 0,
                        ghi_chu = model.GhiChu,
                     
                    };
                    db.DON_HANG.Add(donHang);
                    db.SaveChanges();

                    decimal tongTien = 0;

                    foreach (var item in model.Items)
                    {
                        // 1️⃣ LẤY GIÁ SIZE TỪ DB
                        decimal giaBase = db.SAN_PHAM_SIZE
                            .Where(s => s.san_pham_id == item.SanPhamId && s.size_id == item.SizeId)
                            .Select(s => s.gia ?? 0)
                            .FirstOrDefault();

                        // 2️⃣ TÍNH GIÁ TOPPING
                        decimal giaTopping = 0;
                        if (item.SelectedToppingIds != null && item.SelectedToppingIds.Any())
                        {
                            giaTopping = db.TOPPINGs
                                .Where(t => item.SelectedToppingIds.Contains(t.id))
                                .Sum(t => t.gia ?? 0);
                        }

                        // 3️⃣ GIÁ CUỐI 1 SẢN PHẨM
                        decimal giaCuoi = giaBase + giaTopping;

                        // 4️⃣ THÀNH TIỀN
                        decimal thanhTienItem = giaCuoi * item.SoLuong;
                        tongTien += thanhTienItem;

                        // 5️⃣ LƯU CHI TIẾT ĐƠN
                        var ct = new CHI_TIET_DON_HANG
                        {
                            don_hang_id = donHang.id,
                            san_pham_id = item.SanPhamId,
                            size_id = item.SizeId,
                            so_luong = item.SoLuong,
                            don_gia = giaCuoi   // ✅ GIÁ ĐÃ CỘNG TOPPING
                        };
                        db.CHI_TIET_DON_HANG.Add(ct);
                        db.SaveChanges();

                        // 6️⃣ LƯU TOPPING
                        if (item.SelectedToppingIds != null)
                        {
                            foreach (var topId in item.SelectedToppingIds)
                            {
                                var topping = db.TOPPINGs.Find(topId);
                                if (topping == null) continue;

                                db.CHI_TIET_DON_HANG_TOPPING.Add(new CHI_TIET_DON_HANG_TOPPING
                                {
                                    chi_tiet_don_hang_id = ct.id,
                                    topping_id = topId,
                                    so_luong = 1,
                                    gia = topping.gia
                                });
                            }
                        }

                        // 7️⃣ XÓA GIỎ
                        var gioHangItem = db.CHI_TIET_GIO_HANG.Find(item.ChiTietGioHangId);
                        if (gioHangItem != null)
                            db.CHI_TIET_GIO_HANG.Remove(gioHangItem);
                    }


                    // 4️⃣ CẬP NHẬT TỔNG TIỀN
                    donHang.tong_tien = tongTien;
                    db.SaveChanges();

                    // 5️⃣ THANH TOÁN
                    db.THANH_TOAN.Add(new THANH_TOAN
                    {
                        don_hang_id = donHang.id,
                        phuong_thuc = model.PhuongThucThanhToan,
                        trang_thai = "Chờ xác nhận"
                    });

                    db.SaveChanges();

                    // 6️⃣ CẬP NHẬT SESSION GIỎ
                    var gioHang = db.GIO_HANG.FirstOrDefault(g => g.tai_khoan_id == userId);

                    Session["so_luong"] = gioHang == null
                        ? 0
                        : db.CHI_TIET_GIO_HANG
                            .Where(x => x.gio_hang_id == gioHang.id)
                            .Sum(x => (int?)x.so_luong) ?? 0;

                    tran.Commit();
                    return RedirectToAction("HoanTat");
                }
                catch
                {
                    tran.Rollback();
                    return RedirectToAction("ThatBai");
                }
            }
        }



        [HttpGet]
        public ActionResult HoanTat()
        {
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
