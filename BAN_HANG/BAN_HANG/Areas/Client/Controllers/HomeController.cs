using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BAN_HANG.Areas.Client.Controllers
{
    public class HomeController : Controller
    {
        // GET: Client/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadListNewProduct()
        {
            var db = new HomeClientBusiness();
            var result = db.BS_LoadListNewProduct();
            return PartialView(result);
        }
        public ActionResult LoadLoaiSanPham()
        {
            var db = new HomeClientBusiness();
            var result = db.BS_LoadLoaiSanPham();
            return PartialView(result);
        }

        public ActionResult ChitietSP(int id)
        {
            ViewBag.id = id;
            var db = new HomeClientBusiness();
            var result = db.BS_ChitietSP(id);
            return PartialView(result);
        }

        public ActionResult LoadSPLienquan(int id)
        {
            ViewBag.id = id;
            var db = new HomeClientBusiness();
            var result = db.BS_LoadSPLienquan(id);
            return PartialView(result);
        }
        public ActionResult LoadDSLoaiSP()
        {
            var db = new HomeClientBusiness();
            var result = db.BS_LoadDSLoaiSP();
            return PartialView(result);
        }
        public ActionResult SanPhamMoi()
        {
            var db = new HomeClientBusiness();
            var result = db.BS_LoadListNewProduct();
            return PartialView(result);
        }
        public ActionResult Sanphamkhac(int id)
        {
            ViewBag.id = id;
            var db = new HomeClientBusiness();
            var result = db.BS_LoadSPLienquan(id);
            return PartialView(result);
        }

        //
        public ActionResult CategoryProduct(int id=0)
        {
            var db = new HomeClientBusiness();
            var result = db.LoadDSSP_Theoid_LoaiSP(id);
            return PartialView(result);
        }
    }
}