using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CS.Common.Const;
using CS.Common.Untils;
using CS.Data.DataContext;

namespace WebApplication.Utility
{

    public class CheckActiveMenu
    {

        public static SystemMessage ReturnActive(string type)
        {
            var entity = new DB_CSEntities1();
            SystemMessage sys = new SystemMessage();
            sys.IsSuccess = true;
            sys.Message = "OK";

            //int userid = int.Parse(Env.GetUserInfo("userid"));
            //int roleiduser = int.Parse(Env.GetUserInfo("roleid"));
            //int branchId = int.Parse(Env.GetUserInfo("BranchId"));

            string controllerName = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();

            var menu = entity.Menu.FirstOrDefault(m => m.MenuURL == controllerName);
            if (menu != null)
            {

                //if (type == SystemMessageConst.TypeAction.Addnew)
                //{
                //    var check = entity.MenuPermission.FirstOrDefault(m =>
                //        (m.UserId == userid || m.UserId == null) && m.RoleId == roleiduser && m.MenuId == menu.Id &&
                //        m.IsCreate == true);
                //    if (check == null)
                //    {
                //        sys.IsSuccess = false;
                //        sys.Message = SystemMessageConst.systemmessage.NotRoleCreate;
                //    }
                //}
                //if (type == SystemMessageConst.TypeAction.Update)
                //{
                //    var check = entity.MenuPermission.FirstOrDefault(m =>
                //        (m.UserId == userid || m.UserId == null) && m.RoleId == roleiduser && m.MenuId == menu.Id &&
                //        m.IsUpdate == true);
                //    if (check == null)
                //    {
                //        sys.IsSuccess = false;
                //        sys.Message = SystemMessageConst.systemmessage.NotRoleUpdate;
                //    }
                //}
                //if (type == SystemMessageConst.TypeAction.Delete)
                //{
                //    var check = entity.MenuPermission.FirstOrDefault(m =>
                //        (m.UserId == userid || m.UserId == null) && m.RoleId == roleiduser && m.MenuId == menu.Id &&
                //        m.IsDelete == true);
                //    if (check == null)
                //    {
                //        sys.IsSuccess = false;
                //        sys.Message = SystemMessageConst.systemmessage.NotRoleDelete;
                //    }
                //}

                //if (type == SystemMessageConst.TypeAction.Export)
                //{
                //    var check = entity.MenuPermission.FirstOrDefault(m =>
                //        (m.UserId == userid || m.UserId == null) && m.RoleId == roleiduser && m.MenuId == menu.Id &&
                //        m.IsExport == true);
                //    if (check == null)
                //    {
                //        sys.IsSuccess = false;
                //        sys.Message = SystemMessageConst.systemmessage.NotRoleExport;
                //    }
                //}
            }

            return sys;
        }

    }
}