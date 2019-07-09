using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CS.Data.DataContext;
using Deaura.Common;
using WebApplication;

namespace BAN_HANG.Utility
{
    public static class GetTitleWeb
    {
        public static string GetDataTitle()
        {
            string TitleWeb = "";

            var entity = new DB_CSEntities1();

            string controllerName = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();

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

                TitleWeb += " > " + menu.MenuText;
            }

            return TitleWeb;
        }

    }
}