using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAN_HANG.Areas.Client.Business;
using CS.Common.Const;
using CS.Data.DataContext;
using Newtonsoft.Json;
using WebApplication.Utility;

namespace BAN_HANG.Areas.Client.Controllers
{
    public class LienheController : Controller
    {
        // GET: Client/Lienhe
        public ActionResult Index()
        {
            var db = new LienheBusiness();
            var result = db.GetPage_Lienhe();
            return View(result);
        }

        public ActionResult _AddNewContact(string hoten = "", string diachi = "", string email = "", string sdt = "", string tieude = "Không có tiêu đề", string noidung = "")
        {


            var db = new LienheBusiness();
            tbl_lienhe item = new tbl_lienhe();

            item.hoten = hoten;
            item.diachi = diachi;
            item.dienthoai = sdt;
            item.email = email;
            item.tieude = tieude;
            item.noidung = noidung;
            item.trangthai = 0;
            item.ngaygui=DateTime.Now;

            var result = db.BS_AddNewContact(item);
            // luu log - thêm mới
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Create.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { data = item }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
    }
}