using CuaHangBanGiay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangBanGiay.Areas.Admin.Controllers
{
    public class TinhTrangDonHangController : Controller
    {
        // GET: Admin/TinhTrangDonHang
        ShoesShopEntities db = new ShoesShopEntities();
        public ActionResult Index()
        {
            var dsDH = db.tb_DonMuaHang.Where(s => s.TrangThai != 0 && s.TrangThai!=-1).ToList();
            return View(dsDH);
        }
        public ActionResult CapNhatDH(int id)
        {
            var dh = db.tb_DonMuaHang.Find(id);

            return PartialView(dh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatDH(FormCollection fields)
        {
            int ID = int.Parse(fields["ID"]);
            var donhang = db.tb_DonMuaHang.Where(s => s.ID == ID).SingleOrDefault();
            if (donhang != null)
            {
                donhang.TrangThai = int.Parse(fields["TT"]);
                db.Entry(donhang).State = System.Data.Entity.EntityState.Modified;
            }

            db.SaveChanges();


            return RedirectToAction("Index");
        }


    }
}