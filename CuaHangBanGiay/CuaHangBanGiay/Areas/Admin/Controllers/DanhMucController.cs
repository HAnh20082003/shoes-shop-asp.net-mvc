using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CuaHangBanGiay.Models;

namespace CuaHangBanGiay.Areas.Admin.Controllers
{
    public class DanhMucController : Controller
    {
        private ShoesShopEntities db = new ShoesShopEntities();

        // GET: Admin/DanhMuc
        public ActionResult Index()
        {
            return View(db.tb_DanhMuc.ToList());
        }

      

        public ActionResult ThemDanhMuc() {

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemDanhMuc(FormCollection fields, HttpPostedFileBase Anh)
        {
            var TenDanhMuc = fields["TenDM"];

            tb_DanhMuc danhmuc = new tb_DanhMuc();
            danhmuc.TenDanhMuc = TenDanhMuc;
            danhmuc.SoLuongMatHang = 0;
            if (Anh != null)
            {
                string filename = Path.GetFileName(Anh.FileName);
                string _path = Path.Combine(Server.MapPath("../../" + Init.folderImageCategory), filename);
                Anh.SaveAs(_path);
                danhmuc.AnhMoTa = filename;

            }
            db.tb_DanhMuc.Add(danhmuc);
            try
            {
                db.SaveChanges();
                Session["ThemDanhMuc"] = "Thêm danh mục thành công";
            }
            catch
            {
                Session["LoiThemDanhMuc"] = "Không thể thêm danh mục";
            }
            return RedirectToAction("Index","DanhMuc");
        }

        
        public ActionResult XoaDanhMuc(int? id)
        {
            var danhmuc = db.tb_DanhMuc.Find(id);

            return PartialView(danhmuc);
        }

    
        public ActionResult CFXoaDanhMuc(int id)
        {
            var danhmuc = db.tb_DanhMuc.Find(id);


            if (danhmuc != null)
            {
                db.tb_DanhMuc.Remove(danhmuc);

            }


            try
            {
                db.SaveChanges();
                Session["XoaDanhMuc"] = "Xóa danh mục thành công";
            }
            catch
            {
                Session["LoiXoaDanhMuc"] = "Không thể xóa danh mục";
            }

            
            return RedirectToAction("Index","DanhMuc");
        }



        public ActionResult SuaDanhMuc(int id)
        {
            var danhmuc = db.tb_DanhMuc.Find(id);
            return PartialView(danhmuc);
        }

        [HttpPost]
        public ActionResult SuaDanhMuc(FormCollection fields,HttpPostedFileBase Anh)
        {

            int Id = int.Parse(fields["ID"]);
            tb_DanhMuc danhmuc = db.tb_DanhMuc.Find(Id);

            danhmuc.TenDanhMuc = fields["TenDM"];


            if (Anh != null)
            {
                string filename = Path.GetFileName(Anh.FileName);
                string _path = Path.Combine(Server.MapPath("../../../" + Init.folderImageCategory), filename);
                Anh.SaveAs(_path);
                danhmuc.AnhMoTa = filename;

            }
            db.Entry(danhmuc).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                Session["SuaDanhMuc"] = "Sửa danh mục thành công";
            }
            catch
            {
                Session["LoiSuaDanhMuc"] = "Không thể sửa danh mục";
            }

            return RedirectToAction("Index", "DanhMuc");
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
