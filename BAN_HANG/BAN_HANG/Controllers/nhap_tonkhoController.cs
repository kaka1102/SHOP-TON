using System;
using System.Globalization;
using CS.Common.Const;
using CS.Data.Business;
using CS.Data.DataContext;
using System.Web.Mvc;
using CS.Data.Model;
using CS.Data.Untils;
using Newtonsoft.Json;
using WebApplication;
using WebApplication.Utility;

namespace BAN_HANG.Controllers
{
    public class nhap_tonkhoController : BaseController
    {
        DB_CSEntities1 entity = new DB_CSEntities1();
        // GET: nhap_tonkho
        SessionUser user = GetSessionBusiness.GetUser();

        public ActionResult NhapKho()
        {
            return PartialView();
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListProduct()
        {
            var db = new nhap_tonkhoBussiness();
            var result = db.LoadAll_TonKho();
            return PartialView(result);
        }
        public ActionResult Soluongchitiet_SP(int id)
        {
            ViewBag.id = id;
            var db = new nhap_tonkhoBussiness();
            var result = db.Soluongchitiet_SP(id);
            return PartialView(result);
        }


        public ActionResult _CongSL_SP(int id)
        {
            var db = new nhap_tonkhoBussiness();
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Update);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }
            var result = db.BS_CongSL_SP(id);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _Tru_SL_SP(int id)
        {
            var db = new nhap_tonkhoBussiness();
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Update);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var result = db.BS_Tru_SL_SP(id);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }




        public ActionResult _CongSL_Chil_SP(int id_p,string groupcode)
        {
            var db = new nhap_tonkhoBussiness();
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Update);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }
            var result = db.BS_CongSL_Chil_SP(id_p, groupcode);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _Tru_SL_Chil_SP(int id_p, string groupcode)
        {
            var db = new nhap_tonkhoBussiness();
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Update);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var result = db.BS_Tru_SL_Chil_SP(id_p, groupcode);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult XemLichSuNhapHang(int id_product)
        {
            ViewBag.id_product = id_product;
            return PartialView();
        }


        public ActionResult _XemLichSuNhapHang(int id_product)
        {
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

            
            var db = new nhap_tonkhoBussiness();
            var result = db.LoadLichSu_Nhap(search, currentPage, 10, id_product);


            // luu log - xem báo cáo 
            var name = Env.GetUserInfo("name");
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Visit.ToString(), true, JsonConvert.SerializeObject(new { data = new { type = "Xem lịch sử", nguoitao = name } }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));


            return Content(JsonConvert.SerializeObject(new
            {
                data = result.Data,
                draw = draw,
                recordsFiltered = result.Total,
                recordsTotal = result.Total
            }));
        }

        public ActionResult Xemchitiet_lichsu_sp(int id)
        {
            ViewBag.id = id;
            var db = new nhap_tonkhoBussiness();
            var result = db.bs_Xemchitiet_lichsu_sp(id);
            return PartialView(result);
        }


        public ActionResult _NhapKho(int id_product = 0, int soluong = 0, int id_nguonnhap = 0, float tongtien = 0, string group_code_product = "", string ngaynhap = "", string mota = "")
        {
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Addnew);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new nhap_tonkhoBussiness();

            sys_Nhap item = new sys_Nhap();

            System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
            DateTime n_n;
            DateTime.TryParseExact(ngaynhap, "dd/MM/yyyy", enUS,
                System.Globalization.DateTimeStyles.None,
                out n_n);


            double tb = 0;
            tb = Math.Round(tongtien / soluong, 0);

            item.id_product = id_product;
            item.soluong = soluong;
            item.id_nguonnhap = id_nguonnhap;
            item.tongtien = tongtien;
            item.group_code_product = group_code_product;
            item.ngaynhap = n_n;
            item.mota = mota;
            item.trangthai = true;
            item.nguoinhap = user.Id;
            item.id_center = user.BranchId;
            item.giatrungbinh = tb;

            var result = db.BS_NhapKho(item);
            // luu log - thêm mới
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Create.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { data = item }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
    }
}