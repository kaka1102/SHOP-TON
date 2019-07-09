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
using WebApplication.Utility;

namespace BAN_HANG.Controllers
{
    public class ManagerUserTreeViewController : BaseController
    {
        // GET: ManagerUserTreeView
        SessionUser user = GetSessionBusiness.GetUser();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _ManagerUsersTreeView(int idcenter = 0)
        {
            return PartialView();
        }
        public ActionResult ManagerUsersTreeViewData(int idcenter = 0)
        {
            var db = new ManagerUserTreeViewBusiness();
            var result = db.GetListUsersTreeView(idcenter);
            return Content(JsonConvert.SerializeObject(result));
        }

        public ActionResult EditUser(int id = 0)
        {
            var db = new ManagerListUserBussiness();
            var result = db.GetUserById(id);
            var roleid = db.GetUserRoleById(id);
            ViewBag.roleid = roleid;
            ViewBag.id = id;
            return PartialView(result);
        }
        public ActionResult _EditUser(string username, string password, string confirmPassword, string fullname, string email, int branch, string code, string birth, string phone, bool status = true, int id = 0, int parent = 0, int roleid = 0, bool isusingaccount = false)
        {
            var db = new ManagerListUserBussiness();

            User user = new User();
            user.Id = id;
            user.Password = password;
            user.FullName = fullname.Trim().ToUpper();
            user.Email = email.Trim().ToUpper();
            user.BranchId = branch;
            user.UserCode = code.Trim().ToUpper();
            user.Phone = phone.Trim().ToUpper();
            user.IsActive = status;
            user.isusingaccount = isusingaccount;
            user.UserName = username;
            if (user.isusingaccount == true)
            {
                if (password != confirmPassword)
                {
                    SystemMessage systemMessage = new SystemMessage();
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Xác nhận mật khẩu không chính xác";
                    return Json(new { result = systemMessage }, JsonRequestBehavior.AllowGet);
                }
                user.Password = password;
            }
            if (parent != 0)
            {
                user.ParentId = parent;
                user.parent_update_by = user.Id;
                user.parent_update_time = DateTime.Now;
            }
            user.DateModify = DateTime.Now;
            user.user_update_by = user.Id;
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
                user.DateOfBirth = _birth;
            }

            var result = db.EditUser(user, user.Id, roleid);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddUser(int centerid = 0, bool loadTree = false)
        {
            ViewBag.centerid = centerid;
            ViewBag.loadTree = loadTree;
            return PartialView();
        }

        public ActionResult _AddUser(string username, string password, string confirmPassword, string fullname, string email, int branch, string code, string birth, string phone, int roleId = 0, int parent = 0, bool isusingaccount = false)
        {
            var db = new ManagerListUserBussiness();

            User itemuser = new User();

            itemuser.FullName = fullname.Trim().ToUpper();
            itemuser.Email = email.Trim().ToUpper();
            itemuser.BranchId = branch;
            itemuser.UserCode = code.Trim().ToUpper();
            itemuser.Phone = phone.Trim().ToUpper();
            itemuser.isusingaccount = isusingaccount;

            if (itemuser.isusingaccount == true)
            {
                if (password != confirmPassword)
                {
                    SystemMessage systemMessage = new SystemMessage();
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.ConfirmPasswordNotCorrect;
                    return Json(new { result = systemMessage }, JsonRequestBehavior.AllowGet);
                }
                itemuser.UserName = username.Trim().ToUpper();
                itemuser.Password = password;
            }
            if (parent != 0)
            {
                itemuser.ParentId = parent;
                itemuser.parent_create_by = user.Id;
                itemuser.parent_create_time = DateTime.Now;
            }
            itemuser.DateCreated = DateTime.Now;
            itemuser.user_create_by = user.Id;
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
                itemuser.DateOfBirth = _birth;
            }
            var myRole = user.Roleid;
            if (myRole != SystemMessageConst.Role.Admin)
            {
                var userId = user.Id;
                var db2 = new CommonBusiness();
                var myLevel = db2.GetLevelMaxByIdAcc(userId);
                var newUserLevel = db2.GetLevelByIdRole(roleId);
                if (myLevel >= newUserLevel)
                {

                    SystemMessage systemMessage = new SystemMessage();
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Quyền không hợp lệ";
                    return Json(new { result = systemMessage }, JsonRequestBehavior.AllowGet);
                }
            };
            var result = db.AddUser(itemuser, roleId);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

    }
}