using CuaHangBanGiay.Models;

using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace CuaHangBanGiay.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        // GET: Admin/DonHang
        ShoesShopEntities db = new ShoesShopEntities();
        public ActionResult Index()
        {
            var dh = db.tb_DonMuaHang.ToList();
            return View(dh);
        }

        [HttpPost]
        public ActionResult XoaDonHang(int [] CheckDH)
        {
            if (CheckDH != null)
            {
                foreach (var item in CheckDH)
                {
                    var dh = db.tb_DonMuaHang.Find(item);
                    var mtdh = db.tb_MoTaDonHang.Where(n => n.ID_DonMuaHang == item);
                    foreach (var mt in mtdh)
                    {
                        var ctdh = mt.tb_ChiTietDonMuaHang;
                        db.tb_ChiTietDonMuaHang.RemoveRange(ctdh);
                        var tmpmt = db.tb_MoTaDonHang.SingleOrDefault(n => n.ID == mt.ID);
                        db.tb_MoTaDonHang.Remove(tmpmt);
                    }
                    db.tb_DonMuaHang.Remove(dh);
                }
                db.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }


        public ActionResult ChiTietDonHang(int id)
        {
            //var ctdh = db.tb_MoTaDonHang.Where(s => s.ID_DonMuaHang == id).ToList();
            var donHang = db.tb_DonMuaHang.Find(id);
            return PartialView(donHang);
        }


        public ActionResult XuLyDon(int id)
        {
            var donhang = db.tb_DonMuaHang.Find(id);
            return PartialView(donhang);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XuLyDon(FormCollection fields)
        {
            int ID = int.Parse(fields["ID"]);
            var donhang = db.tb_DonMuaHang.Where(s=> s.ID == ID).SingleOrDefault();
            if (donhang != null)
            {
                donhang.TrangThai = int.Parse(fields["TT"]);
                donhang.NgayDuyet = DateTime.Now;

                if (donhang.TrangThai == 1)
                {
                   
                    donhang.NgayDuKien = DateTime.Now.AddDays(4);
                    getForm(donhang);

                }
                db.Entry(donhang).State = System.Data.Entity.EntityState.Modified; 
            }  
            db.SaveChanges();


            
            return RedirectToAction("Index");
        }

   
        void sendMail(string body)
        {
            string to = "tronghieustopxzy@gmail.com";
            string subject = "Xác nhận đơn hàng";
            //string body = "ShoeShop";
            string email = "tronghieu151103@outlook.com.vn";
            string pass = "Hieu@1511";
            string host = "smtp.office365.com";
            int port = 587;
            //string body = System.IO.File.ReadAllText(Server.MapPath("~/Content/send1.html"));
            
            using (MailMessage mail = new MailMessage(email,to,subject,body))
            {
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(host, port))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(email, pass);
                    smtp.Send(mail);
                }
            }

        }



        void getForm(tb_DonMuaHang donhang)
        {
            string body = System.IO.File.ReadAllText(Server.MapPath("~/Content/formOrder.html"));
            body = body.Replace("{HoTen}", donhang.HoTen);
            body = body.Replace("{MaDonHang}", donhang.ID.ToString());
            body = body.Replace("{TenNguoiDat}", donhang.HoTen);
            body = body.Replace("{SDT}", donhang.SDT);
            body = body.Replace("{NgayDat}", donhang.NgayDat.ToString());
            body = body.Replace("{NgayDuKien}", donhang.NgayDuKien.ToString());
            body = body.Replace("{DiaChi}", donhang.DiaChiGiaoHang);
            body = body.Replace("{HinhThucTT}", donhang.HinhThucTT);
            body = body.Replace("{LoaiDiaChi}", donhang.NhaRieng == true ? "Nhà riêng" : "Văn phòng");
            body = body.Replace("{PhiVanChuyen}", ConvertMoney.convertMoney(donhang.TienShip) + ".000VNĐ");
            body = body.Replace("{TongCong}", ConvertMoney.convertMoney(donhang.TongCong) + ".000VNĐ");

            string dsSanPham = "";
            int i = 1;
            foreach (var item in donhang.tb_MoTaDonHang)
            {
                dsSanPham += sanpham(i, item.tb_MatHang.TenMatHang, item.tb_BangKichThuoc.KichThuoc+" "+ item.tb_BangMauSP.MauSac+" "+ item.tb_BangGioiTinh.GioiTinh, ConvertMoney.convertMoney(item.Gia - item.Gia * item.GiamGia / 100)+ ".000VNĐ", item.SoLuong.ToString(), ConvertMoney.convertMoney(item.ThanhTien) + ".000VNĐ");
                i++;
            }
            body = body.Replace("{SanPham}", dsSanPham);
            sendMail(body);
        }


        string sanpham(int stt,string tenSP,string thongso, string thanhtien,string sl, string tongcong) {
            string text = "<tr>"+
                    @"<td style=""padding-top:15px; padding-left:10px; word-break:break-all"">" + stt + "</td>" +
                    @"<td style=""padding-top:15px;padding-left:10px;word-break:break-all"" >" + tenSP + "</td>" +
                    @"<td style=""padding-top:15px;padding-left:10px;word-break:break-all"" >" + thongso + "</td>" +
                    @"<td style=""padding-top:15px;padding-left:10px;word-break:break-all"" >" + thanhtien + "</td>" +
                    @"<td style=""padding-top:15px;padding-left:10px;word-break:break-all"" >" + sl + "</td>" +
                    @"<td style=""padding-top:15px;padding-left:10px;word-break:break-all"" >" + tongcong + "</td>"+"</tr>";
                   
            return text;
        }

        




    }
}