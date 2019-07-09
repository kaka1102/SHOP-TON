using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAN_HANG.Areas.Client.Business;

namespace BAN_HANG.Areas.Client.Controllers
{
    public class ProductController : Controller
    {
        // GET: Client/Product
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadAllCategoryAndProduct()
        {
            var db = new ProductBusiness();
            var result = db.BS_LoadAllCategoryAndProduct();
            return PartialView(result);
        }
    }
}