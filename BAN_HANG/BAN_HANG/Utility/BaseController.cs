using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CS.Data.DataContext;
using CS.Data.Model;
using CS.Data.Untils;

namespace WebApplication.Utility
{
    public class BaseController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext ctx)
        {
            var entity = new DB_CSEntities1();




            //Check to see if we need to skip authentication
            //if (ctx.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any()
            //    || ctx.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any())
            //    return;


            //if (!ctx.HttpContext.User.Identity.IsAuthenticated)
            //{
            //    ctx.Result = new RedirectToRouteResult(
            //        new RouteValueDictionary(new { controller = "Login", action = "Index" })
            //    );
            //    return;
            //}
            //else
            //{
            SessionUser user = GetSessionBusiness.GetUser();

            if (user.Id == 0)
            {
                ctx.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Login", action = "Index" })
                );
                return;
            }
            else
            {

                int userid = user.Id;
                int roleiduser = user.Roleid;
                int branchId = user.BranchId;


                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();



                if (controllerName != "User" && controllerName != "SiMenuBar")
                {
                    var checkNewPass = entity.User.FirstOrDefault(m => m.Id == userid);
                    if (checkNewPass != null && checkNewPass.status_password.ToString() == "1")
                    {
                        ctx.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(new { controller = "User", action = "ChangePassword" })
                        );
                        return;
                    }
                }
                string TitleWeb = "";
                var menu = entity.Menu.FirstOrDefault(m => m.MenuURL == controllerName);
                if (menu != null)
                {
                    if (!string.IsNullOrEmpty(menu.ParentId.ToString()))
                    {
                        int id_parent = 0;
                        int.TryParse(menu.ParentId.ToString(), out id_parent);

                        var parent = entity.Menu.FirstOrDefault(m => m.Id == id_parent).MenuText;
                        TitleWeb = parent;
                    }
                    else
                    {
                        TitleWeb += " > " + menu.MenuText;
                    }

                   

                    user.TitleWeb = TitleWeb;

                    var check = entity.MenuPermission.FirstOrDefault(m =>
                        (m.UserId == null || m.UserId == userid ) && m.RoleId == roleiduser && m.MenuId == menu.Id &&
                        m.IsRead == true);

                    if (check == null)
                    {
                        ctx.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(new { controller = "NotFound", action = "Index" })
                        );
                        return;
                    }
                }



            }

            //  }
        }
    }
}

