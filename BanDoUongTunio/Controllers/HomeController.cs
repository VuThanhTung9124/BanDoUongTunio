using BanDoUongTunio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanDoUongTunio.Models;

namespace BanDoUongTunio.Controllers
{
    public class HomeController : Controller
    {
        private QL_BAN_DO_UONGDb db = new QL_BAN_DO_UONGDb();
        // GET: Home
        public ActionResult Index()
        {

            var so_luong_san_pham = db.SAN_PHAM.Count();
            var so_luong_danh_muc = db.DANH_MUC.Count();
            var so_luong_don_hang = db.DON_HANG.Count();

            Session["So_luong_san_pham"] = 0;
            if (so_luong_san_pham != null)
            {
                Session["So_luong_san_pham"] = so_luong_san_pham;
                
            }

            Session["So_luong_danh_muc"] = 0;
            if (so_luong_san_pham != null)
            {
                Session["So_luong_danh_muc"] = so_luong_danh_muc;

            }

            Session["So_luong_don_hang"]  = 0;
            if (so_luong_san_pham != null)
            {
                Session["So_luong_don_hang"] = so_luong_don_hang;

            }

            //Session["So_luong_san_pham"] = so_luong_san_pham;
            //Session["So_luong_danh_muc"] = so_luong_danh_muc;
            //Session["So_luong_don_hang"] = so_luong_don_hang;
            return View();
        }
    }
}