using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAN_HANG.Areas.Client.Business;

namespace BAN_HANG.Areas.Client.Controllers
{
    public class GioithieuController : Controller
    {
        // GET: Client/Gioithieu
        public ActionResult Index()
        {
            var db = new GioithieuBusiness();
            var result = db.GetPage_Gioithieu();
            return View(result);
        }
    }
}