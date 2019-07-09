using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Utility;

namespace BAN_HANG.Controllers
{
    public class ds_phieuxuatdahuyController : BaseController
    {
        // GET: ds_phieuxuatdahuy
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadListBill()
        {
            return PartialView();
        }
    }
}