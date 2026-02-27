using BanDoUong_User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDoUong_User.ViewModels
{
	public class ThanhToanVM
	{
        // Thông tin người nhận
        public string NguoiNhan { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChiCuThe { get; set; }
        public string GhiChu { get; set; }

        // Thanh toán
        public string PhuongThucThanhToan { get; set; }

        // Danh sách sản phẩm được chọn
        public List<GioHangItemVM> Items { get; set; }

        public decimal TongTien { get; set; }
    }

    public class GioHangItemVM
    {
        public int ChiTietGioHangId { get; set; }

        public int SanPhamId { get; set; }
        public string TenSanPham { get; set; }
        public string HinhAnh { get; set; }

        public int SizeId { get; set; }
        public string TenSize { get; set; }

        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }

        // Topping
        public List<TOPPING> Toppings { get; set; }
        public List<int> SelectedToppingIds { get; set; }
    }
}