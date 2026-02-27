using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanDoUong_User.Models;


namespace BanDoUong_User.Controllers
{
    public class TAI_KHOANController : Controller
    {
        private QL_BAN_DO_UONGDb db = new QL_BAN_DO_UONGDb();


        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(string taiKhoan, string matKhau)
        {
            if (string.IsNullOrEmpty(taiKhoan) ||
    string.IsNullOrEmpty(matKhau))
            {
                ViewBag.ThongBaoTaiKhoan = "Vui lòng nhập đầy đủ thông tin!";
                return View();
            }

            var ketQua = (from a in db.TAI_KHOAN
                          where (a.ten_dang_nhap == taiKhoan && a.mat_khau == matKhau)
                          select a).FirstOrDefault();
            
            if (ketQua == null)
            {
                ViewBag.ThongBaoTaiKhoan = "Tài khoản hoặc mật khẩu không chính xác!!";
                return View();
            }

            else
            {
                Session["UserId"] = ketQua.id;
                int userId = ketQua.id;
                Session["NguoiDung"] = ketQua.ho_ten;

                

                Session["so_luong"] = db.CHI_TIET_GIO_HANG
    .Count(x => x.GIO_HANG.tai_khoan_id == ketQua.id);

                return RedirectToAction("Index", "SAN_PHAM");
            }
            
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(string taiKhoan, string matKhau, string hoTen, string email, string soDienThoai)
        {
            if (string.IsNullOrEmpty(taiKhoan) ||
    string.IsNullOrEmpty(matKhau) ||
    string.IsNullOrEmpty(hoTen) ||
                    string.IsNullOrEmpty(email) || string.IsNullOrEmpty(soDienThoai))
            {
                ViewBag.ThongBaoTaiKhoan = "Vui lòng nhập đầy đủ thông tin!";
                return View();
            }

            var ketQua = (from a in db.TAI_KHOAN
                          where a.ten_dang_nhap == taiKhoan
                          select a).FirstOrDefault();
            if (ketQua != null)
            {
                ViewBag.ThongBaoTaiKhoan = "Tài khoản đã tồn tại!!!";
                return View();
            }
            else
            {
TAI_KHOAN tk = new TAI_KHOAN(taiKhoan, matKhau, hoTen, email, soDienThoai);
            db.TAI_KHOAN.Add(tk);
            db.SaveChanges();

            return RedirectToAction("DangNhap");
            }
            
        }

        public ActionResult DangXuat()
        {
            Session.Clear();      // Xóa toàn bộ session
            Session.Abandon();    // Hủy session

            return RedirectToAction("Index", "SAN_PHAM");
        }




    }
}
