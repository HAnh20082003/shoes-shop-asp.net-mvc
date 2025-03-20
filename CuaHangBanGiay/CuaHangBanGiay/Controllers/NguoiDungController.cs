using CuaHangBanGiay.Models;
using log4net;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangBanGiay.Controllers
{
    public class NguoiDungController : Controller
    {
        ShoesShopEntities db = new ShoesShopEntities();
        public static Giohang gioHang = new Giohang();
        public static List<int?> id_matHangYeuThichs = new List<int?>();
        public static List<tb_MatHang> spVuaXems = new List<tb_MatHang>();
        public static List<tb_DonMuaHang> donhang_ad = new List<tb_DonMuaHang>();

        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection formCollection)
        {
            // Lấy dữ liệu từ FormCollection
            string tenDangNhap = formCollection["tendn"];
            string matKhau = formCollection["matkhau"];
            string tenHienThi = formCollection["tenhienthi"];
            HttpPostedFileBase file = Request.Files["anhdaidien"];
            string sodienthoai = formCollection["sdt"];
            string email = formCollection["email"];
            int gioitinh = int.Parse(formCollection["gioitinh"]);
            DateTime ngaysinh = DateTime.Parse(formCollection["ngaysinh"]);
            string diachi = formCollection["diachi"];

            if (db.tb_NguoiDung.FirstOrDefault(n => n.TenDangNhap == tenDangNhap) != null)
            {
                Session["ErrorDangKy"] = "Tên đăng nhập đã được sử dụng";
                return View("DangKy");
            }
            tb_NguoiDung nd = new tb_NguoiDung();
            nd.DiaChi = diachi;
            nd.Email = email;
            nd.GioiTinh = gioitinh;
            nd.MatKhau = matKhau;
            nd.NgaySinh = ngaysinh;
            nd.SDT = sodienthoai;
            nd.TenDangNhap = tenDangNhap;
            nd.TenHienThi = tenHienThi;
            nd.TrangThai = true;
            db.tb_NguoiDung.Add(nd);
            db.SaveChanges();
            if (file != null && file.ContentLength != 0)
            {
                file.SaveAs(Server.MapPath("../" + Init.folderAvatarUser + nd.ID + ".png"));
                nd.AnhDaiDien = nd.ID + ".png";
                db.Entry(nd).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            
            Session["SuccessDangKy"] = "Đăng ký thành công";
            return RedirectToAction("DangNhap");
        }
        public ActionResult DangNhap(int? idsp)
        {
            Session["idSP"] = idsp;
            return View();
        }

        public void KiemTraDangNhap(string tendn, string matkhau)
        {
            var tk = db.tb_NguoiDung.SingleOrDefault(n => n.TenDangNhap == tendn && n.MatKhau == matkhau);
            if (tk == null)
            {
                Session["ErrorLogin"] = "Tài khoản đăng nhập không tồn tại";
                Response.Redirect("~/DangNhap");
            }
            else
            {
                Session["SuccessDangNhap"] = "Đăng nhâp tài khoản thành công";
                Session["Taikhoan"] = tk;
                if (Session["idSP"] == null)
                {
                    Response.Redirect("~/TrangChu");
                }
                else
                {
                    Response.Redirect("~/XemSanPham?page=1&id_matHang=" + Session["idSP"]);
                    //RedirectToAction("XemSanPham", new { id_matHang = Session["idSP"] });
                }
            }
        }
        public ActionResult DangXuat()
        {
            Session["Taikhoan"] = null;
            Session["SuccessDangXuat"] = "Đã đăng xuất tài khoản";
            return RedirectToAction("TrangChu", "NguoiDung");
        }

        public ActionResult XemThongTin()
        {
            var tk = (tb_NguoiDung)Session["Taikhoan"];
            return View(tk);
        }

        [HttpPost] 
        public ActionResult CapNhatThongTin(FormCollection collection)
        {
            string matKhau = collection["matkhau"];
            string tenHienThi = collection["tenhienthi"];
            HttpPostedFileBase file = Request.Files["anhdaidien"];
            string sodienthoai = collection["sdt"];
            string email = collection["email"];
            int gioitinh = int.Parse(collection["gioitinh"]);
            DateTime ngaysinh = DateTime.Parse(collection["ngaysinh"]);
            string diachi = collection["diachi"];
            var tmp = (tb_NguoiDung)Session["Taikhoan"];
            var tk = db.tb_NguoiDung.SingleOrDefault(n => n.ID == tmp.ID);
            tk.MatKhau = matKhau;
            tk.TenHienThi = tenHienThi;
            tk.SDT = sodienthoai;
            tk.Email = email;
            tk.GioiTinh = gioitinh;
            tk.NgaySinh = ngaysinh;
            tk.DiaChi = diachi;
            db.Entry(tk).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            if (file != null && file.ContentLength != 0)
            {
                if (System.IO.File.Exists(Server.MapPath("../" + Init.folderAvatarUser + tk.ID + ".png")))
                {
                    System.IO.File.Delete(Server.MapPath("../" + Init.folderAvatarUser + tk.ID + ".png"));
                }
                file.SaveAs(Server.MapPath("../" + Init.folderAvatarUser + tk.ID + ".png"));
            }
            Session["Taikhoan"] = tk;
            Session["SuccessCapNhat"] = "Cập nhật thông tin thành công";
            return RedirectToAction("XemThongTin");
        }

        //Trang chủ
       
        public ActionResult TrangChu()
        {
            Session["GioHang"] = gioHang.matHang.Count == 0 ? null : gioHang;
            var danhMuc = db.tb_DanhMuc;
            ViewBag.Banner = db.tb_Banner.ToList();
            return View(danhMuc);
        }

        //Trang cửa hàng
        public ActionResult CuaHang(int? id_danhMuc, string keyword, int? page, int? sort)
        {
            ViewBag.ID_DanhMuc = id_danhMuc;
            Session["keyword"] = keyword;
            Session["GioHang"] = gioHang.matHang.Count == 0 ? null : gioHang;
            IEnumerable<tb_MatHang> mat_hangs;
            if (id_danhMuc != null)
            {
                mat_hangs = db.tb_MatHang.Where(n => n.ID_DanhMuc == id_danhMuc);
                ViewBag.TenDanhMuc = db.tb_DanhMuc.SingleOrDefault(n => n.ID == id_danhMuc).TenDanhMuc;
            }
            else if (keyword != null)
            {
                mat_hangs = db.tb_MatHang.Where(n => n.TenMatHang.Contains(keyword));
            }
            else
            {
                mat_hangs = db.tb_MatHang;
            }
            ViewBag.TrangHienTai = page ?? 1;

            if (sort == null || sort == 1)
            {
                mat_hangs = mat_hangs.OrderByDescending(n => n.NgayThem);
                ViewBag.SapXep = 1;
            }
            else
            {
                mat_hangs = mat_hangs.OrderByDescending(n => n.Rating);
                ViewBag.SapXep = sort;
            }

            return View(mat_hangs);
        }

        //Load các bảng lọc
        public ActionResult LoadBangGiaPartial(int? id_danhMuc)
        {
            if (id_danhMuc != null)
            {
                ViewBag.ID_DanhMuc = id_danhMuc;
            }
            var banggia = db.tb_BangGiaSP.ToList().OrderBy(n => n.MinGia);
            return PartialView(banggia);
        }

        public ActionResult LoadBangMauSacPartial(int? id_danhMuc)
        {
            if (id_danhMuc != null)
            {
                ViewBag.ID_DanhMuc = id_danhMuc;
            }
            var bangmausac = db.tb_BangMauSP.ToList();
            return PartialView(bangmausac);
        }

        public ActionResult LoadBangKichThuocPartial(int? id_danhMuc)
        {
            if (id_danhMuc != null)
            {
                ViewBag.ID_DanhMuc = id_danhMuc;
            }
            var bangkichthuoc = db.tb_BangKichThuoc.ToList();
            return PartialView(bangkichthuoc);
        }

        public ActionResult LoadBangGioiTinhPartial(int? id_danhMuc)
        {
            if (id_danhMuc != null)
            {
                ViewBag.ID_DanhMuc = id_danhMuc;
            }
            var banggioitinh = db.tb_BangGioiTinh.ToList();
            return PartialView(banggioitinh);
        }

        //Load danh mục sản phẩm
        public ActionResult LoadDanhMucPartial()
        {
            var danhMuc = db.tb_DanhMuc.ToList();
            return PartialView(danhMuc);
        }
        
        //Load danh sách sản phẩm có điều kiện
        public ActionResult LoadDanhSachSanPhamPartial(int? page, int? id_danhMuc, string keyword, int sort, int? id_price, int? id_gender, int? id_size, int? id_color)
        {
            IEnumerable<tb_MatHang> dsmh;
            if (string.IsNullOrEmpty(keyword))
            {
                dsmh = db.tb_MatHang;
            }
            else
            {
                dsmh = db.tb_MatHang.Where(n => n.TenMatHang.Contains(keyword));
            }
            if (id_danhMuc != null)
            {
                dsmh = dsmh.Where(s => s.ID_DanhMuc == id_danhMuc);
            }
            if (id_price != null)
            {
                var price = db.tb_BangGiaSP.SingleOrDefault(n => n.ID == id_price);
                dsmh = dsmh.Where(n => Init.layTienGiamGia(n.Gia, n.GiamGia) >= price.MinGia && Init.layTienGiamGia(n.Gia, n.GiamGia) <= price.MaxGia);
            }

            if (id_gender != null)
            {
                dsmh = dsmh.Where(n => n.tb_GioiTinhMatHang.FirstOrDefault(s => s.ID_GioiTinh == id_gender) != null);
            }

            if (id_size != null)
            {
                dsmh = dsmh.Where(n => n.tb_SizeMatHang.FirstOrDefault(s => s.ID_KichThuoc == id_size) != null);
            }

            if (id_color != null)
            {
                dsmh = dsmh.Where(n => n.tb_MauMatHang.FirstOrDefault(s => s.ID_MauSac == id_color) != null);
            }
            if (sort == 1)
            {
                dsmh = dsmh.OrderByDescending(n => n.NgayThem);
            }
            else
            {
                dsmh = dsmh.OrderByDescending(n => n.Rating);
            }
            ViewBag.SapXep = sort;
            ViewBag.ID_DanhMuc = id_danhMuc;
            Session["keyword"] = keyword;

            return PartialView(dsmh.ToPagedList(page ?? 1, 9));
        }

        //Xem sản phẩm
        public ActionResult XemSanPham(int? page, int? id_matHang)
        {
            Session["GioHang"] = gioHang.matHang.Count == 0 ? null : gioHang;
            if (id_matHang == null)
            {
                return RedirectToAction("TrangChu");
            }
            ViewBag.Trang = page;
            var mh = db.tb_MatHang.SingleOrDefault(n => n.ID == id_matHang);

            bool add = true;
            foreach (var item in spVuaXems)
            {
                if (item.ID == id_matHang)
                {
                    add = false;
                    break;
                }
            }
            if (add)
            {
                spVuaXems.Add(db.tb_MatHang.SingleOrDefault(n => n.ID == id_matHang));
                if (spVuaXems.Count > 4)
                {
                    spVuaXems.RemoveAt(0);
                }
            }

            return View(mh);
        }

        //Giỏ hàng
        public ActionResult ThemGioHang(int id_matHang, int id_kichthuoc, int id_mausac, int id_gioitinh, int soluong)
        {
            gioHang.themListSP(id_matHang, id_kichthuoc, id_mausac, id_gioitinh, soluong);
            return RedirectToAction("LoadSoLuongYeuThichVaGioHangPartial");
        }

        public ActionResult GioHang()
        {
            return View();
        }

        public ActionResult DanhSachGioHangPartial ()
        {
            Session["GioHang"] = gioHang.matHang.Count == 0 ? null : gioHang;
            return PartialView();
        }
        public ActionResult ThayDoiSoLuong(int id_matHang, int id_kichthuoc, int id_mausac, int id_gioitinh, int tang)
        {
            gioHang.thayDoiSoLuong(id_matHang, id_kichthuoc, id_mausac, id_gioitinh, tang);
            return RedirectToAction("DanhSachGioHangPartial");
        }

        public ActionResult XoaMatHangGioHangPartial(int id_matHang, int id_kichthuoc, int id_mausac, int id_gioitinh)
        {
            return PartialView(db.tb_SanPham.FirstOrDefault(n => n.ID_MatHang == id_matHang && n.ID_KichCo == id_kichthuoc && n.ID_MauSac == id_mausac && n.ID_GioiTinh == id_gioitinh));
        }

        public ActionResult XacNhanXoaMatHangGioHang(int id_matHang, int id_kichthuoc, int id_mausac, int id_gioitinh)
        {
            gioHang.xoaMatHang(id_matHang, id_kichthuoc, id_mausac, id_gioitinh);
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatCaGioHangPartial()
        {
            return PartialView();
        }

        public ActionResult XacNhanXoaTatCaGioHang()
        {
            gioHang.xoaGioHang();
            Session["GioHang"] = null;
            return RedirectToAction("GioHang");
        }

        //Yêu thích sản phẩm
        public ActionResult ThemYeuThich(int? id_matHang)
        {
            id_matHangYeuThichs.Add(id_matHang);
            return RedirectToAction("LoadSoLuongYeuThichVaGioHangPartial");
        }
        public ActionResult YeuThich()
        {
            Session["YeuThich"] = id_matHangYeuThichs.Count == 0 ? null : id_matHangYeuThichs;
            var dssp = new List<tb_MatHang>();
            foreach (var idsp in id_matHangYeuThichs)
            {
                dssp.Add(db.tb_MatHang.SingleOrDefault(n => n.ID == idsp));
            }
            return View(dssp);
        }

        public ActionResult DanhSachYeuThichPartial()
        {
            Session["YeuThich"] = id_matHangYeuThichs.Count == 0 ? null : id_matHangYeuThichs;
            var dssp = new List<tb_MatHang>();
            foreach (var idsp in id_matHangYeuThichs)
            {
                dssp.Add(db.tb_MatHang.SingleOrDefault(n => n.ID == idsp));
            }
            return PartialView(dssp);
        }

        public ActionResult XoaYeuThichPartial(int index)
        {
            ViewBag.Index = index;
            int id = id_matHangYeuThichs[index] ?? 1;
            var sp = db.tb_MatHang.FirstOrDefault(n => n.ID == id);
            ViewBag.MatHang = sp;
            return PartialView();
        }

        public ActionResult XacNhanXoaYeuThich(int index)
        {
            id_matHangYeuThichs.RemoveAt(index);
            return RedirectToAction("YeuThich");
        }

        public ActionResult XoaTatCaYeuThichPartial()
        {
            return PartialView();
        }

        public ActionResult XacNhanXoaTatCaYeuThich()
        {
            id_matHangYeuThichs.Clear();
            Session["YeuThich"] = null;
            return RedirectToAction("YeuThich");
        }

        public ActionResult LoadSoLuongYeuThichVaGioHangPartial()
        {
            int? soluong = 0;
            for (int i = 0; i < gioHang.matHang.Count; i++)
            {
                soluong += gioHang.matHang[i].sps.Count();
            }
            ViewBag.SoLuongYeuThich = id_matHangYeuThichs.Count();
            ViewBag.SoLuongGioHang = soluong;
            return PartialView();
        }
        public ActionResult LoadSoLuongSPPartial(int? id_matHang)
        {
            ViewBag.ID_MatHang = id_matHang;
            for (int i = 0; i < gioHang.matHang.Count; i++)
            {
                if (gioHang.matHang[i].id == id_matHang)
                {
                    ViewBag.SoLuong = gioHang.matHang[i].sps.Count();
                }
            }
            return PartialView();
        }
        public ActionResult LoadThanhTienMatHangPartial(int? id_matHang, int id_kichthuoc, int id_mausac, int id_gioitinh)
        {
            for (int i = 0; i < gioHang.matHang.Count; i++)
            {
                if (gioHang.matHang[i].id == id_matHang && gioHang.matHang[i].sps[0].ID_KichCo == id_kichthuoc && gioHang.matHang[i].sps[0].ID_MauSac == id_mausac && gioHang.matHang[i].sps[0].ID_GioiTinh == id_gioitinh)
                {
                    ViewBag.ThanhTien = ConvertMoney.convertMoney(Init.thanhTienMatHang(id_matHang, gioHang.matHang[i].sps.Count()));
                }
            }
            return PartialView();
        }
        public decimal? LayThanhTienToanSP()
        {
            decimal? sum = 0;
            foreach (var mh in gioHang.matHang)
            {
                var mathang = db.tb_MatHang.SingleOrDefault(n => n.ID == mh.id);
                sum += mh.sps.Count() * Init.layTienGiamGia(mathang.Gia, mathang.GiamGia);
            }
            return sum;
        }

        public ActionResult LoadTomTatGioHangPartial()
        {
            decimal? sum = LayThanhTienToanSP();
            ViewBag.ThanhTienToanSP = sum;
            var bangGiaShip = db.tb_BangGiaShip.ToList();
            foreach (var item in bangGiaShip)
            {
                if (sum >= item.MinThanhTien && sum <= item.MaxThanhTien)
                {
                    ViewBag.TienShip = item.Phi;
                    ViewBag.TongCong = sum + item.Phi;
                    return PartialView();
                }
            }
            ViewBag.TienShip = 15;
            ViewBag.TongCong = sum + 15;
            return PartialView();
        }

        public ActionResult LoadThongTinDanhGiaSPPartial(int? page, int? id_matHang)
        {
            var tenmh = db.tb_MatHang.SingleOrDefault(n => n.ID == id_matHang);
            var dsDG = db.tb_DanhGiaTraiNghiem.Where(n => n.ID_MatHang == id_matHang).OrderByDescending(n => n.NgayDanhGia);
            int pageSize = 3;
            int pageNum = (page ?? 1);
            if (dsDG.Count() == 0)
            {
                Session["HienThiTrang"] = null;
                return PartialView();
            }
            Session["HienThiTrang"] = true;
            ViewBag.ID_MatHang = id_matHang;
            ViewBag.TongSoDG = Init.getSoLuotDanhGiaSanPham(id_matHang);
            ViewBag.TenMatHang = tenmh.TenMatHang;
            return PartialView(dsDG.ToPagedList(pageNum, pageSize));
        }

        public ActionResult DangBaiDanhGia(int id_matHang, double? rating, string noidung, int? page)
        {
            if (Session["Taikhoan"] == null)
            {
                Session["ErrorYeuCauDangNhap"] = "Bạn cần đăng nhập để đăng bài đánh giá";
                return RedirectToAction("XemSanPham", new { @page = page, @id_matHang = id_matHang });
            }
            var taikhoan = (tb_NguoiDung)Session["Taikhoan"];
            tb_DanhGiaTraiNghiem dg = new tb_DanhGiaTraiNghiem();
            dg.ID_MatHang = id_matHang;
            dg.ID_NguoiDung = taikhoan.ID;
            dg.NgayDanhGia = DateTime.Now;
            dg.NoiDung = noidung;
            dg.Rating = rating;
            db.tb_DanhGiaTraiNghiem.Add(dg);
            db.SaveChanges();
            return RedirectToAction("LoadThongTinDanhGiaSPPartial", new { @page = page, @id_matHang = id_matHang });
        }

        public ActionResult YeuCauDangNhap(int id_sp)
        {
            ViewBag.ID_SP = id_sp;
           
            return PartialView();
        }

        public ActionResult XacNhanDangNhap(int? id_sp)
        {

            return RedirectToAction("DangNhap", new { idsp = id_sp });
        }

        public ActionResult LoadSPVuaXemPartial(int? id_matHang, string nameAction)
        {
            Session["SPVuaXem"] = spVuaXems;
            ViewBag.ID_MatHang = id_matHang;
            ViewBag.NameAction = nameAction;
            return PartialView();
        }

        public ActionResult LoadGoiYSanPhamPartial(int? id_matHang, string nameAction)
        {
            var sp = db.tb_MatHang.SingleOrDefault(n => n.ID == id_matHang);
            var sptuongtu = db.tb_MatHang.Where(n => n.ID_DanhMuc == sp.ID_DanhMuc && n.ID != sp.ID);
            ViewBag.NameAction = nameAction;
            ViewBag.ID_MatHang = id_matHang;
            return PartialView(sptuongtu);
        }
        public ActionResult LoadSanPhamMoiNhatPartial(string nameAction)
        {
            var sp = db.tb_MatHang.OrderByDescending(n => n.NgayThem).Take(8);
            ViewBag.NameAction = nameAction;
            return PartialView(sp);
        }



        class thongtinTT
        {
            public string hoten { get; set; }
            public string sdt { get; set; }
            public string diachi { get; set; }
            public bool nharieng { get; set; }
            public bool tructiep { get; set; }
            public string thanhtien { get; set; }
            public string tienship { get; set; }
            public string tongcong { get; set; }

          

        }

        void TaoDonHang(thongtinTT tt)
        {
            tb_DonMuaHang dmh = new tb_DonMuaHang();
            dmh.HinhThucTT = tt.tructiep ? Init.thanhToanTT : Init.thanhToanVNPay;
            dmh.HoTen = tt.hoten;
            dmh.SDT = tt.sdt;

            dmh.NgayDat = DateTime.Now;
            dmh.TienShip = decimal.Parse(tt.tienship);
            dmh.TongTien = decimal.Parse(tt.thanhtien);
            dmh.TongCong = decimal.Parse(tt.tongcong);
            dmh.TrangThai = Init.doiDuyet;
            dmh.DiaChiGiaoHang = tt.diachi;
            dmh.NhaRieng = tt.nharieng;

            var tk = (tb_NguoiDung)Session["Taikhoan"];
            if (tk != null)
            {
                dmh.ID_NguoiDung = tk.ID;
            }

            db.tb_DonMuaHang.Add(dmh);
            db.SaveChanges();

            foreach (var mh in gioHang.matHang)
            {
                var mathang = db.tb_MatHang.SingleOrDefault(n => n.ID == mh.id);
                tb_MoTaDonHang mota = new tb_MoTaDonHang();
                mota.Gia = mathang.Gia;
                mota.GiamGia = mathang.GiamGia;
                mota.ID_DonMuaHang = dmh.ID;
                mota.ID_MatHang = (int?)mh.id;
                mota.ID_KichCo = mh.sps[0].tb_SizeMatHang.ID_KichThuoc;
                mota.ID_MauSac = mh.sps[0].tb_MauMatHang.ID_MauSac;
                mota.ID_GioiTinh = mh.sps[0].tb_GioiTinhMatHang.ID_GioiTinh;
                int soluongsp = mh.sps.Count();
                mota.SoLuong = soluongsp;
                mota.ThanhTien = Init.thanhTienMatHang(mh.id, soluongsp);
                db.tb_MoTaDonHang.Add(mota);
                db.SaveChanges();

                foreach (var sp in mh.sps)
                {
                    tb_ChiTietDonMuaHang ctdmh = new tb_ChiTietDonMuaHang();
                    ctdmh.ID_MoTaDonHang = mota.ID;
                    ctdmh.ID_SanPham = sp.ID;
                    db.tb_ChiTietDonMuaHang.Add(ctdmh);
                    var spt = db.tb_SanPham.SingleOrDefault(n => n.ID == sp.ID);
                    spt.TrangThai = true;
                    db.Entry(spt).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
            }
            gioHang.xoaGioHang();
            Session["GioHang"] = null;
            if (tk == null)
            {
                donhang_ad.Add(dmh);
            }
            Session["SuccessThanhToan"] = "Đã gửi đơn hàng thành công";
            Response.Redirect("~/GioHang");
        }

        static thongtinTT tt = null;

        public void ThanhToan(string hoten, string sdt, string diachi, bool nharieng, bool tructiep, string thanhtien, string tienship, string tongcong)
        {

            var hinhthucTT = tructiep ? Init.thanhToanTT : Init.thanhToanVNPay;
            tt = new thongtinTT
            {
                hoten = hoten,
                sdt = sdt,
                diachi = diachi,
                nharieng = nharieng,
                tructiep = tructiep,
                thanhtien = thanhtien,
                tienship = tienship,
                tongcong = tongcong,
            };
            if (hinhthucTT == Init.thanhToanVNPay)
            {

                ThanhToanVNPAY(decimal.Parse(tongcong) * 1000);
            }
            else
            {
                TaoDonHang(tt);
                tt = null;
            }

        }

        public ActionResult LoadThongSoSanPhamPartial(int? id_matHang, int? id_kichthuoc, int? id_mausac, int? id_gioitinh)
        {
            var mh = db.tb_MatHang.SingleOrDefault(n => n.ID == id_matHang);
            var ktDau = db.tb_SizeMatHang.FirstOrDefault();
            var mauSacDau = db.tb_MauMatHang.FirstOrDefault();
            var gtDau = db.tb_GioiTinhMatHang.FirstOrDefault();
            if (ktDau == null || mauSacDau == null || gtDau == null)
            {
                return PartialView();
            }
            if (id_kichthuoc == null)
            {
                id_kichthuoc = ktDau.ID;
            }
            if (id_mausac == null)
            {
                id_mausac = mauSacDau.ID;
            }
            if (id_gioitinh == null)
            {
                id_gioitinh = gtDau.ID;
            }
            var sp = mh.tb_SanPham.Where(n => n.ID_KichCo == id_kichthuoc && n.ID_MauSac == id_mausac && n.ID_GioiTinh == id_gioitinh && n.TrangThai == false);
            ViewBag.MH = mh;
            ViewBag.ID_KichThuoc = id_kichthuoc;
            ViewBag.ID_MauSac = id_mausac;
            ViewBag.ID_GioiTinh = id_gioitinh;
            return PartialView(sp);
        }

        public ActionResult DanhSachDonHangPartial()
        {
            tb_NguoiDung nd = (tb_NguoiDung)Session["Taikhoan"];
            var dsdh = db.tb_DonMuaHang.Where(n => n.ID_NguoiDung == nd.ID);
            return PartialView(dsdh);
        }
        public ActionResult DanhSachDonHangAnDanhPartial()
        {
            Session["DonHangAnDanh"] = donhang_ad;
            return PartialView();
        }

        public ActionResult XoaDonHangPartial(int? id)
        {
            return PartialView(db.tb_DonMuaHang.SingleOrDefault(n => n.ID == id));
        }
        public ActionResult XacNhanHuyDonHang(int? id)
        {
            var dh = db.tb_DonMuaHang.SingleOrDefault(n => n.ID == id);

            var dsmt = db.tb_MoTaDonHang.Where(n => n.ID_DonMuaHang == id);

            foreach (var mt in dsmt)
            {
                var dsct = db.tb_ChiTietDonMuaHang.Where(n => n.ID_MoTaDonHang == mt.ID);
                foreach (var ct in dsct)
                {
                    var sp = db.tb_SanPham.SingleOrDefault(n => n.ID == ct.ID_SanPham);
                    sp.TrangThai = false;
                    db.tb_ChiTietDonMuaHang.Remove(ct);
                    db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
                }
                db.tb_MoTaDonHang.Remove(mt);
            }

            db.tb_DonMuaHang.Remove(dh);
            db.SaveChanges();
            Session["SucessHuyDonHang"] = "Đã huỷ đơn hàng thành công";


            foreach (var item in donhang_ad)
            {
                if (item.ID == id) {
                    donhang_ad.Remove(item);
                    break;
                }
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XemDonHangPartial(int? id)
        {
            return PartialView(db.tb_DonMuaHang.SingleOrDefault(n => n.ID == id));
        }

        public ActionResult BoLocSanPham(int? id_danhmuc, string keyword, int? id_price, int? id_gender, int? id_size, int? id_color)
        {
            return RedirectToAction("LoadDanhSachSanPhamPartial", new { @id_danhMuc = id_danhmuc, @keyword = keyword, @sort = 1, @id_price = id_price, @id_gender = id_gender, @id_size = id_size, @id_color = id_color });
        }

        public ActionResult XacNhanDonHang()
        {
            log.InfoFormat("Begin VNPAY Return, URL={0}", Request.RawUrl);
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        //Thanh toan thanh cong
                        ViewBag.MessageSuccess = "Giao dịch được thực hiện thành công.";
                        log.InfoFormat("Thanh toán thành công, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);

                        TaoDonHang(tt);
                        tt = null;



                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.MessageError = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        log.InfoFormat("Thanh toán thất bại, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    ViewBag.MaWeb = TerminalID;
                    ViewBag.MaGD = orderId;
                    ViewBag.MaGDVNPAY = vnpayTranId;
                    ViewBag.SoTien = (decimal)vnp_Amount;
                    ViewBag.NganHang = bankCode;
                }
                else
                {
                    log.InfoFormat("Invalid signature, InputData={0}", Request.RawUrl);
                    ViewBag.MessageError = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }
            return View();
        }
        private static readonly ILog log =
          LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void ThanhToanVNPAY(decimal? tongTien)
        {
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Get payment input
            OrderInfo order = new OrderInfo();
            order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            order.Amount = (long)tongTien; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" khởi tạo giao dịch chưa có IPN
            order.CreatedDate = DateTime.Now;
            //Save order to db

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000

            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());

            vnpay.AddRequestData("vnp_Locale", "vn");

            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng mã " + order.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            Response.Redirect(paymentUrl);
        }



        public ActionResult ChinhSach()
        {
            return View();
        }
        

        public ActionResult LienHe()
        {
            return View();
        }

    }
}