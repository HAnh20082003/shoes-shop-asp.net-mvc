using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuaHangBanGiay.Models
{
    public class Init
    {
        public static int maxRating = 5;
        public static ShoesShopEntities db = new ShoesShopEntities();

        public static string thanhToanVNPay = "VNPay";
        public static string thanhToanTT = "Thanh toán khi nhận hàng";

        public static int huyDon = -1, doiDuyet = 0, daDuyet = 1, daNhan = 2;
        
        public static string folderImageProduct = "Content/imgs/sanpham/";
        public static string folderImageCategory = "Content/imgs/danhmuc/";
        public static string folderAvatarUser = "Content/imgs/Users/";
        public static string folderBanner = "Content/imgs/Banners/";
        public static string tailFileProduct = ".jpg";

        public static int? getSoLuongMucGia(decimal? minGia, decimal? maxGia, int? id_danhMuc, string keyWord)
        {
            List<tb_MatHang> mh;
            if (id_danhMuc != null)
            {
                mh = db.tb_MatHang.Where(n => n.ID_DanhMuc == id_danhMuc).ToList();
            }
            else
            {
                mh = db.tb_MatHang.ToList();
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                mh = mh.Where(n => n.TenMatHang.Contains(keyWord)).ToList();
            }
            //mh = mh.Where(n => n.Gia >= minGia && n.Gia <= maxGia).ToList();
            //int? soLuong = 0;
            //foreach (var mat_hang in mh)
            //{
            //    soLuong += mat_hang.SoLuongSanPham;
            //}
            //return soLuong;
            return mh.Where(n => n.Gia >= minGia && n.Gia <= maxGia).Sum(s => s.tb_SanPham.Count());

        }

        public static int? getSoLuongMau(int id_mauSac, int? id_danhMuc, string keyWord)
        {
            List<tb_MatHang> mh;
            if (id_danhMuc != null)
            {
                mh = db.tb_MatHang.Where(n => n.ID_DanhMuc == id_danhMuc).ToList();
            }
            else
            {
                mh = db.tb_MatHang.ToList();
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                mh = mh.Where(n => n.TenMatHang.Contains(keyWord)).ToList();
            }

            //int soLuong = 0;
            //foreach (var m in mh)
            //{
            //    soLuong += m.tb_SanPham.Where(n => n.ID_MauSac == id_mauSac).Count();
            //}
            //return soLuong;
            return mh.Sum(s => s.tb_SanPham.Where(t => t.tb_MauMatHang.ID_MauSac == id_mauSac).Count());
        }

        public static int? getSoLuongKichThuoc(int id_kichThuoc, int? id_danhMuc, string keyWord)
        {
            List<tb_MatHang> mh;
            if (id_danhMuc != null)
            {
                mh = db.tb_MatHang.Where(n => n.ID_DanhMuc == id_danhMuc).ToList();
            }
            else
            {
                mh = db.tb_MatHang.ToList();
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                mh = mh.Where(n => n.TenMatHang.Contains(keyWord)).ToList();
            }
            //int? soLuong = 0;
            //foreach (var m in mh)
            //{
            //    soLuong += m.tb_SanPham.Count(n => n.ID_KichCo == id_kichThuoc);
            //}
            //return soLuong;
            return mh.Sum(s => s.tb_SanPham.Where(t => t.tb_SizeMatHang.ID_KichThuoc == id_kichThuoc).Count());
        }
        
        public static int getsoluongmh(int id)
        {
            var danhmuc = db.tb_DanhMuc.Find(id);
            return danhmuc.tb_MatHang.Sum(s => s.tb_SanPham.Count());
        }


        public static int? getSoLuongGioiTinh(int id_GioiTinh, int? id_danhMuc, string keyWord)
        {
            List<tb_MatHang> mh;
            if (id_danhMuc != null)
            {
                mh = db.tb_MatHang.Where(n => n.ID_DanhMuc == id_danhMuc).ToList();
            }
            else
            {
                mh = db.tb_MatHang.ToList();
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                mh = mh.Where(n => n.TenMatHang.Contains(keyWord)).ToList();
            }

            return mh.Sum(s => s.tb_SanPham.Where(t=>t.tb_GioiTinhMatHang.ID_GioiTinh == id_GioiTinh).Count());
            //return mh.Count();

        }
        //public static int? getSoLuongGioiTinh(int id_GioiTinh, int id_danhMuc)
        //{
        //    var dsGioiTinh = db.tb_GioiTinhMatHang.Where(n => n.ID_GioiTinh == id_GioiTinh).ToList();
        //    int? soLuong = 0;
        //    foreach (var gioitinh in dsGioiTinh)
        //    {
        //        var mat_hangs = db.tb_MatHang.Where(n => n.ID == gioitinh.ID_MatHang && n.ID_DanhMuc == id_danhMuc).ToList();
        //        foreach (var mat_hang in mat_hangs)
        //        {
        //            soLuong += mat_hang.SoLuongSanPham;
        //        }
        //    }
        //    return soLuong;
        //}
        //public static int? getSoLuongGioiTinh(int id_GioiTinh, string Keyword)
        //{
        //    var dsGioiTinh = db.tb_GioiTinhMatHang.Where(n => n.ID_GioiTinh == id_GioiTinh).ToList();
        //    int? soLuong = 0;
        //    foreach (var gioitinh in dsGioiTinh)
        //    {
        //        var mat_hangs = db.tb_MatHang.Where(n => n.ID == gioitinh.ID_MatHang && n.TenMatHang.Contains(Keyword)).ToList();
        //        foreach (var mat_hang in mat_hangs)
        //        {
        //            soLuong += mat_hang.SoLuongSanPham;
        //        }
        //    }
        //    return soLuong;
        //}

        public static int? getSoLuongMatHang(int? id_danhMuc, string keyword = null)
        {
            //List<tb_MatHang> mat_hangs;
            if (id_danhMuc != null)
            {
                return db.tb_MatHang.Where(s=>s.ID_DanhMuc == id_danhMuc).Sum(s => s.tb_SanPham.Count());
                //mat_hangs = db.tb_MatHang.Where(n => n.ID_DanhMuc == id_danhMuc).ToList();
            }
            else if (!string.IsNullOrEmpty(keyword))
            {
                return db.tb_MatHang.Where(s => s.TenMatHang.Contains(keyword)).Sum(s => s.tb_SanPham.Count());
                //mat_hangs = db.tb_MatHang.Where(n => n.TenMatHang.Contains(keyword)).ToList();
            }
            //else
            //{
            //    mat_hangs = db.tb_MatHang.ToList();
            //}
            //int? soLuong = 0;
            //foreach (var mat_hang in mat_hangs)
            //{
            //    soLuong += mat_hang.SoLuongSanPham;
            //}
            //return soLuong;
            return db.tb_MatHang.Sum(s => s.tb_SanPham.Count());
        }

        public static int? getSoLuotDanhGiaSanPham(int? id_matHang)
        {
            return db.tb_DanhGiaTraiNghiem.Where(n => n.ID_MatHang == id_matHang).Count();
        }

        public static decimal? layTienGiamGia(decimal? gia, int? phanTram)
        {
            decimal? giamGia = gia - gia * phanTram / 100;
            return giamGia;
        }
        public static string getGiamGiaSanPham(decimal? gia, int? phanTram)
        {
            return ConvertMoney.convertMoney(layTienGiamGia(gia, phanTram));
        }
        public static decimal? thanhTienMatHang(int? id_matHang, int? soLuong)
        {
            var matHang = db.tb_MatHang.SingleOrDefault(n => n.ID == id_matHang);
            decimal? gia = layTienGiamGia(matHang.Gia, matHang.GiamGia);
            decimal? newGia = gia * soLuong;
            return newGia;
        }

        public static (int, int) getSoLuongHTvaMaxSP(int id_mathang, IEnumerable<tb_SanPham> sanPham, int id_kichthuoc, int id_mausac, int id_gioitinh, Giohang gioHang)
        {
            var maxslsp = sanPham.Count();
            int count = 0;
            for (int i = 0; i < gioHang.matHang.Count; i++)
            {
                if (gioHang.matHang[i].id == id_mathang)
                {
                    foreach (var sp in gioHang.matHang[i].sps)
                    {
                        var ttsp = db.tb_SanPham.SingleOrDefault(n => n.ID == sp.ID && n.ID_KichCo == id_kichthuoc && n.ID_MauSac == id_mausac && n.ID_GioiTinh == id_gioitinh);
                        if (ttsp != null)
                        {
                            count++;
                        }
                    }
                }
            }

            int sl = 1;

            maxslsp -= count;
            if (sl > maxslsp)
            {
                sl = maxslsp;
            }
            return (sl, maxslsp);
        }

        public static string getTrangThai(int? trangthai)
        {
            if (trangthai == doiDuyet)
            {
                return "Đợi duyệt";
            }
            if (trangthai == daDuyet)
            {
                return "Đã duyệt";
            }
            if (trangthai == huyDon)
            {
                return "Huỷ đơn";
            }
            return "Đã nhận";
        }
    }
}