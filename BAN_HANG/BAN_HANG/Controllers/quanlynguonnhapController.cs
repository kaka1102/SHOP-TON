using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS.Common.Const;
using CS.Data.Business;
using CS.Data.DataContext;
using CS.Data.Model;
using CS.Data.Untils;
using Newtonsoft.Json;
using WebApplication.Utility;

namespace BAN_HANG.Controllers
{
    public class quanlynguonnhapController : BaseController
    {
        // GET: quanlynguonnhap
        private DB_CSEntities1 entity = new DB_CSEntities1();
        SessionUser user = GetSessionBusiness.GetUser();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Listnguonnhap()
        {
            return PartialView();
        }

        public ActionResult _Listnguonnhap()
        {
            var db = new quanlynguonnhapBusiness();
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
            var result = db.BS_Listnguonnhap(search, currentPage, 10, idcenter, roleid);

            return Content(JsonConvert.SerializeObject(new
            {
                data = result.Data,
                draw = draw,
                recordsFiltered = result.Total,
                recordsTotal = result.Total

            }));
        }
        public ActionResult DeleteNguonnhap(int id)
        {
            // kiểm tra quyền thêm xóa 
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Delete);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new quanlynguonnhapBusiness();
            var result = db.DeleteNguonNhapById(id);

            // luu log - xóa
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Delete.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { data = new { id = id } }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            return Json(new { result }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult EditNguonnhap(int id)
        {
            var db = new quanlynguonnhapBusiness();
            var result = db.GetnguonnhapById(id);
            ViewBag.id = id;
            return PartialView(result);
        }
        public ActionResult _EditNguonnhap(string fullName, string code, string address, string phone, string note, int branchId = 0, int id = 0)
        {
            // kiểm tra quyền thêm sửa 
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Update);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }



            var db = new quanlynguonnhapBusiness();

            sys_nguonnhap serviceCustomer = new sys_nguonnhap();
            serviceCustomer.tennguon = fullName;
            serviceCustomer.code = code;
            serviceCustomer.diachi = address;
            serviceCustomer.sdt = phone;
            serviceCustomer.description = note;
            serviceCustomer.id_center = branchId;
            serviceCustomer.id = id;
            
            var result = db.Editnguon(serviceCustomer);

            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Update.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { log = "CapNhatThongTin_nguonnhap", newtdata = serviceCustomer }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));


            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddNguonnhap()
        {
            int BranchId = 0;
            int.TryParse(user.BranchId.ToString(), out BranchId);
            ViewBag.BranchId = BranchId;

            return PartialView();
        }
        public ActionResult _AddNguonnhap(string fullName,  string address, string phone, string code, string note, int branchId = 0)
        {
            // kiểm tra quyền thêm 
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Addnew);

            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new quanlynguonnhapBusiness();

            sys_nguonnhap serviceCustomer = new sys_nguonnhap();
            
            serviceCustomer.tennguon = fullName;
            serviceCustomer.diachi = address;
            serviceCustomer.sdt = phone;
            serviceCustomer.code = code;
            serviceCustomer.description = note;
            serviceCustomer.isactive = true;
            serviceCustomer.id_center = branchId;

            var result = db.AddNguonnhap(serviceCustomer);

            // luu log - thêm mới
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Create.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { data = serviceCustomer }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));


            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

    }
}