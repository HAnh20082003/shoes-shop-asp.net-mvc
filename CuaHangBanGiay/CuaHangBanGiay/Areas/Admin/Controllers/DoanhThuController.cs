using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangBanGiay.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace CuaHangBanGiay.Areas.Admin.Controllers
{
    public class DoanhThuController : Controller
    {
        private ShoesShopEntities db = new ShoesShopEntities();
        // GET: Admin/DoanhThu
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BangThongKe(string date1, string date2)
        {
            if (date1 != null && date2 != null)
            {
                DateTime d1 = DateTime.Parse(date1);
                DateTime d2 = DateTime.Parse(date2);
                var dt = db.tb_DonMuaHang.Where(s => s.NgayDuyet <= d2 && s.NgayDuyet >= d1 && s.TrangThai == 2).GroupBy(s => s.NgayDuyet).Select(s => new { Ngay = s.Select(d => d.NgayDuyet).FirstOrDefault(), Tong = s.Sum(d => d.TongCong) }).ToList();
                List<doanhthuNgay> doanhthu = new List<doanhthuNgay>();
                foreach (var item in dt)
                {
                    doanhthuNgay tmp = new doanhthuNgay();
                    tmp.Ngay = DateTime.Parse(item.Ngay.ToString()).ToString("dd/MM/yyyy");
                    tmp.TongTien = (int)item.Tong;
                    doanhthu.Add(tmp);
                }

                ViewBag.dt = doanhthu;
                var topdt = doanhthu.OrderByDescending(s => s.TongTien).Take(10).ToList();
                ViewBag.topdt = topdt;
                ViewBag.count = dt.Count();
                Session["dt"] = doanhthu;
            }
            else
            {
                DateTime d1 = DateTime.Now;
                DateTime d2 = DateTime.Now.AddDays(-30);
                var dt = db.tb_DonMuaHang.Where(s => s.NgayDuyet <= d1 && s.NgayDuyet >= d2 && s.TrangThai==2).GroupBy(s => s.NgayDuyet).Select(s => new { Ngay = s.Select(d => d.NgayDuyet).FirstOrDefault(), Tong = s.Sum(d => d.TongCong) }).ToList();
                List<doanhthuNgay> doanhthu = new List<doanhthuNgay>();
                foreach (var item in dt)
                {
                    doanhthuNgay tmp = new doanhthuNgay();
                    tmp.Ngay = DateTime.Parse(item.Ngay.ToString()).ToString("dd/MM/yyyy");
                    tmp.TongTien = (int)item.Tong;
                    doanhthu.Add(tmp);
                }

                ViewBag.dt = doanhthu;
                var topdt = doanhthu.OrderByDescending(s => s.TongTien).Take(10).ToList();
                ViewBag.topdt = topdt;
                ViewBag.count = dt.Count();
                Session["dt"] = doanhthu;
            }
            return PartialView();
        }


        public void InFileExcel()
        {
            var doanhthu = Session["dt"] as List<doanhthuNgay>;


            ExcelPackage pck = new ExcelPackage();

            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Danh sách");

            string d1 = doanhthu.OrderBy(s => s.Ngay).Select(s => s.Ngay).Take(1).FirstOrDefault();
            string d2 = doanhthu.OrderByDescending(s => s.Ngay).Select(s => s.Ngay).Take(1).FirstOrDefault();

            var tongTien = doanhthu.Sum(s => s.TongTien);

            ws.Cells["D1"].Value = "Thống kê doanh thu từ " + d1 + " đến " + d2;
            ws.Cells["D2"].Value = "Ngày lập danh sách";
            ws.Cells["E2"].Value = string.Format("{0:dd MMMM yyyy} at {0:H : mm tt}", DateTimeOffset.Now);
            ws.Cells["D3"].Value = "Tổng tiền: ";
            ws.Cells["E3"].Value = tongTien;
            ws.Cells["A5"].Value = "STT";
            ws.Cells["B5"].Value = "Ngày";
            ws.Cells["C5"].Value = "Tổng";



            ws.Row(4).Height = 20;
            ws.Row(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Row(4).Style.Font.Bold = true;

            int rowStart = 6;
            int i = 1;
            foreach (var item in doanhthu)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = i;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.Ngay;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.TongTien;
                i++;
                rowStart++;

            }
            ws.Cells["A:AZ"].AutoFitColumns();
            string excelName = "Thống kê doanh thu từ " + d1 + " đến " + d2;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                pck.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
}