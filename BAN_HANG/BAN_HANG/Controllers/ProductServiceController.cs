using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using CS.Common.Const;
using CS.Common.Untils;
using CS.Data.Business;
using CS.Data.DataContext;
using Microsoft.Owin.Security;
using WebApplication;
using WebApplication.Utility;

namespace BAN_HANG.Controllers
{
    public class ProductServiceController : BaseController
    {
        DB_CSEntities1 entity = new DB_CSEntities1();
        public ActionResult Index()
        {
            return PartialView();
        }
        public ActionResult ListServices()
        {
            var db = new ProductServiceBusiness();
            var result = db.GetListProductsByType(SystemMessageConst.Key.Product_Service);
            return PartialView(result);
        }
        public ActionResult ListProductRetail()
        {
            var db = new ProductServiceBusiness();
            var result = db.GetListProductsByType(SystemMessageConst.Key.Product_Sale);
            return PartialView(result);
        }
        public ActionResult ListServicesItem(int id)
        {
            ViewBag.id = id;
            var db = new ProductServiceBusiness();
            var result = db.GetListProductsDetail(id);
            return PartialView(result);
        }

        public ActionResult _XoathongsoTheoSP(int id_config_attr = 0)
        {
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Delete);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new ProductServiceBusiness();
            var result = db.BS_XoathongsoTheoSP(id_config_attr);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowDetailProduct(int id)
        {
            ViewBag.id = id;
            var db = new ProductServiceBusiness();

            var lst_TSKT = db.Hienthithongtinchitiet_sp_thongsokt(id);
            ViewBag.lst_TSKT = lst_TSKT;

            var lst_Price = db.Hienthithongtinchitiet_sp_ds_gia(id);
            ViewBag.lst_Price = lst_Price;

            var lst_IMG = db.Hienthithongtinchitiet_sp_ds_anh(id);
            ViewBag.lst_IMG = lst_IMG;

            return PartialView();
        }

        public ActionResult ThemMoi_Thongso_kythuat(int id)
        {
            ViewBag.id = id;
            return PartialView();
        }

     
        public ActionResult _Themmoi_thongso(int id_attr, string value_attr, int id_product)
        {
            var db = new ProductServiceBusiness();

            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Addnew);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var result = db.BS_Themmoi_thongso(id_attr, value_attr, id_product);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChinhSua_Thongso_kythuat(int id_p = 0, int id_cf_attr = 0,int id_attr=0)
        {
            ViewBag.id_p = id_p;
            ViewBag.id_attr = id_attr;
            ViewBag.id_cf_attr = id_cf_attr;

            var data = entity.sys_attr_product.FirstOrDefault(m => m.id == id_cf_attr);
            string val_attr = "";
            if (data != null)
            {
                val_attr = data.descripton_attr;
            }

            ViewBag.val_attr = val_attr;
            return PartialView();
        }
        public ActionResult _ChinhSua_Thongso_kythuat(int id_attr,int id_cf_attr, string value_attr, int id_product)
        {
            var db = new ProductServiceBusiness();
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Update);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }
            var result = db.BS_ChinhSua_Thongso_kythuat(id_attr, id_cf_attr, value_attr, id_product);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }



        // price

        public ActionResult ThemMoi_Gia_SP(int id)
        {
            ViewBag.id = id;
            return PartialView();
        }
        public ActionResult _ThemMoi_Gia_SP(int gianhap, int giaxuat, string type_price,int id_product)
        {
            var db = new ProductServiceBusiness();
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Addnew);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }
            var result = db.BS_ThemMoi_Gia_SP(gianhap, giaxuat, type_price, id_product);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _XoaGiaTheoSP(int id_price = 0,int id_product=0)
        {
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Delete);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new ProductServiceBusiness();
            var result = db.BS_XoaGiaTheoSP(id_price, id_product);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChinhSua_Gia_SP(int id_price = 0, int id_product = 0)
        {
            ViewBag.id_price = id_price;
            ViewBag.id_product = id_product;

            var data = entity.tbl_price.FirstOrDefault(m => m.id == id_price && m.id_product == id_product);
            double gianhap = 0;
            double giaxuat = 0;
            string value = "0";

            if (data != null)
            {
                gianhap = data.gia_nhap.Value;
                giaxuat = data.gia_xuat.Value;
                value = data.type_price;
            }

            ViewBag.gianhap = gianhap;
            ViewBag.giaxuat = giaxuat;
            ViewBag.value = value;
            return PartialView();
        }

        public ActionResult _ChinhSua_Gia_SP(int gianhap, int giaxuat, string type_price, int id_product,int id_price)
        {
            var db = new ProductServiceBusiness();
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Update);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }
            var result = db.BS_ChinhSua_Gia_SP(gianhap, giaxuat, type_price, id_product, id_price);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        // Anh
        public ActionResult ThemMoi_Anh_SP(int id)
        {
            ViewBag.id = id;
            return PartialView();
        }
     

        [HttpPost]
        public ActionResult _SaveUploadedFile(int id_product = 0)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
               // var file = Request.Files;
               // var fileImages = Request.Files[0];

                foreach (string fileImages in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileImages];

                    string url = SaveMultiFile.SaveFile(file, SystemMessageConst.Key.Url_Save_Img_Product,
                        SystemMessageConst.Key.exImg);


                    if (!string.IsNullOrEmpty(url))
                    {
                        img_product img = new img_product();
                        img.id_product = id_product;
                        img.path_img = url;
                        img.isactive = true;

                        entity.img_product.Add(img);
                        entity.SaveChanges();

                        systemMessage.IsSuccess = true;
                        systemMessage.Message = "Upload thành công !!";
                    }
                    else
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = "Error";
                    }
                }
                return Json(new { result = systemMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = "Error";
                return Json(new { result = systemMessage }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult _XoaAnhTheoSP(int id_img = 0, int id_product = 0)
        {
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Delete);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new ProductServiceBusiness();
            var result = db.BS_XoaAnhTheoSP(id_img, id_product);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _ChinhSua_Anh_TheoSP(int id_img, int id_product, string txt_des)
        {
            var db = new ProductServiceBusiness();
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Update);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }
            var result = db.BS_ChinhSua_Anh_TheoSP(id_img, id_product, txt_des);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ThemmoiSP()
        {
            return PartialView();
        }
        public ActionResult _ThemmoiSP(string tensp,string codesp,string loaisp,int unitsp,string gioithieusp, bool status = false)
        {
            var db = new ProductServiceBusiness();
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Addnew);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }
            var result = db.BS_ThemmoiSP(tensp, codesp, loaisp, unitsp, gioithieusp, status);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Chinhsua_TT_SP(int id)
        {
            ViewBag.id = id;

            var data = entity.sys_product.FirstOrDefault(m => m.id == id);

          
            return PartialView(data);
        }
        public ActionResult _Chinhsua_TT_SP(int id,string tensp, string codesp, string loaisp, int unitsp, string gioithieusp, bool status = false)
        {
            var db = new ProductServiceBusiness();
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Update);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }
            var result = db.BS_Chinhsua_TT_SP(id, tensp, codesp, loaisp, unitsp, gioithieusp, status);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
    }
}