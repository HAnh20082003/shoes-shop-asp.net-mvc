using CuaHangBanGiay.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangBanGiay.Areas.Admin.Controllers
{
    public class MatHangController : Controller
    {
        // GET: Admin/MatHang
        ShoesShopEntities db = new ShoesShopEntities();


        public ActionResult Index()
        {
            var dsMatHang = db.tb_MatHang.ToList();
            return View(dsMatHang);
        }



        public ActionResult ThemMatHang()
        {
            var danhmuc = db.tb_DanhMuc.ToList();
            ViewBag.DanhMuc = new SelectList(danhmuc, "ID", "TenDanhMuc");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ThemMatHang(FormCollection fields,HttpPostedFileBase AnhMH)
        {
            var TenMH = fields["TenMH"];
            var DanhMuc = fields["DanhMuc"];
            var Gia = fields["GiaBan"];
            var MoTa = fields["MoTa"];

            tb_MatHang mh = new tb_MatHang();

            mh.TenMatHang = TenMH;
            mh.ID_DanhMuc = int.Parse(DanhMuc);
            mh.Gia = decimal.Parse(Gia);
            mh.MoTa = MoTa;
            mh.NgayThem = DateTime.Now;
            mh.Rating = Init.maxRating;
            mh.SoLuongSanPham = 0;
            mh.DaBan = 0;
            mh.GiamGia = 0;
            if (AnhMH != null)
            {
                string _pathMH = Path.Combine(Server.MapPath("../../" + Init.folderImageProduct), TenMH);
                if (!System.IO.Directory.Exists(_pathMH))
                {
                    Directory.CreateDirectory(_pathMH);
                }
                string filename = Path.GetFileName(AnhMH.FileName);
                mh.AnhMoTa = filename;
                string _path = Path.Combine(_pathMH, filename);
                AnhMH.SaveAs(_path);
            }

            db.tb_MatHang.Add(mh);
            try
            {
                db.SaveChanges();
                Session["ThemMH"] = "Thêm mặt hàng " + TenMH + " thành công";
            }
            catch
            {
                Session["LoiThemMH"] = "Không thể thêm mặt hàng " + TenMH;
            }

            return RedirectToAction("Index", "MatHang");
        }



        public ActionResult SuaMatHang(int id)
        {
            var mathang = db.tb_MatHang.Find(id);
            var danhmuc = db.tb_DanhMuc.ToList();
            ViewBag.DanhMuc = new SelectList(danhmuc, "ID", "TenDanhMuc");


            return PartialView(mathang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]    
        public ActionResult SuaMatHang(FormCollection fields, HttpPostedFileBase AnhMH)
        {

            var ID = int.Parse(fields["ID"]);
            var Gia = decimal.Parse(fields["GiaMH"]);
            var MoTa = fields["SuaMoTa"];
            var TenMH = fields["TenMH"];
            var GiamGia = fields["GiamGia"];
            var DanhMuc = fields["DanhMuc"];
            var mathang = db.tb_MatHang.Find(ID);

            if (!TenMH.Equals(mathang.TenMatHang))
            {

                string _CurrentPath = Path.Combine(Server.MapPath("../../../" + Init.folderImageProduct), mathang.TenMatHang);
                string _ChangePath = Path.Combine(Server.MapPath("../../../" + Init.folderImageProduct), TenMH);
                Directory.Move(_CurrentPath, _ChangePath);
                Directory.Delete(_CurrentPath,true);
                mathang.TenMatHang = TenMH;

            }

            mathang.Gia = Gia;
            mathang.MoTa = MoTa;
            mathang.GiamGia = int.Parse(GiamGia);
            mathang.ID_DanhMuc = int.Parse(DanhMuc);
            if (AnhMH != null)
            {
                string _pathMH = Path.Combine(Server.MapPath("../../" + Init.folderImageProduct), TenMH);
                if (System.IO.File.Exists(Path.Combine(_pathMH, mathang.AnhMoTa)))
                {
                    System.IO.File.Delete(Path.Combine(_pathMH, mathang.AnhMoTa));
                }
                string filename = Path.GetFileName(AnhMH.FileName);
                mathang.AnhMoTa = filename;
                string _path = Path.Combine(_pathMH, filename);
                AnhMH.SaveAs(_path);
            }

            db.Entry(mathang).State = System.Data.Entity.EntityState.Modified;
            try
            {
                db.SaveChanges();
                Session["SuaMH"] = "Sửa mặt hàng thành công";
            }
            catch
            {
                Session["LoiSuaMH"] = "Không thể sửa mặt hàng này";
            }




            return RedirectToAction("Index", "MatHang");
        }


        public ActionResult XemChiTiet(int? id)
        {
            var mathang = db.tb_MatHang.Find(id);
            return View(mathang);
        }

        public ActionResult DanhSachAnh(int id)
        {
            ViewBag.ID = id;
            ViewBag.MatHang = db.tb_MatHang.Find(id);
            var dsAnh = db.tb_AnhMatHang.Where(s => s.ID_MatHang == id).ToList();
            return PartialView(dsAnh);
            
        }

        [HttpPost]
        public ActionResult ThemAnhCTMH(FormCollection fields, HttpPostedFileBase [] AnhMH)
        {
            var ID = int.Parse(fields["ID"]);
            var mathang = db.tb_MatHang.Find(ID);
            

            foreach(var item in AnhMH)
            {
                tb_AnhMatHang anhMH = new tb_AnhMatHang();
                anhMH.ID_MatHang = ID;
                
                string _pathMH = Path.Combine(Server.MapPath("../../" + Init.folderImageProduct), mathang.TenMatHang);
                string filename = Path.GetFileName(item.FileName);
                anhMH.DuongDanAnh = filename;
                string _path = Path.Combine(_pathMH, filename);
                item.SaveAs(_path);
                db.tb_AnhMatHang.Add(anhMH);

            }
            db.SaveChanges();
            return RedirectToAction("XemChiTiet","MatHang", new { id = ID });
        }

      
        public ActionResult XoaAnhCTMH(int id, int idMH)
        {
            var AnhCTMH = db.tb_AnhMatHang.Find(id);
            if (AnhCTMH != null)
            {
                db.tb_AnhMatHang.Remove(AnhCTMH);
            }
            db.SaveChanges();
            return RedirectToAction("XemChiTiet","MatHang", new {id = idMH });
        }

        public ActionResult DanhSachSanPham(int id)
        {
            var dsSP = db.tb_SanPham.Where(s => s.ID_MatHang == id).ToList();
            ViewBag.ID = id;
            ViewBag.KichThuoc = db.tb_BangKichThuoc.ToList();
            ViewBag.MauSac = db.tb_BangMauSP.ToList();
            ViewBag.GioiTinh = db.tb_BangGioiTinh.ToList();
            return PartialView(dsSP);
        }


        [HttpPost]
        public ActionResult ThemSP(FormCollection fields)
        {
            int ID = int.Parse(fields["ID"]);
            int KichThuoc = int.Parse(fields["KichThuoc"]);
            int MauSac = int.Parse(fields["MauSac"]);
            int GioiTinh = int.Parse(fields["GioiTinh"]);
            int SoLuong = int.Parse(fields["SoLuong"]);
            var mh = db.tb_MatHang.SingleOrDefault(n => n.ID == ID);
            if(mh.tb_SizeMatHang.FirstOrDefault(s=>s.ID_KichThuoc == KichThuoc) == null)
            {
                tb_SizeMatHang size = new tb_SizeMatHang();
                size.ID_MatHang = ID;
                size.ID_KichThuoc = KichThuoc;
                db.tb_SizeMatHang.Add(size);
                db.SaveChanges();
            }
            if (mh.tb_MauMatHang.FirstOrDefault(s => s.ID_MauSac == MauSac) == null)
            {
                tb_MauMatHang mau = new tb_MauMatHang();
                mau.ID_MatHang = ID;
                mau.ID_MauSac = MauSac;
                db.tb_MauMatHang.Add(mau);
                db.SaveChanges();
            }
            if (mh.tb_GioiTinhMatHang.FirstOrDefault(s => s.ID_GioiTinh == GioiTinh) == null)
            {
                tb_GioiTinhMatHang gt = new tb_GioiTinhMatHang();
                gt.ID_MatHang = ID;
                gt.ID_GioiTinh = GioiTinh;
                db.tb_GioiTinhMatHang.Add(gt);
                db.SaveChanges();
            }

            for (int i = 0; i < SoLuong; i++)
            {
                tb_SanPham sp = new tb_SanPham();
                sp.ID_MatHang = ID;
                sp.ID_KichCo = mh.tb_SizeMatHang.SingleOrDefault(n => n.ID_KichThuoc == KichThuoc).ID;
                sp.ID_MauSac = mh.tb_MauMatHang.SingleOrDefault(n => n.ID_MauSac == MauSac).ID;
                sp.ID_GioiTinh = mh.tb_GioiTinhMatHang.SingleOrDefault(n => n.ID_GioiTinh == GioiTinh).ID;
                sp.TrangThai = false;
                db.tb_SanPham.Add(sp);
            }

            db.SaveChanges();


            return RedirectToAction("XemChiTiet", "MatHang", new { id = ID });
        }

        //public ActionResult XoaSanPham(int id)
        //{
        //    var xoasp = db.tb_SanPham.Find(id);
        //    int? idMH = xoasp.ID_MatHang;

        //    if (xoasp != null)
        //    {
        //        //var mathang = xoasp.ID_MatHang;
        //        //var kichthuoc = xoasp.ID_KichCo;
        //        //var mausac = xoasp.ID_MauSac;
        //        //var gioitinh = xoasp.ID_GioiTinh;
        //        db.tb_SanPham.Remove(xoasp);
                
               
        //        try
        //        {
        //            db.SaveChanges();
                   

        //            Session["XoaSP"] = "Xóa sản phẩm thành công";
        //        }
        //        catch
        //        {
        //            Session["LoiXoaSP"] = "Không thể xóa sản phẩm";
        //        }
        //    }


        //    return RedirectToAction("XemChiTiet", "MatHang", new { id = idMH });
        //}

        public ActionResult XoaSanPham(int id)
        {
            var sp = db.tb_SanPham.Find(id);
            int? idMH = sp.ID_MatHang;

            var id_kichco = sp.ID_KichCo;
            var id_mausac = sp.ID_MauSac;
            var id_gioitinh = sp.ID_GioiTinh;


            db.tb_SanPham.Remove(sp);
            try
            {
                db.SaveChanges();


                Session["XoaSP"] = "Xóa sản phẩm thành công";
            }
            catch
            {
                Session["LoiXoaSP"] = "Không thể xóa sản phẩm";
            }

            var kc = db.tb_SanPham.Where(n => n.ID_MatHang == idMH && n.ID_KichCo == id_kichco).Count();
            var ms = db.tb_SanPham.Where(n => n.ID_MatHang == idMH && n.ID_MauSac == id_mausac).Count();
            var gt = db.tb_SanPham.Where(n => n.ID_MatHang == idMH && n.ID_GioiTinh == id_gioitinh).Count();

            if (kc == 0)
            {
                var tmp = db.tb_SizeMatHang.FirstOrDefault(n => n.ID_MatHang == idMH && n.ID == id_kichco);
                db.tb_SizeMatHang.Remove(tmp);
            }
            if (ms == 0)
            {
                var tmp = db.tb_MauMatHang.FirstOrDefault(n => n.ID_MatHang == idMH && n.ID == id_mausac);
                db.tb_MauMatHang.Remove(tmp);
            }
            if (gt == 0)
            {
                var tmp = db.tb_GioiTinhMatHang.FirstOrDefault(n => n.ID_MatHang == idMH && n.ID == id_gioitinh);
                db.tb_GioiTinhMatHang.Remove(tmp);
            }
            try
            {
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("XemChiTiet", "MatHang", new { id = idMH });
        }



        public ActionResult XoaMH(int id)
        {
            var mh = db.tb_MatHang.Find(id);
            return PartialView(mh);
        }


        public ActionResult CFXoaMH(int id)
        {
            var mh = db.tb_MatHang.Find(id);

            if (mh != null)
            {
                string ten = mh.TenMatHang;
                if (mh.tb_SanPham.Count() == 0 && mh.tb_MoTaDonHang.Count()==0)
                {
                    if (Directory.Exists(Path.Combine(Server.MapPath("~/Content/imgs/sanpham"), mh.TenMatHang)))
                    {
                        Directory.Delete(Path.Combine(Server.MapPath("~/Content/imgs/sanpham"), mh.TenMatHang),true);
                    }
                    db.tb_AnhMatHang.RemoveRange(mh.tb_AnhMatHang);                    
                    db.tb_SizeMatHang.RemoveRange(mh.tb_SizeMatHang);
                    db.tb_MauMatHang.RemoveRange(mh.tb_MauMatHang);
                    db.tb_GioiTinhMatHang.RemoveRange(mh.tb_GioiTinhMatHang);
                    db.tb_DanhGiaTraiNghiem.RemoveRange(mh.tb_DanhGiaTraiNghiem);
                    db.SaveChanges();
                        
                }
                db.tb_MatHang.Remove(mh);
                try
                {
                    db.SaveChanges();

                    Session["XoaMH"] = "Xóa mặt hàng " + ten + " thành công";
                }
                catch
                {
                    Session["LoiXoaMH"] = "Không thể xóa mặt hàng " + ten;
                }


            }

            return RedirectToAction("Index");
        }
    }
}