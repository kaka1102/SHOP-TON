using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using BAN_HANG.Utility;
using CS.Data.DataContext;
using WebApplication.Utility;

namespace BAN_HANG.Controllers
{
    public class MenuController : BaseController
    {

        public ActionResult Index()
        {
         //   ViewBag.TitleWeb = GetTitleWeb.GetDataTitle();
            return ModelBind();
        }


        public ActionResult GetGrid()
        {
            var tak = db.Menu.ToArray();

            var result = from c in tak
                         select new string[] {

                Convert.ToString(c.Id),
                Convert.ToString(c.MenuText),
                Convert.ToString(c.MenuURL),
                Convert.ToString(c.ParentId),
                Convert.ToString(c.SortOrder),
                Convert.ToString(c.IsDisplayWebsite),
                c.Id.ToString(),
             };
            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ModelBindIndex()
        {
            return ModelBind();
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu ObjMenu = db.Menu.Find(id);
            if (ObjMenu == null)
            {
                return HttpNotFound();
            }
            return View(ObjMenu);
        }
        public ActionResult Create()
        {
            ViewBag.ParentId = new SelectList(db.Menu, "Id", "MenuText");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Menu ObjMenu)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {

                    db.Menu.Add(ObjMenu);
                    db.SaveChanges();

                    var getrole = db.Role.FirstOrDefault(m => m.Level == 12).Id;
                    if (getrole != null)
                    {
                        var checkmenu =
                            db.MenuPermission.FirstOrDefault(m => m.MenuId == ObjMenu.Id && m.RoleId == getrole);
                        if (checkmenu == null)
                        {
                            MenuPermission mn = new MenuPermission();
                            mn.MenuId = ObjMenu.Id;
                            mn.RoleId = getrole;
                            mn.IsCreate = true;
                            mn.IsRead = true;
                            mn.IsDelete = true;
                            mn.IsUpdate = true;
                            mn.IsExport = true;
                            db.MenuPermission.Add(mn);
                            db.SaveChanges();
                        }
                    }


                    sb.Append("Sumitted");
                    return Content(sb.ToString());

                }
                else
                {
                    foreach (var key in this.ViewData.ModelState.Keys)
                    {
                        foreach (var err in this.ViewData.ModelState[key].Errors)
                        {
                            sb.Append(err.ErrorMessage + "<br/>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sb.Append("Error :" + ex.Message);
            }

            ViewBag.ParentId = new SelectList(db.Menu, "Id", "MenuText", ObjMenu.ParentId);
            return Content(sb.ToString());

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu ObjMenu = db.Menu.Find(id);
            if (ObjMenu == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(db.Menu, "Id", "MenuText", ObjMenu.ParentId);

            return View(ObjMenu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Menu ObjMenu)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {

                    db.Entry(ObjMenu).State = EntityState.Modified;
                    db.SaveChanges();
                    sb.Append("Sumitted");
                    return Content(sb.ToString());
                }
                else
                {
                    foreach (var key in this.ViewData.ModelState.Keys)
                    {
                        foreach (var err in this.ViewData.ModelState[key].Errors)
                        {
                            sb.Append(err.ErrorMessage + "<br/>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sb.Append("Error :" + ex.Message);
            }

            ViewBag.ParentId = new SelectList(db.Menu, "Id", "MenuText", ObjMenu.ParentId);

            return Content(sb.ToString());

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu ObjMenu = db.Menu.Find(id);
            if (ObjMenu == null)
            {
                return HttpNotFound();
            }
            return View(ObjMenu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StringBuilder sb = new StringBuilder();
            try
            {

                Menu ObjMenu = db.Menu.Find(id);
                db.Menu.Remove(ObjMenu);
                db.SaveChanges();

                sb.Append("Sumitted");
                return Content(sb.ToString());

            }
            catch (Exception ex)
            {
                sb.Append("Error :" + ex.Message);
            }

            return Content(sb.ToString());

        }

        public ActionResult MultiViewIndex(int? id)
        {
            Menu ObjMenu = db.Menu.Find(id);
            ViewBag.IsWorking = 0;
            if (id > 0)
            {
                ViewBag.IsWorking = id;
                ViewBag.ParentId = new SelectList(db.Menu, "Id", "MenuText", ObjMenu.ParentId);

            }

            return View(ObjMenu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult MultiViewIndex(Menu ObjMenu)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {


                    db.Entry(ObjMenu).State = EntityState.Modified;
                    db.SaveChanges();

                    sb.Append("Sumitted");
                    return Content(sb.ToString());
                }
                else
                {
                    foreach (var key in this.ViewData.ModelState.Keys)
                    {
                        foreach (var err in this.ViewData.ModelState[key].Errors)
                        {
                            sb.Append(err.ErrorMessage + "<br/>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sb.Append("Error :" + ex.Message);
            }


            ViewBag.ParentId = new SelectList(db.Menu, "Id", "MenuText", ObjMenu.ParentId);

            return Content(sb.ToString());

        }
        public ActionResult MenuPermissionGetGrid(int id = 0)
        {
            try
            {
                var tak = db.MenuPermission.Where(i => i.MenuId == id).ToArray();

                var result = from c in tak
                             select new string[] {

                        c.Id.ToString(),
                        Convert.ToString(c.Id),
                        Convert.ToString(c.MenuId),
                        Convert.ToString(c.Role.RoleName),
                        Convert.ToString( string.IsNullOrEmpty(c.UserId.ToString()) ? "" : db.User.FirstOrDefault(m=>m.Id==c.UserId.Value).UserName),
                        Convert.ToString(c.SortOrder),
                        Convert.ToString(c.IsCreate),
                        Convert.ToString(c.IsRead),
                        Convert.ToString(c.IsUpdate),
                        Convert.ToString(c.IsDelete),
                    };

                return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private DB_CSEntities1 db = new DB_CSEntities1();

        private ActionResult ModelBind()
        {
            var Menus = db.Menu;
            return View(Menus.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}