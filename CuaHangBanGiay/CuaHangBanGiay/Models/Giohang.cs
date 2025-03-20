using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CuaHangBanGiay.Models;

namespace CuaHangBanGiay.Models
{
    public class Giohang
    {
        public static ShoesShopEntities db = new ShoesShopEntities();
        public List<IDMatHang> matHang = new List<IDMatHang>();
        public bool themListSP(int id_matHang, int id_kichthuoc, int id_mausac, int id_gioitinh, int soLuong)
        {
            var dssp = db.tb_SanPham.Where(n => n.ID_MatHang == id_matHang && n.TrangThai == false && n.ID_KichCo == id_kichthuoc && n.ID_MauSac == id_mausac && n.ID_GioiTinh == id_gioitinh).ToList();
            int count = 0;
            bool found = false;
            for (int i = 0; i < matHang.Count; i++)
            {
                if (matHang[i].id == id_matHang) //tìm ra các mặt hàng cùng id_matHang trong giỏ
                {
                    if (matHang[i].sps[0].ID_KichCo == id_kichthuoc && matHang[i].sps[0].ID_MauSac == id_mausac && matHang[i].sps[0].ID_GioiTinh == id_gioitinh)
                    {
                        found = true;
                        foreach (var sp in dssp)
                        {
                            if (matHang[i].sps.FirstOrDefault(n => n.ID == sp.ID) == null)
                            {
                                matHang[i].sps.Add(sp);
                                count++;
                                if (count == soLuong)
                                {
                                    return true;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            if (!found)
            {
                matHang.Add(new IDMatHang() { id = id_matHang, sps = new List<tb_SanPham>() });
                int lasti = matHang.Count - 1;
                foreach (var sp in dssp)
                {
                    if (matHang[lasti].sps.FirstOrDefault(n => n.ID == sp.ID) == null)
                    {
                        matHang[lasti].sps.Add(sp);
                        count++;
                        if (count == soLuong)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        public bool themListSP(int? id_matHang, int id_kichthuoc, int id_mausac, int id_gioitinh)
        {
            var dssp = db.tb_SanPham.Where(n => n.ID_MatHang == id_matHang && n.TrangThai == false && n.ID_KichCo == id_kichthuoc && n.ID_MauSac == id_mausac && n.ID_GioiTinh == id_gioitinh).ToList();
            for (int i = 0; i < matHang.Count; i++)
            {
                if (matHang[i].id == id_matHang) //tìm ra các mặt hàng cùng id_matHang trong giỏ
                {
                    if (matHang[i].sps[0].ID_KichCo == id_kichthuoc && matHang[i].sps[0].ID_MauSac == id_mausac && matHang[i].sps[0].ID_GioiTinh == id_gioitinh)
                    {
                        foreach (var sp in dssp)
                        {
                            if (matHang[i].sps.Where(n => n.ID == sp.ID) == null)
                            {
                                matHang[i].sps.Add(sp);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public void xoaGioHang()
        {
            matHang.Clear();
        }

        public void xoaMatHang(int id_matHang, int id_kichthuoc, int id_mausac, int id_gioitinh)
        {
            for (int i = 0; i < matHang.Count; i++)
            {
                if (matHang[i].id == id_matHang && matHang[i].sps[0].ID_KichCo == id_kichthuoc && matHang[i].sps[0].ID_MauSac == id_mausac && matHang[i].sps[0].ID_GioiTinh == id_gioitinh)
                {
                    matHang.RemoveAt(i);
                    return;
                }
            }
        }

        public void thayDoiSoLuong(int id_matHang, int id_kichthuoc, int id_mausac, int id_gioitinh, int tang = 1)
        {
            for (int i = 0; i < matHang.Count; i++)
            {
                if (matHang[i].id == id_matHang && matHang[i].sps[0].ID_KichCo == id_kichthuoc && matHang[i].sps[0].ID_MauSac == id_mausac && matHang[i].sps[0].ID_GioiTinh == id_gioitinh)
                {
                    var dssp = db.tb_SanPham.Where(n => n.TrangThai == false && n.ID_KichCo == id_kichthuoc && n.ID_MauSac == id_mausac && n.ID_GioiTinh == id_gioitinh);
                    if (tang == 1)
                    {
                        foreach (var sp in dssp)
                        {
                            if (matHang[i].sps.FirstOrDefault(n => n.ID == sp.ID) == null)
                            {
                                matHang[i].sps.Add(sp);
                                break;
                            }
                        }
                    }
                    else
                    {
                        matHang[i].sps.RemoveAt(0);
                        if (matHang[i].sps.Count == 0)
                        {
                            matHang.RemoveAt(i);
                        }
                    }
                    break;
                }
            }
        }
    }
}