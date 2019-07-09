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
    public class gioithieuwebsiteController : BaseController
    {
        // GET: gioithieuwebsite
        SessionUser user = GetSessionBusiness.GetUser();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult danhsachtranggioithieu()
        {
            return PartialView();
        }
        public ActionResult _danhsachtranggioithieu()
        {
            var db = new gioithieuwebsiteBusiness();
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

            var result = db.BS_GetAllPage(search, currentPage, 10);

            return Content(JsonConvert.SerializeObject(new
            {
                data = result.Data,
                draw = draw,
                recordsFiltered = result.Total,
                recordsTotal = result.Total
            }));

        }

        public ActionResult EditPage(int id)
        {
            ViewBag.id = id;
            var db = new gioithieuwebsiteBusiness();
            var result = db.GetPagebyid(id);
            if (result !=null)
            {
                ViewBag.noidung = result.noidung;
            }
            else
            {
                ViewBag.noidung = "";
            }
            return PartialView(result);
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult _EditPage(FormCollection formCollection)
        {
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Update);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new gioithieuwebsiteBusiness();
            tbl_gioithieu item = new tbl_gioithieu();

            item.id = int.Parse(formCollection.Get("id"));
            item.noidung = formCollection.Get("noidung");
            item.ngaychinhsua = DateTime.Now;
            item.tennguoisua = user.FullName;

            var result = db.BS_EditPage(item);
            // luu log - sửa 
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Update.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { data = item }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
    }
}