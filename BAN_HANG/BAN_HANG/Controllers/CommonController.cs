using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using CS.Common.Const;
using CS.Data.Business;
using CS.Data.DataContext;
using CS.Data.Model;
using CS.Data.Untils;
using WebApplication;
using WebApplication.Utility;

namespace BAN_HANG.Controllers
{
    public class CommonController : BaseController
    {
        private DB_CSEntities1 entity = new DB_CSEntities1();
        SessionUser user = GetSessionBusiness.GetUser();





        public ActionResult GetAllCenter(string id, bool hasAll = false, int idSelected = -1, bool isDisable = false)
        {

            int roleid = user.Roleid;
            int idcenter = user.BranchId;

            ViewBag.id = id;
            ViewBag.hasAll = hasAll;

            if (roleid != 1)
            {
                ViewBag.idSelected = idcenter;
            }
            else
            {
                ViewBag.idSelected = idSelected;
            }


            var checkBrandch = entity.Branch.FirstOrDefault(m => m.Id == idcenter);

            if (checkBrandch.IsParent == true || user.Roleid== SystemMessageConst.Role.Admin)
            {
                isDisable = false;
            }
            else
            {
                isDisable = true;
            }

            ViewBag.isDisable = isDisable;

            var db = new CommonBusiness();

            var result = db.GetAllCenter(roleid, idcenter);

            return PartialView(result);
        }
        public ActionResult GetAllBranchByRoleLogin(string id, bool hasAll = false, int idSelected = 0, bool isDisable = false)
        {
            int userid = 0, roleid = 0, BranchId = 0;

            int.TryParse(user.Id.ToString(), out userid);
            int.TryParse(user.Roleid.ToString(), out roleid);
            int.TryParse(user.BranchId.ToString(), out BranchId);

            var db = new CommonBusiness();
            var result = db.GetAllBranchsByRoleIdLogin(userid, roleid, BranchId);
            var checkRole = entity.Role.FirstOrDefault(m => m.Id == roleid);
            var checkBrandch = entity.Branch.FirstOrDefault(m => m.Id == BranchId);

            if (checkRole.Level == 12 || checkBrandch.IsParent == true)
            {
                ViewBag.isDisable = false;
                hasAll = true;
            }
            else
            {
                ViewBag.isDisable = true;
            }
            ViewBag.id = id;
            ViewBag.idSelected = idSelected;
            ViewBag.hasAll = hasAll;

            return PartialView(result);
        }
        public ActionResult GetListAccountParentByLevelAndCenter(string id, bool hasAll = false, int idSelected = 0, bool isDisable = false, int level = 0, int idcenter = 0)
        {
            int userid = user.Id;
            int roleid = user.Roleid;
            int branchId = user.BranchId;

            if (roleid != SystemMessageConst.Role.Admin)
            {
                idcenter = branchId;
            }
            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;

            var db = new CommonBusiness();
            var result = db.GetAllAccountParentByCenter(idcenter, level);
            return PartialView("GetListAccountParent", result);
        }
        public ActionResult ListStatus(string id, bool hasAll = false, bool idSelected = false, bool isDisable = false)
        {
            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;
            return PartialView();
        }
        public ActionResult GetListAccountParent(string id, bool hasAll = false, int idSelected = 0, bool isDisable = false, int idacc = 0, int idcenter = 0)
        {
            int roleid = user.Roleid;
            int BranchId = user.BranchId;

            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;

            var db = new CommonBusiness();
            var levelMax = db.GetLevelMaxByIdAcc(idacc);
            var result = db.GetAllAccountParentByCenter(idcenter, levelMax);
            return PartialView(result);
        }

        public ActionResult GetAllRoleByUserRole(string id, bool hasAll = false, int idSelected = -1, bool isDisable = false)
        {
            int userid = user.Id;
            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;

            var db = new CommonBusiness();
            var levelMax = db.GetLevelMaxByIdAcc(userid);
            var result = db.GetAllRoleByLevel(levelMax);
            return PartialView(result);
        }
        public ActionResult GetListParameterByKey(string key, string id, bool hasAll = false, string idSelected = "0", bool isDisable = false)
        {
            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;
            var db = new CommonBusiness();
            var result = db.GetListParametersByKey(key);
            return PartialView(result);
        }

        public ActionResult GetAllRoleLevel(string id, bool hasAll = false, int idSelected = 0, bool isDisable = false)
        {
            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;

            var db = new CommonBusiness();
            var result = db.GetAllRoleLevel();
            return PartialView(result);
        }
        public ActionResult GelAllRoleChildrenByIdAdmin(string id, bool hasAll = false, int idSelected = 0, bool isDisable = false)
        {

            string userid = user.Id.ToString();
            string roleid = user.Roleid.ToString();
            string BranchId = user.BranchId.ToString();


            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;

            var db = new CommonBusiness();
            var result = db.GelAllRoleChildrenByIdAdmin(userid, roleid, BranchId);
            return PartialView(result);
        }

        public ActionResult   DS_thongso_theoid_sp(string id, bool hasAll = false, int idSelected = 0, bool isDisable = false, int id_product = 0)
        {
           

            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;

            var db = new CommonBusiness();
            var result = db.CM_DS_thongso_theoid_sp(id_product);
            return PartialView(result);
        }

        public ActionResult LoadAll_DS_thongso_theoid_sp(string id, bool hasAll = false, int idSelected = 0, bool isDisable = false, int id_product = 0)
        {


            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;

            var db = new CommonBusiness();
            var result = db.CM_LoadAll_DS_thongso_theoid_sp(id_product);
            return PartialView(result);
        }

        public ActionResult DS_Gia_SP(string id, bool hasAll = false, int idSelected = 0, bool isDisable = false, int id_product = 0)
        {


            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;

            var db = new CommonBusiness();
            var result = db.CM_DS_Gia_SP(id_product);
            return PartialView(result);
        }

        public ActionResult LoadAll_DS_Price_theoid_sp(string id, bool hasAll = false, int idSelected = 0, bool isDisable = false)
        {


            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;

            var db = new CommonBusiness();
            var result = db.CM_LoadAll_DS_Price_theoid_sp();
            return PartialView(result);
        }

        public ActionResult DS_Unit(string id, bool hasAll = false, int idSelected = 0, bool isDisable = false)
        {


            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;

            var db = new CommonBusiness();
            var result = db.CM_DS_Unit();
            return PartialView(result);
        }

        public ActionResult DS_SP(string id, bool hasAll = false, int idSelected = 0, bool isDisable = false)
        {


            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;

            var db = new CommonBusiness();
            var result = db.CM_DS_SP();
            return PartialView(result);
        }


        public ActionResult DS_NguonNhap(string id, bool hasAll = false, int idSelected = 0, bool isDisable = false)
        {


            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;


            var db = new CommonBusiness();
            var result = db.CM_DS_NguonNhap(user.BranchId);
            return PartialView(result);
        }

        public ActionResult DS_ChiNhanh(string id, bool hasAll = false, int idSelected = 0, bool isDisable = false)
        {


            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;

            var db = new CommonBusiness();
            var result = db.CM_DS_ChiNhanh(user.BranchId);
            return PartialView(result);
        }


        
        public ActionResult LoadBrandByRoleLogin(string id, bool hasAll = false, int idSelected = 0, bool isDisable = false)
        {
            int userid = 0, roleid = 0, BranchId = 0;

            int.TryParse(user.Id.ToString(), out userid);
            int.TryParse(user.Roleid.ToString(), out roleid);
            int.TryParse(user.BranchId.ToString(), out BranchId);

            var db = new CommonBusiness();
            var result = db.GetAllBranchsByRoleIdLogin(userid, roleid, BranchId);
            var checkRole = entity.Role.FirstOrDefault(m => m.Id == roleid);
            var checkBrandch = entity.Branch.FirstOrDefault(m => m.Id == BranchId);

            if (checkRole.Level == 12 || checkBrandch.IsParent == true)
            {
                ViewBag.isDisable = false;
                ViewBag.idSelected = idSelected;
            }
            else
            {
                ViewBag.isDisable = true;
                ViewBag.idSelected = user.BranchId;
            }

            ViewBag.id = id;
            return PartialView(result);
        }


        public ActionResult DS_SP_TheoDaiLy(string id, bool hasAll = false, int idSelected = 0, bool isDisable = false)
        {


            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;

            var db = new CommonBusiness();
            var result = db.CM_DS_SP_TheoDaiLy(user.BranchId);
            return PartialView(result);
        }

        public ActionResult DS_Nhom_SP_TonKho_TheoID(int id_p=0, string id="", bool hasAll = false, int idSelected = 0, bool isDisable = false)
        {


            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;

            var db = new CommonBusiness();
            var result = db.CM_DS_Nhom_SP_TonKho_TheoID(user.BranchId, id_p);
            return PartialView(result);
        }
        public ActionResult DS_Nhom_SP_TonKho_TheoID_Json(int id_p = 0, string id = "", bool hasAll = false, int idSelected = 0, bool isDisable = false)
        {


            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;

            var db = new CommonBusiness();
            var result = db.CM_DS_Nhom_SP_TonKho_TheoID(user.BranchId, id_p);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DS_Gia_SP_TheoID(string id, bool hasAll = false, int idSelected = 0, bool isDisable = false, int id_product = 0)
        {
            ViewBag.id = id;
            ViewBag.hasAll = hasAll;
            ViewBag.idSelected = idSelected;
            ViewBag.isDisable = isDisable;

            var db = new CommonBusiness();
            var result = db.CM_DS_Gia_SP_TheoID(id_product);
            // return Json(new { result }, JsonRequestBehavior.AllowGet);
            return PartialView(result);
        }
        public ActionResult LoadUnit_ByID_ProductCallBack(int id_product = 0)
        {

            var db = new CommonBusiness();
            var result = db.CM_LoadUnit_ByID_ProductCallBack(id_product);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
    }

}