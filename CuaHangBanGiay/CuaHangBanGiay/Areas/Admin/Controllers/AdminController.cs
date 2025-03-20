using CuaHangBanGiay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangBanGiay.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        ShoesShopEntities db = new ShoesShopEntities();
        // GET: Admin/Admin
        public ActionResult Index()
        {
            ViewBag.MatHang = db.tb_MatHang.Count();
            ViewBag.SPDB = db.tb_SanPham.Where(s => s.TrangThai == true).Count();
            ViewBag.KH = db.tb_NguoiDung.Where(s => s.TrangThai == true).Count();
            ViewBag.DH = db.tb_DonMuaHang.Where(s => s.TrangThai == -1).Count();

            var DanhGia = db.tb_DanhGiaTraiNghiem.OrderByDescending(s => s.NgayDanhGia).Take(10).ToList();
            var MatHang = db.tb_MatHang.OrderByDescending(s => s.tb_SanPham.Where(t => t.TrangThai == true).Count()).Take(10).ToList();

            ViewBag.DanhGia = DanhGia;
            ViewBag.MHBC = MatHang;

            return View();
        }
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(FormCollection fields)
        {

            var tendangnhap = fields["TenDangNhap"];
            var matkhau = fields["MatKhau"];

            var Admin = db.tb_Admin.Where(s => s.TenDangNhap.ToString().Equals(tendangnhap) && s.MatKhau.ToString().Equals(matkhau)).SingleOrDefault();
            if (Admin == null)
            {
                ViewBag.Loi = "Sai tài khoản hoặc mật khẩu";
                return View();
            }

            Session["Admin"] = Admin.TenDangNhap;

            return RedirectToAction("Index");
        }
    }
}