using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp_Stylo.Models;

namespace WebApp_Stylo.Controllers
{
    public class HomeController : Controller
    {
        private fashion_shopEntities db = new fashion_shopEntities();

        public ActionResult Index()
        {
            return View();
        }
        // ===== DOANH THU =====
        public JsonResult RevenueByMonth()
        {
            var data = db.DonHangs
                .GroupBy(x => new { x.NgayDat.Year, x.NgayDat.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Label = g.Key.Month + "/" + g.Key.Year,
                    Total = g.Sum(x => x.TongThanhToan)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RevenueByWeek()
        {
            var data = db.DonHangs
                .GroupBy(x =>
                    System.Data.Entity.SqlServer.SqlFunctions.DatePart("week", x.NgayDat)
                )
                .Select(g => new
                {
                    Week = g.Key,
                    Total = g.Sum(x => x.TongThanhToan)
                })
                .OrderBy(x => x.Week)
                .ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        // ===== TOP SẢN PHẨM =====
        public JsonResult TopSellingProducts()
        {
            var data = db.DonHang_ChiTiet
                .GroupBy(ct => ct.SanPham_BienThe.SanPham.TenSanPham)
                .Select(g => new
                {
                    Product = g.Key,
                    Quantity = g.Sum(x => x.SoLuong)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(5)
                .ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        // ===== KHÁCH HÀNG =====
        public JsonResult CustomerStats()
        {
            var total = db.KhachHangs.Count();

            var thisMonth = db.KhachHangs
                .Count(x => x.created_at.Month == DateTime.Now.Month
                         && x.created_at.Year == DateTime.Now.Year);

            return Json(new
            {
                Total = total,
                ThisMonth = thisMonth
            }, JsonRequestBehavior.AllowGet);
        }

        // ===== TỔNG DOANH THU =====
        [AllowAnonymous]
        public JsonResult TotalRevenue()
        {
            var total = db.DonHangs.Sum(x => (decimal?)x.TongThanhToan) ?? 0;
            return Json(total, JsonRequestBehavior.AllowGet);
        }

    }
}