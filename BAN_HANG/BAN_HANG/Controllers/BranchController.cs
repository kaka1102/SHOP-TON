using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS.Common.Const;
using CS.Data.Business;
using CS.Data.DataContext;
using Newtonsoft.Json;
using WebApplication.Utility;

namespace BAN_HANG.Controllers
{
    public class BranchController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadListBranch()
        {
            return PartialView();
        }

        public ActionResult _LoadListBranch()
        {
            var db = new BranchBusiness();
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

            var result = db.BS_GetAllBranch(search, currentPage, 10);

            return Content(JsonConvert.SerializeObject(new
            {
                data = result.Data,
                draw = draw,
                recordsFiltered = result.Total,
                recordsTotal = result.Total
            }));

        }

        public ActionResult ChangeStatusBranch(int id, int status = 0)
        {
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Update);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }
            var db = new BranchBusiness();

            Branch item = new Branch();
            item.Id = id;
            item.is_active = status;

            var result = db.BS_ChangeStatusBranch(item);
            // luu log - sửa 
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Update.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { data = item }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditBranch(int id)
        {
            var db = new BranchBusiness();
            var result = db.BS_GetBranchByID(id);
            ViewBag.id = id;
            return PartialView(result);
        }

        public ActionResult _EditBranch(int id = 0, string name = "", string add = "", string phone = "", string tax = "", string brandcode = "", string shortname = "", string city = "", bool status = false)
        {
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Update);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new BranchBusiness();
            Branch item = new Branch();

            item.Id = id;
            item.Name = name;
            item.AddressName = add;
            item.Phone = phone;
            item.Tax_Number = tax;
            item.Branch_code = brandcode;
            item.ShortName = shortname;
            item.City = city;
            item.is_active = Convert.ToInt32(status);

            var result = db.BS_EditBrandch(item);
            // luu log - sửa 
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Update.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { data = item }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddNewBrandch()
        {
            return PartialView();
        }

        public ActionResult _AddNewBrandch(string name = "", string add = "", string phone = "", string tax = "", string brandcode = "", string shortname = "", string city = "", bool status = false)
        {
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Addnew);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new BranchBusiness();
            Branch item = new Branch();

            item.Name = name;
            item.AddressName = add;
            item.Phone = phone;
            item.Tax_Number = tax;
            item.Branch_code = brandcode;
            item.ShortName = shortname;
            item.City = city;
            item.is_active = Convert.ToInt32(status);

            var result = db.BS_AddNewBrandch(item);
            // luu log - thêm mới
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Create.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { data = item }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
    }
}