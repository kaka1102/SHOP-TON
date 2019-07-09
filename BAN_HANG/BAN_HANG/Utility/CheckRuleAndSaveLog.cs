using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CS.Data.DataContext;
using Deaura.Common;

namespace WebApplication.Utility
{
    public class CheckRuleAndSaveLog
    {
        public static void ReturnCheckRuleAndSaveLog(string typeLog,bool stutus,string data)
        {
            var entity = new DB_CSEntities1();

            string controllerName = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();

            var menu = entity.Menu.FirstOrDefault(m => m.MenuURL == controllerName);
            if (menu != null)
            {
               
                sys_log logEntity = new sys_log();
                logEntity.moduleName = controllerName;
                logEntity.type = typeLog;
                logEntity.result = stutus;
                logEntity.description = data;

                new LogApp().WriteDbLog(logEntity);
               
            }
        }
    }
}