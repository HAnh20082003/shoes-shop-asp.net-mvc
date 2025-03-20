using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangBanGiay.Models;
namespace CuaHangBanGiay.Areas.Admin.Controllers
{
    public class BannerController : Controller
    {
        ShoesShopEntities db = new ShoesShopEntities();
        // GET: Admin/Banner
        public ActionResult Index()
        {
            var banner = db.tb_Banner.ToList();
            return View(banner);
        }
        [HttpPost]
        public ActionResult ThemBanner(HttpPostedFileBase[] Anh)
        {
            if (Anh != null)
            {
                foreach (var item in Anh)
                {
                    tb_Banner tmp = new tb_Banner();

                    string filename = Path.GetFileName(item.FileName);
                    tmp.AnhBanner = filename;
                    string _path = Path.Combine(Server.MapPath("../../" + Init.folderBanner), filename);
                    item.SaveAs(_path);
                    db.tb_Banner.Add(tmp);


                }
                try
                {
                    db.SaveChanges();
                    Session["ThemBanner"] = "Đã thêm " + Anh.Count() + " ảnh";
                }
                catch
                {
                    Session["LoiThemBanner"] = "Không thể thêm ảnh";
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult XoaBanner(int? id)
        {
            var banner = db.tb_Banner.Find(id);

            if (banner != null)
            {
                db.tb_Banner.Remove(banner);
                try
                {
                    db.SaveChanges();
                    Session["XoaBanner"] = "Xóa banner thành công";
                }
                catch
                {

                }
            }

            return RedirectToAction("Index");
        }
    }
}