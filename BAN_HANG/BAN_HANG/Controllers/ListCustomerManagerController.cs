using System;
using System.Collections.Generic;
using System.Globalization;
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
using WebApplication;
using WebApplication.Utility;

namespace BAN_HANG.Controllers
{
    public class ListCustomerManagerController : BaseController
    {
        // GET: ListCustomerManager
        private DB_CSEntities1 entity = new DB_CSEntities1();
        SessionUser user = GetSessionBusiness.GetUser();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListCustomer()
        {
            return PartialView();
        }

        public ActionResult _ListCustomer()
        {
            var db = new CustomerBusiness();
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
            var result = db.ListCustomer(search, currentPage, 10, idcenter, roleid);

            return Content(JsonConvert.SerializeObject(new
            {
                data = result.Data,
                draw = draw,
                recordsFiltered = result.Total,
                recordsTotal = result.Total

            }));
        }
        public ActionResult DeleteCustomer(int id)
        {
            // kiểm tra quyền thêm xóa 
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Delete);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new CustomerBusiness();
            var result = db.DeleteCustomerById(id);

            // luu log - xóa
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Delete.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { data = new { id = id } }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            return Json(new { result }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult EditCustomer(int id)
        {
            var db = new CustomerBusiness();
            var result = db.GetCustomerById(id);
            ViewBag.id = id;
            return PartialView(result);
        }
        public ActionResult _EditCustomer(string fullName, string cmt, string birth, string address, string phone, string note, int branchId = 0, int id = 0)
        {
            // kiểm tra quyền thêm sửa 
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Update);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }



            var db = new CustomerBusiness();
            service_customer serviceCustomer = new service_customer();
            serviceCustomer.fullname = fullName;
            serviceCustomer.id_card = cmt;
            serviceCustomer.address = address;
            serviceCustomer.mobile = phone;
            serviceCustomer.note = note;
            serviceCustomer.id_center = branchId;
            serviceCustomer.id = id;

            DateTime _birth;
            if (!string.IsNullOrEmpty(birth))
            {
                if (!DateTime.TryParseExact(birth, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out _birth))
                {
                    SystemMessage systemMessage = new SystemMessage();
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = string.Format(SystemMessageConst.ValidateConst.DateIsNotValid, "Ngày sinh");
                    return Json(new { result = systemMessage }, JsonRequestBehavior.AllowGet);
                }
                serviceCustomer.birthday = _birth;
            }
            var result = db.EditCustomer(serviceCustomer);

            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Update.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { log = "CapNhatThongTinKH", newtdata = serviceCustomer }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));


            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddCustomer()
        {
            int BranchId = 0;
            int.TryParse(user.BranchId.ToString(), out BranchId);
            ViewBag.BranchId = BranchId;

            return PartialView();
        }
        public ActionResult _AddCustomer( string fullName, string cmt, string birth, string address, string phone, string note, int branchId = 0)
        {
            // kiểm tra quyền thêm 
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Addnew);

            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new CustomerBusiness();

            service_customer serviceCustomer = new service_customer();


            var getbrand = entity.Branch.FirstOrDefault(m => m.Id == branchId);

            serviceCustomer.fullname = fullName;
            serviceCustomer.id_card = cmt;
            serviceCustomer.address = address;
            serviceCustomer.mobile = phone;
            serviceCustomer.note = note;
            serviceCustomer.status = 1;
            serviceCustomer.id_center = branchId;
            serviceCustomer.code_center = getbrand.Branch_code;
            serviceCustomer.created_by = user.FullName;
            serviceCustomer.created_at = DateTime.Now;
            
            DateTime _birth;

            if (!string.IsNullOrEmpty(birth))
            {
                if (!DateTime.TryParseExact(birth, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out _birth))
                {
                    SystemMessage systemMessage = new SystemMessage();
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = string.Format(SystemMessageConst.ValidateConst.DateIsNotValid, "Ngày sinh");
                    return Json(new { result = systemMessage }, JsonRequestBehavior.AllowGet);
                }
                serviceCustomer.birthday = _birth;
            }
            var result = db.AddCustomer(serviceCustomer);

            // luu log - thêm mới
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Create.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { data = serviceCustomer }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));


            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
      
    }
}