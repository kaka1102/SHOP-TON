using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS.Data.Business;

namespace BAN_HANG.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CheckLogin(string username, string password)
        {

            var db = new LoginBusiness();
            var result = db.AdminCheckLogin(username, password);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Logout()
        {
            Session["session"] = null;
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}