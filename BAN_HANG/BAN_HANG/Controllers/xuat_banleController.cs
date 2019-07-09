using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS.Common.Const;
using CS.Data.Business;
using CS.Data.Model;
using CS.Data.Untils;
using Newtonsoft.Json;
using WebApplication.Utility;

namespace BAN_HANG.Controllers
{
    public class xuat_banleController : BaseController
    {
        // GET: xuat_banle
        SessionUser user = GetSessionBusiness.GetUser();
        public ActionResult Index()
        {
            var db = new xuat_dailyBusiness();
            ViewBag.nguoixuat = user.FullName;
            long number = db.GetBillNumer();
            ViewBag.number = number;
            ViewBag.code_brand = user.Code_brand;

            return View();
        }
        [HttpPost]
        public ActionResult _xuat_ban_le(FormCollection formCollection)
        {
            // kiểm tra quyền thêm sửa 
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Addnew);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            Add_Bill item = new Add_Bill();

            item.type = "BL";
            item.loaibill = 2;
            item.data = formCollection.Get("data");

            item.billType = formCollection.Get("billType");
            item.bonusTotal = long.Parse(formCollection.Get("bonusTotal"));
            item.total = long.Parse(formCollection.Get("total"));
            item.discount = long.Parse(formCollection.Get("discount"));

            item.billNumber = formCollection.Get("billNumber");
            item.ngayxuat = formCollection.Get("ngayxuat");
            item.nguoixuat = formCollection.Get("nguoixuat");
            item.notebill = formCollection.Get("notebill");

            item.tenkh = formCollection.Get("tenkh");
            item.diachi = formCollection.Get("diachi");
            //item.nguoidaidien = formCollection.Get("nguoidaidien");
            item.dienthoai = formCollection.Get("dienthoai");

            item.id_center = user.BranchId;
            item.id_thaotac = user.Id;
            item.code_center = user.Code_brand;
            item.tennguoithaotac = user.FullName;

            var db = new xuat_dailyBusiness();

            var result = db.AddBillProduct(item);

            // luu log - thêm mới
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Create.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { data = item }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));


            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
    }
}