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
    public class danhsachphieuxuatController : BaseController
    {
        // GET: danhsachphieuxuat
        SessionUser user = GetSessionBusiness.GetUser();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadListBill()
        {
            return PartialView();
        }
        public ActionResult _LoadListBill(string date_sale,int status)
        {
            var db = new danhsachphieuxuatBusiness();
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


            //DateTime dateS;
            //DateTime.TryParse(date_sale, out dateS);
            //date_sale = dateS.ToString("yyyy-MM-dd");
            //if (date_sale == "0001-01-01")
            //{
            //    date_sale = DateTime.Now.ToString("yyyy-MM-dd");
            //}

            var result = db.BS_LoadListBill(search, currentPage, 10, date_sale, status, user.BranchId);

            return Content(JsonConvert.SerializeObject(new
            {
                data = result.Data,
                draw = draw,
                recordsFiltered = result.Total,
                recordsTotal = result.Total
            }));

        }

        public ActionResult ExportPhieuXuat(int id_bill)
        {
            ViewBag.id_bill = id_bill;
            return PartialView();
        }
        public ActionResult _ExportPhieuXuat(int id_bill)
        {

            var db = new xuat_dailyBusiness();
            ViewBag.id_bill = id_bill;
            ViewBag.TTKH = db.ThongTinKH_TheoIdBill(id_bill);
            ViewBag.TTBill = db.ThongTinDonHangTheoID_Bill(id_bill);
            ViewBag.ChiTietSP = db.ChiTietSanPhamTheoID_Bill(id_bill);
            ViewBag.TTDaiLy = db.ThongTinDaiLy(user.BranchId);

            return PartialView();
        }

        public ActionResult Xoadonhang(int id_bill)
        {
            var db = new danhsachphieuxuatBusiness();

            var result = db.BS_Huydonhang(id_bill, user.FullName);

            Add_Bill item = new Add_Bill();

            item.id_center = user.BranchId;
            item.id_thaotac = user.Id;
            item.code_center = user.Code_brand;
            item.tennguoithaotac = user.FullName;
            item.id_bill = id_bill;
            item.data = "Hủy đơn hàng";

            // luu log - thêm mới
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Create.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { data = item }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Chinhsuadonhang(int id_bill)
        {
            var db = new xuat_dailyBusiness();
            ViewBag.id_bill = id_bill;
            ViewBag.TTKH = db.ThongTinKH_TheoIdBill(id_bill);
            ViewBag.TTBill = db.ThongTinDonHangTheoID_Bill(id_bill);
            ViewBag.ChiTietSP = db.ChiTietSanPhamTheoID_Bill(id_bill);

            return PartialView();
        }
        [HttpPost]
        public ActionResult _Chinhsuadonhang(FormCollection formCollection)
        {
            // kiểm tra quyền thêm sửa 
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Addnew);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            Add_Bill item = new Add_Bill();

            item.id_bill = int.Parse(formCollection.Get("id_bill"));

            item.data = formCollection.Get("data");

            item.billType = formCollection.Get("billType");
            item.bonusTotal = long.Parse(formCollection.Get("bonusTotal"));
            item.total = long.Parse(formCollection.Get("total"));
            item.discount = long.Parse(formCollection.Get("discount"));

            item.billNumber = formCollection.Get("billNumber");
            item.ngayxuat = formCollection.Get("ngayxuat");
            item.nguoixuat = formCollection.Get("nguoixuat");

            item.tenkh = formCollection.Get("tenkh");
            item.diachi = formCollection.Get("diachi");
            item.dienthoai = formCollection.Get("dienthoai");

            item.id_center = user.BranchId;
            item.id_thaotac = user.Id;
            item.code_center = user.Code_brand;
            item.tennguoithaotac = user.FullName;

            var db = new danhsachphieuxuatBusiness();

            var result = db.EditBillProduct(item);

            // luu log - thêm mới
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Create.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { data = item }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));


            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _Hoanthanhdonhang(int id_bill)
        {
            var db = new danhsachphieuxuatBusiness();

            var result = db.BS_Hoantatdonhang(id_bill, user.FullName);

            Add_Bill item = new Add_Bill();

            item.id_center = user.BranchId;
            item.id_thaotac = user.Id;
            item.code_center = user.Code_brand;
            item.tennguoithaotac = user.FullName;
            item.id_bill = id_bill;
            item.data = "Hoàn thành đơn hàng";

            // luu log - thêm mới
            CheckRuleAndSaveLog.ReturnCheckRuleAndSaveLog(DbLogType.Create.ToString(), result.IsSuccess, JsonConvert.SerializeObject(new { data = item }, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
    }
}