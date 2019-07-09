using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Web.UI;
using CS.Common.Const;
using CS.Data.Business;
using CS.Data.DataContext;
using CS.Data.Model;
using CS.Data.Untils;
using Newtonsoft.Json;
using WebApplication;
using WebApplication.Utility;

namespace BAN_HANG.Controllers
{
    public class RoleUserController : BaseController
    {
        private DB_CSEntities1 entity = new DB_CSEntities1();

        SessionUser user = GetSessionBusiness.GetUser();
       
        /// long

        public ActionResult Index()
        {
            ViewBag.id = user.Roleid;
            return View();
        }

        public ActionResult ListUserInSystem()
        {

            return PartialView();
        }
        public ActionResult _ListUserInSystem()
        {
            var db = new RoleUserBusiness();

            int userid = user.Id;
            int roleiduser = user.Roleid;
            int branchId = user.BranchId;

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

            var result = db.ListUserInSystem(search, currentPage, 10, userid, roleiduser, branchId);

            return Content(JsonConvert.SerializeObject(new
            {
                data = result.Data,
                draw = draw,
                recordsFiltered = result.Total,
                recordsTotal = result.Total
            }));
        }

        public ActionResult AddAccountIntoRole(int Id)
        {
            ViewBag.id = Id;
            return PartialView();
        }

        public ActionResult _AddAccountIntoRole(int idAccount, int idRole)
        {
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Addnew);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new RoleUserBusiness();
            MenuPermission item = new MenuPermission();
            item.RoleId = idRole;
            item.UserId = idAccount;
            var result = db.Add_Account_Into_Role(item);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteUserAndRole(int idrole, int iduser, int idroleuser)
        {
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Delete);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new RoleUserBusiness();
            var result = db.DeleteRoleUserAndMenu(idrole, iduser, idroleuser);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListMenuOfRole(int iduser, int idrole)
        {
            ViewBag.iduser = iduser;
            ViewBag.idrole = idrole;

            var result = entity.User.FirstOrDefault(m => m.Id == iduser);
            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.UserName))
                {
                    ViewBag.username = result.UserName;
                }
                else
                {
                    ViewBag.username = result.FullName;
                }
            }


            return PartialView();
        }

        public ActionResult ListMenuOfRoleAndAccount(int iduser, int idrole)
        {
            ViewBag.iduser = iduser;
            ViewBag.idrole = idrole;

            return PartialView();
        }
        public ActionResult _ListMenuOfRoleAndAccount(int iduser, int idrole)
        {
            var db = new RoleUserBusiness();

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

            var result = db.ListMenuOfRoleAndAccount(search, currentPage, 10, iduser, idrole);

            return Content(JsonConvert.SerializeObject(new
            {
                data = result.Data,
                draw = draw,
                recordsFiltered = result.Total,
                recordsTotal = result.Total
            }));
        }

        public ActionResult UpdateSelectInMenu(int id, string type, bool gt)
        {
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Update);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new RoleUserBusiness();
            EditSelectMenu item = new EditSelectMenu();
            item.id = id;
            item.type = type;
            item.gt = gt;
            var result = db.Edit_Select_Menu(item);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteMenuInRoleAccount(int id)
        {
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Delete);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new RoleUserBusiness();
            EditSelectMenu item = new EditSelectMenu();
            item.id = id;
            var result = db.Delete_Select_Menu(item);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddMenuIntoAccount(int Id, int idrole)
        {
            ViewBag.id = Id;
            ViewBag.idrole = idrole;
            return PartialView();
        }
        public ActionResult _AddMenuIntoAccount(int idAccount, int idRole, int idMenu, string IsRead, string IsCreate, string IsUpdate, string IsDelete, string IsExport)
        {
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Addnew);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new RoleUserBusiness();
            MenuPermission item = new MenuPermission();
            item.MenuId = idMenu;
            item.RoleId = idRole;
            item.UserId = idAccount;
            item.IsRead = bool.Parse(IsRead);
            item.IsCreate = bool.Parse(IsCreate);
            item.IsUpdate = bool.Parse(IsUpdate);
            item.IsDelete = bool.Parse(IsDelete);
            item.IsExport = bool.Parse(IsExport);

            var result = db.AddMenuIntoAccountRole(item);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllAccountByRole(int IdRole)
        {
            ViewBag.id = IdRole;

            int userid = user.Id;
            int roleid = user.Roleid;
            int branchId = user.BranchId;

            var db = new RoleBusiness();
            var result = db.ListUserByRoleId(roleid, userid, branchId, IdRole);

            return PartialView(result);
        }

        public ActionResult GetAllMenuByRole(int IdRole)
        {
            ViewBag.id = IdRole;


            int userid = user.Id;
            int roleid = user.Roleid;
            int branchId = user.BranchId;


            var db = new RoleUserBusiness();
            var result = db.ListMenuByIdUserLogin(roleid, userid, branchId, IdRole);
            return PartialView(result);
        }

        public ActionResult _AddNewRoleUserMenu()
        {
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Addnew);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }


            int userid = user.Id;
            int roleid = user.Roleid;
            int branchId = user.BranchId;

            List<int> lstAccount = (List<int>)Newtonsoft.Json.JsonConvert.DeserializeObject(HttpContext.Request["listAccount"], typeof(List<int>));
            List<int> listMenu = (List<int>)Newtonsoft.Json.JsonConvert.DeserializeObject(HttpContext.Request["listMenu"], typeof(List<int>));
            List<string> lstOptionMenu = (List<string>)Newtonsoft.Json.JsonConvert.DeserializeObject(HttpContext.Request["lstOptionMenu"], typeof(List<string>));

            bool statusCheckAllAcount = false;
            int RoleId = 0;
            int.TryParse(HttpContext.Request["roleId"], out RoleId);
            bool.TryParse(HttpContext.Request["statusCheckAllAcount"], out statusCheckAllAcount);

            var db = new RoleUserBusiness();

            var result = db._AddNewRoleUserMenu(lstAccount, listMenu, lstOptionMenu, RoleId, statusCheckAllAcount);

            return Json(new { result }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Themmoimenuchotaikhoan(int idMember, int idrole)
        {

            ViewBag.id = idMember;


            int userid = user.Id;
            int roleid = user.Roleid;
            int branchId = user.BranchId;

            var db = new RoleUserBusiness();
            var result = db.BS_Danhsachmenuthemmoivaotaikhoan(roleid, userid, branchId, idMember, idrole);

            return PartialView(result);
        }
    }
}