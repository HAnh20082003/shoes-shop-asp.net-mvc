using CuaHangBanGiay.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangBanGiay.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        ShoesShopEntities db = new ShoesShopEntities();
        // GET: Admin/KhachHang
        public ActionResult Index()
        {
            var danhsachKH = db.tb_NguoiDung.ToList();
            return View(danhsachKH);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ThemKH(FormCollection fields, HttpPostedFileBase Anh)
        {


            tb_NguoiDung kh = new tb_NguoiDung();

            kh.TenDangNhap = fields["TenDN"];
            kh.MatKhau = fields["MatKhau"];
            kh.TenHienThi = fields["HoTen"];
            kh.SDT = fields["SDT"];
            kh.Email = fields["Email"];
            kh.GioiTinh = int.Parse(fields["GioiTinh"]);
            kh.NgaySinh = DateTime.Parse(fields["NgaySinh"]);
            kh.DiaChi = fields["DiaChi"];
            kh.TrangThai = true;
            if (Anh != null)
            {

                string filename = Path.GetFileName(Anh.FileName);
                kh.AnhDaiDien = filename;
                string _path = Path.Combine(Server.MapPath("../../" + Init.folderAvatarUser), filename);
                Anh.SaveAs(_path);
            }
            else
            {
                kh.AnhDaiDien = null;
            }

            db.tb_NguoiDung.Add(kh);
            try
            {
                db.SaveChanges();
                Session["ThemKH"] = "Thêm khách hàng thành công";
            }
            catch
            {
                Session["LoiThemKH"] = "Không thể thêm khách hàng";
            }


            return RedirectToAction("Index");
        }


        public ActionResult SuaThongTin(int id)
        {
            var kh = db.tb_NguoiDung.Find(id);
            return PartialView(kh);
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SuaThongTin(FormCollection fields, HttpPostedFileBase Anh)
        {

            var kh = db.tb_NguoiDung.Find(int.Parse(fields["ID"]));

            if (kh != null)
            {

                kh.MatKhau = fields["MatKhau"];
                kh.TenHienThi = fields["HoTen"];
                kh.SDT = fields["SDT"];
                kh.Email = fields["Email"];
                kh.GioiTinh = int.Parse(fields["GioiTinh"]);
                kh.NgaySinh = DateTime.Parse(fields["NgaySinh"]);
                kh.DiaChi = fields["DiaChi"];

                if (Anh != null)
                {
                    if (kh.AnhDaiDien != null)
                    {
                        string _pathKH = Path.Combine(Server.MapPath("../../../" + Init.folderAvatarUser), kh.AnhDaiDien);
                        if (System.IO.File.Exists(Path.Combine(_pathKH, kh.AnhDaiDien)))
                        {
                            System.IO.File.Delete(Path.Combine(_pathKH, kh.AnhDaiDien));
                        }
                    }
                    string filename = Path.GetFileName(Anh.FileName);
                    kh.AnhDaiDien = filename;
                    string _path = Path.Combine(Server.MapPath("../../../" + Init.folderAvatarUser), filename);
                    Anh.SaveAs(_path);
                }


                db.Entry(kh).State = System.Data.Entity.EntityState.Modified;
            }


            try
            {
                db.SaveChanges();
                Session["SuaKH"] = "Sửa thông tin thành công";
            }
            catch
            {
                Session["LoiSuaKH"] = "Không thể sửa thông tin";
            }

            return RedirectToAction("Index");
        }


        public ActionResult XoaKH(int id)
        {
            var kh = db.tb_NguoiDung.Find(id);
            return PartialView(kh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XoaKH(FormCollection fields)
        {
            var kh = db.tb_NguoiDung.Find(int.Parse(fields["ID"]));
            if (kh != null)
            {
                kh.TrangThai = false;
                db.Entry(kh).State = System.Data.Entity.EntityState.Modified;

                try
                {
                    db.SaveChanges();
                    Session["XoaKH"] = "Ngừng hoạt động thành công";
                }
                catch
                {

                }

            }

            return RedirectToAction("Index");
        }


        public ActionResult ChiTietKH(int id)
        {
            var kh = db.tb_NguoiDung.Find(id);
            return View(kh);
        }

        [HttpPost]
        public ActionResult CapNhatGoBan(FormCollection collection)
        {
            var tk = db.tb_NguoiDung.Where(n => n.TrangThai == false);
            foreach (var item in tk)
            {
                if (string.IsNullOrEmpty(collection["id-goban-" + item.ID]))
                {
                    var tmp = db.tb_NguoiDung.FirstOrDefault(n => n.ID == item.ID);
                    tmp.TrangThai = true;
                    db.Entry(tmp).State = System.Data.Entity.EntityState.Modified;
                }
            }
            db.SaveChanges();
            Session["SuccessGoBan"] = "Đã cho hoạt động lại tài khoản thành công";
            return RedirectToAction("Index");
        }

    }
}