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
using WebApplication.Utility;
using Newtonsoft.Json;
using WebApplication;


namespace BAN_HANG.Controllers
{
    public class RoleController : BaseController
    {
        SessionUser user = GetSessionBusiness.GetUser();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListRoleInSystem()
        {
            return PartialView();
        }

        public ActionResult _ListRoleInSystem()
        {
            var db = new RoleBusiness();
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


            var result = db.ListRoll(search, currentPage, 10);

            return Content(JsonConvert.SerializeObject(new
            {
                data = result.Data,
                draw = draw,
                recordsFiltered = result.Total,
                recordsTotal = result.Total
            }));
        }
        public ActionResult AddNewRole()
        {
            return PartialView();
        }
        public ActionResult _AddLevelRole(string RoleName, int IdLevelRole, string IsActive)
        {
            var db = new RoleBusiness();
            Role item = new Role();
            item.RoleName = RoleName;
            item.IsActive = bool.Parse(IsActive);
            item.Level = IdLevelRole;

            var result = db.AddRoleSystem(item);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditRole(int id)
        {
            var db = new RoleBusiness();
            var result = db.GetRoleSystemById(id);
            ViewBag.id = id;
            return PartialView(result);
        }
        public ActionResult _EditRole(string RoleName, int IdLevelRole, string IsActive, int Id = 0)
        {
            var db = new RoleBusiness();
            Role item = new Role();
            item.Id = Id;
            item.RoleName = RoleName;
            item.IsActive = bool.Parse(IsActive);
            item.Level = IdLevelRole;


            var result = db.EditRoleSystem(item);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InformationRoleSystem(int id)
        {
            var db = new RoleBusiness();
            var result = db.GetRoleSystemById(id);
            ViewBag.id = id;
            ViewBag.name = result.RoleName;
            return View();
        }

        public ActionResult ListAccountUsingRole(int id)
        {
            ViewBag.id = id;
            return PartialView();
        }
        public ActionResult _ListAccountUsingRole()
        {

            int userid = user.Id;
            int roleiduser = user.Roleid;
            int branchId = user.BranchId;
            
            var db = new RoleBusiness();
            int minRow = 0;
            int maxRow = 0;
            int Id = 0;
            int.TryParse(HttpContext.Request["start"], out minRow);
            int length = 10;
            int.TryParse(HttpContext.Request["length"], out length);
            maxRow = (minRow + length);
            int draw = 0;
            int.TryParse(HttpContext.Request["draw"], out draw);
            string search = HttpContext.Request["search[value]"].Trim();
            int currentPage = (minRow / 10) + 1;

            int.TryParse(HttpContext.Request["Id"], out Id);
            var result = db.ListAccountUsingRole(search, currentPage, 10, Id, userid, roleiduser, branchId);

            return Content(JsonConvert.SerializeObject(new
            {
                data = result.Data,
                draw = draw,
                recordsFiltered = result.Total,
                recordsTotal = result.Total
            }));
        }

        public ActionResult AddAccountUsingRole(int Id)
        {
            ViewBag.id = Id;
            return PartialView();
        }
        public ActionResult _AddAccountUsingRole(int idAccount, int idRole)
        {
            var db = new RoleBusiness();
            MenuPermission item = new MenuPermission();
            item.RoleId = idRole;
            item.UserId = idAccount;
            var result = db.AddAccount_Menu_UsingRole(item);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MessageModel()
        {
            return PartialView();
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

        public ActionResult DeleteAccountUsingRole(int Id)
        {
            var db = new RoleBusiness();
            var result = db.DeleteAccountUsingRole(Id);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadListMenuByIdUserLogin(int idMember, int idrole)
        {
            ViewBag.id = idMember;
            
            int userid = user.Id;
            int roleid = user.Roleid;
            int branchId = user.BranchId;

            var db = new RoleBusiness();
            var result = db.ListMenuByIdUserLogin(roleid, userid, branchId, idMember, idrole);

            return PartialView(result);
        }

        public ActionResult ListMenuInRole(int id)
        {
            ViewBag.id = id;
            return PartialView();
        }

        public ActionResult _ListMenuInRole()
        {


            var db = new RoleBusiness();
            int minRow = 0;
            int maxRow = 0;
            int Id = 0;
            int.TryParse(HttpContext.Request["start"], out minRow);
            int length = 10;
            int.TryParse(HttpContext.Request["length"], out length);
            maxRow = (minRow + length);
            int draw = 0;
            int.TryParse(HttpContext.Request["draw"], out draw);
            string search = HttpContext.Request["search[value]"].Trim();
            int currentPage = (minRow / 10) + 1;

            int.TryParse(HttpContext.Request["Id"], out Id);

            var result = db.BS_ListMenuInRole(search, currentPage, 10, Id);

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

            var db = new RoleBusiness();
            EditSelectMenu item = new EditSelectMenu();
            item.id = id;
            item.type = type;
            item.gt = gt;
            var result = db.Edit_Select_Menu(item);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DeleteMenuInRole(int id)
        {
            var db = new RoleBusiness();
            var result = db.BS_DeleteMenuInRole(id);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddMenuIntoRole(int Id)
        {
            ViewBag.idrole = Id;
            return PartialView();
        }

        public ActionResult Themmoimenuchoquyen(int idrole)
        {
            

            int userid = user.Id;
            int roleid_login = user.Roleid;

            var db = new RoleBusiness();
            var result = db.BS_Danhsachmenuthemmoivaoquyen(roleid_login, userid, idrole);

            return PartialView(result);
        }

        public ActionResult _AddMenuIntoRole(int idRole, int idMenu, string IsRead, string IsCreate, string IsUpdate, string IsDelete, string IsExport)
        {
            // kiểm tra quyền thêm sửa xóa của từng menu , nhớ truyền type tương ứng
            var sys = CheckActiveMenu.ReturnActive(SystemMessageConst.TypeAction.Addnew);
            if (sys.IsSuccess == false)
            {
                return Json(new { result = sys }, JsonRequestBehavior.AllowGet);
            }

            var db = new RoleBusiness();
            MenuPermission item = new MenuPermission();
            item.MenuId = idMenu;
            item.RoleId = idRole;
            item.IsRead = bool.Parse(IsRead);
            item.IsCreate = bool.Parse(IsCreate);
            item.IsUpdate = bool.Parse(IsUpdate);
            item.IsDelete = bool.Parse(IsDelete);
            item.IsExport = bool.Parse(IsExport);

            var result = db.BS_AddMenuIntoRole(item);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
    }
}
