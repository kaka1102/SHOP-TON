using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using CS.Data.DataContext;
using CS.Data.Untils;
using MicrosoftHelper;
using WebApplication;
using WebApplication.Controllers;
using SessionUser = CS.Data.Model.SessionUser;

namespace BAN_HANG.Controllers
{
    public class SiMenuBarController : Controller
    {
        // GET: SiMenuBar

        SessionUser user = GetSessionBusiness.GetUser();


        [CompressFilter]
        [WhitespaceFilter]
        public ActionResult Index()
        {
            ViewBag.bar = GetMenuBarPage(null);
            return PartialView();
        }


        [OutputCache(VaryByParam = "none", Duration = 3600)]
        [CompressFilter]
        [WhitespaceFilter]
        public MvcHtmlString GetMenuBarPage(Nullable<int> ParentId)
        {
            StringBuilder sb = new StringBuilder();
            DB_CSEntities1 db = new DB_CSEntities1();

            //get role id and role regarding to role bind this
            var userId = user.Id;
            var RoleId = user.Roleid;

            var q = db.MenuPermission.Where(i => (i.Menu.IsDisplayWebsite == true) && ((i.RoleId == RoleId && i.UserId == userId) || (i.RoleId == RoleId && String.IsNullOrEmpty(i.UserId.ToString())))).OrderByDescending(ob => ob.Menu.SortOrder).ToArray();

            sb.Append("<ul class=\"nav metismenu\" id=\"side-menu\"  >");
            sb.Append("<li class=\"active\"> <a href=\"" + MicrosoftHelper.MSHelper.GetSiteRoot() + "/Home\"> <i class=\"fa fa-dashboard\"></i> <span>Home</span> </a> </li>");

            sb.Append(GetMenuBar(ParentId, q));
            sb.Append("</ul>");
            return MvcHtmlString.Create(sb.ToString());

        }


        [OutputCache(VaryByParam = "none", Duration = 3600)]
        [CompressFilter]
        [WhitespaceFilter]
        public MvcHtmlString GetMenuBar(Nullable<int> ParentId, MenuPermission[] q)
        {

            StringBuilder sb = new StringBuilder();

            if (q != null)
            {
                foreach (var item in q.Where(i => i.Menu.ParentId == ParentId))
                {
                    var js = q;

                    if (js.Count(j => j.Menu.ParentId == item.Menu.Id) > 0)
                    {
                        if (item.Menu.ParentId == null)
                        {
                            sb.Append("<li class=\"treeview\"> <a href=\"#\"> <i class=\"fa fa-folder\"></i> <span>" + item.Menu.MenuText + "</span> <span class=\"fa arrow\"></span>  </a><ul class=\"nav nav-second-level collapse\">");
                        }
                        else
                        {
                            sb.Append("<li class=\"treeview\"> <a href=\"#\"> <i class=\"fa fa-folder\"></i> <span>" + item.Menu.MenuText + "</span> <span class=\"fa arrow\"></span>  </a><ul class=\"nav nav-second-level collapse\">");
                        }
                        sb.Append(GetMenuBar(item.Menu.Id, q));
                    }
                    else
                    {
                        if (item.Menu.ParentId == null)
                        {
                            sb.Append("<li class=\"\"> <a href=\"" + MicrosoftHelper.MSHelper.GetSiteRoot() + "/" + item.Menu.MenuURL + "\"> <i class=\"fa fa-angle-double-right\"></i>  " + item.Menu.MenuText + "</a></li>");
                        }
                        else
                        {
                            sb.Append("<li class=\"\"> <a href=\"" + MicrosoftHelper.MSHelper.GetSiteRoot() + "/" + item.Menu.MenuURL + "\"><i class=\"fa fa-angle-double-right\"></i>  " + item.Menu.MenuText + "</a></li>");
                        }

                    }

                }
                sb.Append("</ul>");
            }

            return MvcHtmlString.Create(sb.ToString());

        }
    }
}