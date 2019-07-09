using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS.Common.Const;
using CS.Common.Untils;
using CS.Data.Business;
using CS.Data.DataContext;
using CS.Data.Model;
using CS.Data.Untils;
using Newtonsoft.Json;
using WebApplication.Utility;

namespace BAN_HANG.Controllers
{
    public class ManagerCategoryController : BaseController
    {
        // GET: ManagerCategory
        private DB_CSEntities1 entity = new DB_CSEntities1();
        SessionUser user = GetSessionBusiness.GetUser();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListCategory()
        {
            return PartialView();
        }
        public ActionResult _ListCategory()
        {
            var db = new ManagerCategoryBusiness();
            int minRow = 0;
            int maxRow = 0;
            int.TryParse(HttpContext.Request["start"], out minRow);
            int length = 10;
            int.TryParse(HttpContext.Request["length"], out length);
            maxRow = (minRow + length);
            int draw = 0;
            int.TryParse(HttpContext.Request["draw"], out draw);
            string search = HttpContext.Request["search[value]"].Trim();
            int currentPage = (minRow / 10) + 1;
            int roleid = user.Roleid;
            int idcenter = user.BranchId;
            var result = db.BS_ListCategory(search, currentPage, 10);

            return Content(JsonConvert.SerializeObject(new
            {
                data = result.Data,
                draw = draw,
                recordsFiltered = result.Total,
                recordsTotal = result.Total

            }));
        }

        public ActionResult DeleteCategory(int id)
        {
            // kiểm tra quyền thêm xóa 
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Delete);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new ManagerCategoryBusiness();
            var result = db.DeleteCategoryById(id);

            // luu log - xóa
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Delete.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { data = new { id = id } }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            return Json(new { result }, JsonRequestBehavior.AllowGet);


        }


        public ActionResult EditCategory(int id)
        {
            var db = new ManagerCategoryBusiness();
            var result = db.GetCategoryById(id);
            ViewBag.id = id;
            return PartialView(result);
        }

        public ActionResult _EditCategory(string fullName, string note, bool status = false, int id = 0)
        {
            // kiểm tra quyền thêm sửa 
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Update);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new ManagerCategoryBusiness();

            tbl_LoaiSP serviceCustomer = new tbl_LoaiSP();

            serviceCustomer.category_name = fullName;
            serviceCustomer.description = note;
            serviceCustomer.isactive = status;
            serviceCustomer.id = id;

            var result = db.EditCategory(serviceCustomer);

            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Update.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { log = "CapNhatThongTinDVT", newtdata = serviceCustomer }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));


            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddCategory()
        {
            int BranchId = 0;
            int.TryParse(user.BranchId.ToString(), out BranchId);
            ViewBag.BranchId = BranchId;

            return PartialView();
        }
        public ActionResult _AddCategory(string fullName, string note, bool status = false)
        {
            // kiểm tra quyền thêm 
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Addnew);

            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new ManagerCategoryBusiness();

            tbl_LoaiSP serviceCustomer = new tbl_LoaiSP();

            serviceCustomer.category_name = fullName;
            serviceCustomer.description = note;
            serviceCustomer.isactive = status;
            var result = db.AddCategory(serviceCustomer);

            // luu log - thêm mới
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Create.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { data = serviceCustomer }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));


            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllProductByIDCategory(int id)
        {
            ViewBag.id = id;
            var db = new ManagerCategoryBusiness();

            var result = db.BS_LstProductByCategoryID(id);

            return PartialView(result);
        }


        public ActionResult ThemmoiSP(int id)
        {
            ViewBag.id = id;

            return PartialView();
        }

        public ActionResult _ThemmoiSP(int id_product =0, int id=0)
        {
            // kiểm tra quyền thêm 
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Addnew);

            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new ManagerCategoryBusiness();

            tbl_detail_category item = new tbl_detail_category();

            item.id_product = id_product;
            item.id_category = id;
            item.isactive = true;
            var result = db.AddProductToCategory(item);

            // luu log - thêm mới
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Create.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { data = item }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));


            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _xoasanphamkhoinhom(int id)
        {
            // kiểm tra quyền thêm xóa 
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Delete);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new ManagerCategoryBusiness();
            var result = db.BS_xoasanphamkhoinhom(id);

            // luu log - xóa
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Delete.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { data = new { id = id } }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            return Json(new { result }, JsonRequestBehavior.AllowGet);


        }
    }
}