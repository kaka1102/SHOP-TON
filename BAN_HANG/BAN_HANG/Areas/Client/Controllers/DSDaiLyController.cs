using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS.Data.Business;

namespace BAN_HANG.Areas.Client.Controllers
{
    public class DSDaiLyController : Controller
    {
        // GET: Client/DSDaiLy
        public ActionResult Index()
        {
            var db = new DSDaiLyBusiness();
            var resuft = db.GetAllBrand();
            return View(resuft);
        }
    }
}